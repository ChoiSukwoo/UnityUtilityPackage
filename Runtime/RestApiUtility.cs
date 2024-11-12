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
		// ������ ������ GetJsonWithAuth
		public static IEnumerator GetJsonWithAuth<T>(string url, string authToken, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			SetAuthHeader(ref headers, authToken);
			yield return GetJson(url, onComplete, onProgress, headers);
		}

		// �⺻ GetJson
		public static IEnumerator GetJson<T>(string url, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			//Josn �Ľ�
			ApiResponse<string> apiRes = null;
			yield return GetText(url, (res => apiRes = res), onProgress, headers);
			//����ó��
			if(!apiRes.Success) {
				onComplete?.Invoke(new FailureResponse<T>(apiRes.ErrorMessage));
				yield break;
			}

			try {
				var data = JsonConvert.DeserializeObject<T>(apiRes.Data);
				onComplete?.Invoke(new SuccessResponse<T>(data));
			} catch(System.Exception ex) {
				onComplete?.Invoke(new FailureResponse<T>($"JSON �Ľ� ����: {ex.Message}"));
			}
		}
		#endregion

		#region Texture
		// ������ �ʿ��� GetTextureWithAuth
		public static IEnumerator GetTextureWithAuth(string url, string authToken, UnityAction<ApiResponse<Texture2D>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			SetAuthHeader(ref headers, authToken);
			yield return GetTexture(url, onComplete, onProgress, headers);
		}

		// �⺻ GetTexture
		public static IEnumerator GetTexture(string url, UnityAction<ApiResponse<Texture2D>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {

			using(UnityWebRequest request = UnityWebRequestTexture.GetTexture(url)) {
				//��� ����
				SetRequestHeaders(request, headers);

				//��û ���� ���
				RequestDebug(request, headers);

				// ��û�� ������ ���� ��Ȳ�� ������Ʈ
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				//��û ��� ���
				ResultDebug(request);

				// ��û�� �Ϸ�� �� ��� Ȯ��
				if(request.result == UnityWebRequest.Result.Success) {
					// �������� ������ Texture2D Ÿ������ ����
					var texture = DownloadHandlerTexture.GetContent(request);
					onComplete?.Invoke(new SuccessResponse<Texture2D>(texture));
				} else {
					// ���� �� ���� �޽����� ����
					onComplete?.Invoke(new FailureResponse<Texture2D>(request.error));
				}
			}

		}
		#endregion

		#region Sound
		// ������ �ʿ��� GetSoundWithAuth
		public static IEnumerator GetSoundWithAuth(string url, string authToken, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<AudioClip>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			SetAuthHeader(ref headers, authToken);
			yield return GetSound(url, audioType, onComplete, onProgress, headers);
		}

		// �⺻ GetSound
		public static IEnumerator GetSound(string url, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<AudioClip>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			using(UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, audioType)) {
				//��� ����
				SetRequestHeaders(request, headers);

				//��û ���� ���
				RequestDebug(request, headers);

				// ��û�� ������ ���� ��Ȳ�� ������Ʈ
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				//��û ��� ���
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
		// ������ �ʿ��� GetAssetWithAuth
		public static IEnumerator GetAssetWithAuth(string url, string authToken, UnityAction<ApiResponse<AssetBundle>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			SetAuthHeader(ref headers, authToken);
			yield return GetAsset(url, onComplete, onProgress, headers);
		}

		// �⺻ GetAsset
		public static IEnumerator GetAsset(string url, UnityAction<ApiResponse<AssetBundle>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			using(UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url)) {
				//��� ����
				SetRequestHeaders(request, headers);

				//��û ���� ���
				RequestDebug(request, headers);

				// ��û ����
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				//��û ��� ���
				ResultDebug(request);

				// ��û�� �Ϸ�� �� ��� Ȯ��
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
		// ������ �ʿ��� GetVideoWithAuth
		public static IEnumerator GetVideoWithAuth(string url, string authToken, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			SetAuthHeader(ref headers, authToken);
			yield return GetVideo(url, onComplete, onProgress, headers);
		}

		// �⺻ GetVideo
		public static IEnumerator GetVideo(string url, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			ApiResponse<byte[]> apiRes = null;

			// ������ byte[]�� �ٿ�ε�
			yield return GetByte(url, (res => apiRes = res), onProgress, headers);

			if(!apiRes.Success) {
				onComplete?.Invoke(new FailureResponse<string>(apiRes.ErrorMessage));
				yield break;
			}

			string tempPath = Path.Combine(Application.persistentDataPath, "temp_video.mp4");
			File.WriteAllBytes(tempPath, apiRes.Data);

			Debug.Log("Temporary video path: " + tempPath);

			// 3. VideoClip ����
			onComplete?.Invoke(new SuccessResponse<string>(tempPath));
		}
		#endregion


		// �ؽ�Ʈ �����͸� �������� �޼���
		public static IEnumerator GetText(string url, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			using(UnityWebRequest request = UnityWebRequest.Get(url)) {
				SetRequestHeaders(request, headers);

				//��û ���� ���
				RequestDebug(request, headers);

				// ��û ����
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				//��û ��� ���
				ResultDebug(request);

				// ��û�� �Ϸ�� �� ��� Ȯ��
				if(request.result == UnityWebRequest.Result.Success) {
					onComplete?.Invoke(new SuccessResponse<string>(request.downloadHandler.text));
				} else {
					onComplete?.Invoke(new FailureResponse<string>(request.error));
				}
			}
		}

		// ����Ʈ �����͸� �������� �޼���
		public static IEnumerator GetByte(string url, UnityAction<ApiResponse<byte[]>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			using(UnityWebRequest request = UnityWebRequest.Get(url)) {
				//��� ����
				SetRequestHeaders(request, headers);

				//��û ���� ���
				RequestDebug(request, headers);

				// ��û ����
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				//��û ��� ���
				ResultDebug(request);

				// ��û �Ϸ� �� ��� Ȯ��
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
			// ��û �Ϸ� �� ������� 100%�� ����
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

			Debug.Log(reqInfo.ToString()); // ���� ���� �α� �� �� ���
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
					// Content-Type�� �ؽ�Ʈ �迭�̸� ������ ��ȿ�� �ؽ�Ʈ�� ��
					resInfo.AppendLine($"	Response Text: {request.downloadHandler.text}");
				} else if(request.downloadHandler.data != null && request.downloadHandler.data.Length > 0) {
					// ���̳ʸ� �������� ���
					resInfo.AppendLine($"	Response Data (binary): {request.downloadHandler?.data?.Length ?? 0} bytes");
				} else {
					// ���� �����Ͱ� ��� �ִ� ���
					resInfo.AppendLine("	No response data available.");
				}
			} else {
				resInfo.AppendLine($"Error: {request.error}");
			}

			Debug.Log(resInfo.ToString()); // ���� ���� �α� �� �� ���
		}
	}

}