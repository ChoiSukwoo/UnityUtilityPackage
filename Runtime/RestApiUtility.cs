using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Suk {
	public static class RestApiUtility {

		#region Json
		// 인증을 포함한 GetJsonWithAuth
		public static IEnumerator GetJsonWithAuth<T>(string url, string authToken, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			SetAuthHeader(ref headers, authToken);
			yield return GetJson(url, onComplete, onProgress, headers);
		}

		// 기본 GetJson
		public static IEnumerator GetJson<T>(string url, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			//Josn 파싱
			ApiResponse<string> apiRes = null;
			yield return GetText(url, (res => apiRes = res), onProgress, headers);
			//실패처리
			if(!apiRes.Success) {
				onComplete?.Invoke(new FailureResponse<T>(apiRes.ErrorMessage));
				yield break;
			}

			try {
				var data = JsonConvert.DeserializeObject<T>(apiRes.Data);
				onComplete?.Invoke(new SuccessResponse<T>(data));
			} catch(System.Exception ex) {
				onComplete?.Invoke(new FailureResponse<T>($"JSON 파싱 오류: {ex.Message}"));
			}
		}
		#endregion

		#region Texture
		// 인증이 필요한 GetTextureWithAuth
		public static IEnumerator GetTextureWithAuth(string url, string authToken, UnityAction<ApiResponse<Texture2D>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			SetAuthHeader(ref headers, authToken);
			yield return GetTexture(url, onComplete, onProgress, headers);
		}

		// 기본 GetTexture
		public static IEnumerator GetTexture(string url, UnityAction<ApiResponse<Texture2D>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {

			using(UnityWebRequest request = UnityWebRequestTexture.GetTexture(url)) {
				//헤더 설정
				SetRequestHeaders(request, headers);

				//요청 정보 출력
				RequestDebug(request, headers);

				// 요청을 보내고 진행 상황을 업데이트
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				//요청 결과 출력
				ResultDebug(request);

				// 요청이 완료된 후 결과 확인
				if(request.result == UnityWebRequest.Result.Success) {
					// 성공적인 응답을 Texture2D 타입으로 전달
					var texture = DownloadHandlerTexture.GetContent(request);
					onComplete?.Invoke(new SuccessResponse<Texture2D>(texture));
				} else {
					// 실패 시 에러 메시지를 전달
					onComplete?.Invoke(new FailureResponse<Texture2D>(request.error));
				}
			}

		}
		#endregion

		#region Sound
		// 인증이 필요한 GetSoundWithAuth
		public static IEnumerator GetSoundWithAuth(string url, string authToken, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<AudioClip>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			SetAuthHeader(ref headers, authToken);
			yield return GetSound(url, audioType, onComplete, onProgress, headers);
		}

		// 기본 GetSound
		public static IEnumerator GetSound(string url, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<AudioClip>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			using(UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, audioType)) {
				//헤더 설정
				SetRequestHeaders(request, headers);

				//요청 정보 출력
				RequestDebug(request, headers);

				// 요청을 보내고 진행 상황을 업데이트
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				//요청 결과 출력
				ResultDebug(request);

				if(request.result == UnityWebRequest.Result.Success) {
					AudioClip audioClip = DownloadHandlerAudioClip.GetContent(request);
					onComplete?.Invoke(new SuccessResponse<AudioClip>(audioClip));
				} else {
					onComplete?.Invoke(new FailureResponse<AudioClip>(request.error));
				}
			}
		}
		#endregion

		#region Asset
		// 인증이 필요한 GetAssetWithAuth
		public static IEnumerator GetAssetWithAuth(string url, string authToken, UnityAction<ApiResponse<AssetBundle>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			SetAuthHeader(ref headers, authToken);
			yield return GetAsset(url, onComplete, onProgress, headers);
		}

		// 기본 GetAsset
		public static IEnumerator GetAsset(string url, UnityAction<ApiResponse<AssetBundle>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			using(UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url)) {
				//헤더 셋팅
				SetRequestHeaders(request, headers);

				//요청 정보 출력
				RequestDebug(request, headers);

				// 요청 전송
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				//요청 결과 출력
				ResultDebug(request);

				// 요청이 완료된 후 결과 확인
				if(request.result == UnityWebRequest.Result.Success) {
					AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);
					onComplete?.Invoke(new SuccessResponse<AssetBundle>(assetBundle));
				} else {
					onComplete?.Invoke(new FailureResponse<AssetBundle>(request.error));
				}
			}
		}
		#endregion


		#region Video
		// 인증이 필요한 GetVideoWithAuth
		public static IEnumerator GetVideoWithAuth(string url, string authToken, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			SetAuthHeader(ref headers, authToken);
			yield return GetVideo(url, onComplete, onProgress, headers);
		}

		// 기본 GetVideo
		public static IEnumerator GetVideo(string url, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			ApiResponse<byte[]> apiRes = null;

			// 비디오를 byte[]로 다운로드
			yield return GetByte(url, (res => apiRes = res), onProgress, headers);

			if(!apiRes.Success) {
				onComplete?.Invoke(new FailureResponse<string>(apiRes.ErrorMessage));
				yield break;
			}

			string tempPath = Path.Combine(Application.persistentDataPath, "temp_video.mp4");
			File.WriteAllBytes(tempPath, apiRes.Data);

			Debug.Log("Temporary video path: " + tempPath);

			// 3. VideoClip 전달
			onComplete?.Invoke(new SuccessResponse<string>(tempPath));
		}
		#endregion


		// 텍스트 데이터를 가져오는 메서드
		public static IEnumerator GetText(string url, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			using(UnityWebRequest request = UnityWebRequest.Get(url)) {
				SetRequestHeaders(request, headers);

				//요청 정보 출력
				RequestDebug(request, headers);

				// 요청 전송
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				//요청 결과 출력
				ResultDebug(request);

				// 요청이 완료된 후 결과 확인
				if(request.result == UnityWebRequest.Result.Success) {
					onComplete?.Invoke(new SuccessResponse<string>(request.downloadHandler.text));
				} else {
					onComplete?.Invoke(new FailureResponse<string>(request.error));
				}
			}
		}

		// 바이트 데이터를 가져오는 메서드
		public static IEnumerator GetByte(string url, UnityAction<ApiResponse<byte[]>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			using(UnityWebRequest request = UnityWebRequest.Get(url)) {
				//헤더 셋팅
				SetRequestHeaders(request, headers);

				//요청 정보 출력
				RequestDebug(request, headers);

				// 요청 전송
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				//요청 결과 출력
				ResultDebug(request);

				// 요청 완료 후 결과 확인
				if(request.result == UnityWebRequest.Result.Success) {
					onComplete?.Invoke(new SuccessResponse<byte[]>(request.downloadHandler.data));
				} else {
					onComplete?.Invoke(new FailureResponse<byte[]>(request.error));
				}
			}
		}


		private static void SetAuthHeader(ref Dictionary<string, string> headers, string authToken) {
			if(headers == null)
				headers = new Dictionary<string, string>();
			headers["Authorization"] = $"Bearer {authToken}";
		}

		private static void SetRequestHeaders(UnityWebRequest request, Dictionary<string, string> headers) {
			if(headers != null) {
				foreach((string key, string value) in headers) {
					request.SetRequestHeader(key, value);
				}
			}
		}

		private static IEnumerator UpdateProgress(UnityWebRequestAsyncOperation asyncOperation, UnityAction<float> onProgress) {
			float lastProgressUpdate = Time.time;
			while(!asyncOperation.isDone) {
				if(Time.time >= lastProgressUpdate + 1f) {
					onProgress?.Invoke(asyncOperation.progress);
					lastProgressUpdate = Time.time;
				}
				yield return null;
			}
			// 요청 완료 시 진행률을 100%로 설정
			onProgress?.Invoke(1f);
		}

		static void RequestDebug(UnityWebRequest request, Dictionary<string, string> headers) {
			var reqInfo = new System.Text.StringBuilder();

			reqInfo.AppendLine("[RestApiUtility] Sending Request:")
				.AppendLine($"Method: {request.method}")
				.AppendLine($"URL: {request.url}")
				.AppendLine("Headers:");

			if(headers != null) {
				foreach(var header in headers)
					reqInfo.AppendLine($"  {header.Key}: {header.Value}");
			} else {
				reqInfo.AppendLine("  No headers provided.");
			}

			Debug.Log(reqInfo.ToString()); // 응답 정보 로그 한 번 출력
		}

		static void ResultDebug(UnityWebRequest request) {
			var resInfo = new System.Text.StringBuilder();
			string contentType = request.GetResponseHeader("Content-Type");

			resInfo.AppendLine("[RestApiUtility] Request Completed.")
				.AppendLine($"Status: {(request.result == UnityWebRequest.Result.Success ? "Success" : "Failure")}")
				.AppendLine($"contentType: {contentType}")
				.AppendLine($"Status Code: {request.responseCode}")
				.AppendLine($"Uploaded Bytes: {request.uploadedBytes}")
				.AppendLine($"Downloaded Bytes: {request.downloadedBytes}")
				.AppendLine($"Response Data Length: {request.downloadHandler?.data?.Length ?? 0} bytes");


			if(request.result == UnityWebRequest.Result.Success) {
				resInfo.AppendLine("Request completed successfully.");

				if(!string.IsNullOrEmpty(contentType) && contentType.Contains("text") && !string.IsNullOrEmpty(request.downloadHandler.text)) {
					// Content-Type이 텍스트 계열이며 응답이 유효한 텍스트일 때
					resInfo.AppendLine($"	Response Text: {request.downloadHandler.text}");
				} else if(request.downloadHandler.data != null && request.downloadHandler.data.Length > 0) {
					// 바이너리 데이터인 경우
					resInfo.AppendLine($"	Response Data (binary): {request.downloadHandler?.data?.Length ?? 0} bytes");
				} else {
					// 응답 데이터가 비어 있는 경우
					resInfo.AppendLine("	No response data available.");
				}
			} else {
				resInfo.AppendLine($"Error: {request.error}");
			}

			Debug.Log(resInfo.ToString()); // 응답 정보 로그 한 번 출력
		}
	}

}