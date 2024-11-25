using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static Suk.RestApi.RestApiState;



namespace Suk.RestApi
{
	internal static class RestApiDebug
	{
		public static void Request(UnityWebRequest request, Dictionary<string, string> headers)
		{

			//��� �ź�
			if (!enableDebugLog)
				return;

			var reqInfo = new StringBuilder();

			reqInfo.AppendLine("[RestApiUtility] Sending Request:")
				.AppendLine($"Method: {request.method}")
				.AppendLine($"URL: {request.url}")
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

			// POST/PUT ��û�� ������ ���
			if (request.method == UnityWebRequest.kHttpVerbPOST || request.method == UnityWebRequest.kHttpVerbPUT)
			{
				if (request.uploadHandler?.data != null)
				{
					string bodyData = System.Text.Encoding.UTF8.GetString(request.uploadHandler.data);
					reqInfo.AppendLine("Body Data:").AppendLine(bodyData);
				}
				else
				{
					reqInfo.AppendLine("No body data provided.");
				}
			}

			Debug.Log(reqInfo.ToString()); // ���� ���� �α� �� �� ���
		}


		public static void Result(UnityWebRequest request, ContentTypeState contentTypeState)
		{

			//��� �ź�
			if (!enableDebugLog)
				return;

			var resInfo = new StringBuilder();
			string contentType = request.GetResponseHeader("Content-Type");

			resInfo.AppendLine("[RestApiUtility] Request Completed.")
					.AppendLine($"Status: {(request.result == UnityWebRequest.Result.Success ? "Success" : "Failure")}")
					.AppendLine($"Content-Type: {contentType}")
					.AppendLine($"Status Code: {request.responseCode}")
					.AppendLine($"Uploaded Bytes: {request.uploadedBytes}")
					.AppendLine($"Downloaded Bytes: {request.downloadedBytes} bytes");


			// ��û ���� ���� Ȯ��
			if (request.result != UnityWebRequest.Result.Success)
			{
				resInfo.AppendLine($"Status: Failure").AppendLine($"Error: {request.error}");
				Debug.Log(resInfo.ToString());
				return; // ���� �� �߰� ����� �ߴ�
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

			Debug.Log(resInfo.ToString()); // ���� ���� �α� �� �� ���
		}



	}
}