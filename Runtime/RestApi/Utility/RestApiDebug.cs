using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine.Networking;

namespace Suk.RestApi
{
	internal static class RestApiDebug
	{
		public static void Request(UnityWebRequest request, Dictionary<string, string> headers, Stopwatch stopwatch)
		{
			//출력 거부
			if (!RestApiState.enableDebugLog)
				return;

			var reqInfo = new StringBuilder();

			reqInfo.AppendLine("[RestApiUtility] Sending Request:")
				.AppendLine($"Method: {request.method}")
				.AppendLine($"URL: {request.url}")
				.AppendLine($"Start Time: {System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}")
				.AppendLine("Headers:");

			if (headers != null)
			{
				foreach (var header in headers)
					reqInfo.AppendLine($"  {header.Key}: {header.Value}");
			}
			else
			{
				reqInfo.AppendLine("  No headers provided.");
			}

			// POST/PUT 요청의 데이터 출력
			if (request.method == UnityWebRequest.kHttpVerbPOST || request.method == UnityWebRequest.kHttpVerbPUT)
			{
				if (request.uploadHandler?.data != null)
				{
					string bodyData = Encoding.UTF8.GetString(request.uploadHandler.data);
					reqInfo.AppendLine("Body Data:").AppendLine(bodyData);
				}
				else
				{
					reqInfo.AppendLine("No body data provided.");
				}
			}

			UnityEngine.Debug.Log(reqInfo.ToString()); // 응답 정보 로그 한 번 출력
		}

		public static void Result(UnityWebRequest request, ContentTypeState contentTypeState, Stopwatch stopwatch)
		{
			//출력 거부
			if (!RestApiState.enableDebugLog)
				return;

			stopwatch.Stop();
			long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

			string endTime = System.DateTime.UtcNow.ToString("o");
			var resInfo = new StringBuilder();
			string contentType = request.GetResponseHeader("Content-Type");

			resInfo.AppendLine("[RestApiUtility] Request Completed.")
					.AppendLine($"Status: {(request.result == UnityWebRequest.Result.Success ? "Success" : "Failure")}")
					.AppendLine($"End Time: {System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}")
					.AppendLine($"Elapsed Time: {elapsedMilliseconds} ms")
					.AppendLine($"Content-Type: {contentType}")
					.AppendLine($"Status Code: {request.responseCode}")
					.AppendLine($"Uploaded Bytes: {request.uploadedBytes}")
					.AppendLine($"Downloaded Bytes: {request.downloadedBytes} bytes");


			// 요청 성공 여부 확인
			if (request.result != UnityWebRequest.Result.Success)
			{
				resInfo.AppendLine($"Status: Failure").AppendLine($"Error: {request.error}");
				UnityEngine.Debug.Log(resInfo.ToString());
				return; // 실패 시 추가 디버깅 중단
			}

			resInfo.AppendLine("Status: Success");
			if (contentTypeState == ContentTypeState.Text)
			{
				string jsonResponse = request.downloadHandler.text;
				resInfo.AppendLine($"Response (Text):\n{jsonResponse}");
			}
			else
			{
				if (request.downloadedBytes > 0)
				{
					resInfo.AppendLine($"Response ({contentTypeState}): {request.downloadedBytes} bytes (binary)");
				}
				else
				{
					resInfo.AppendLine($"No {contentTypeState.ToString().ToLower()} data available.");
				}
			}

			UnityEngine.Debug.Log(resInfo.ToString()); // 응답 정보 로그 한 번 출력
		}
	}
}