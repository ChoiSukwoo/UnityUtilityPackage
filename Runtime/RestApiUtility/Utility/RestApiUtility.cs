using Cysharp.Threading.Tasks;
using Suk.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using static Suk.RestApi.RestApiState;

namespace Suk.RestApi {
	internal static class RestApiUtility {

		//���� ��ū ���
		public static void SetAuthHeader(ref Dictionary<string, string> headers, string authToken) {
			if(headers == null)
				headers = new Dictionary<string, string>();
			headers["Authorization"] = $"Bearer {authToken}";
		}

		public static void SetContentHeader(ref Dictionary<string, string> headers, string contentType) {
			if(headers == null)
				headers = new Dictionary<string, string>();
			headers["Content-Type"] = contentType;
		}

		//��� ����
		public static void SetRequestHeaders(UnityWebRequest request, Dictionary<string, string> headers) {
			if(headers != null) {
				foreach((string key, string value) in headers) {
					request.SetRequestHeader(key, value);
				}
			}
		}

		public static DownloadHandler CreateDownloadHandler(string url, ContentTypeState expectedType, AudioType audioType = AudioType.UNKNOWN, uint crc = 0) {
			switch(expectedType) {
				case ContentTypeState.Audio:
				return new DownloadHandlerAudioClip(url, audioType);
				case ContentTypeState.Image:
				return new DownloadHandlerTexture();
				case ContentTypeState.Asset:
				return new DownloadHandlerAssetBundle(url, crc);
				default:
				return new DownloadHandlerBuffer(); // �⺻ �ڵ鷯
			}
		}


		// Content-Type ��ȿ�� �˻� �Լ�
		public static bool ValidateContentType(UnityWebRequest request, ref ContentTypeState expectedType) {
			if(expectedType == ContentTypeState.Unknown) {
				string contentType = request.GetResponseHeader("Content-Type");
				if(!TryParseContentType(contentType, out expectedType)) {
					RestApiDebug.Result(request, expectedType);
					return false;
				}
			}
			return true;
		}

		public static T ParseResponse<T>(UnityWebRequest request, ContentTypeState expectedType) {
			switch(expectedType) {
				case ContentTypeState.Image:
				return (T)(object)DownloadHandlerTexture.GetContent(request);
				case ContentTypeState.Audio:
				return (T)(object)DownloadHandlerAudioClip.GetContent(request);
				case ContentTypeState.Asset:
				return (T)(object)DownloadHandlerAssetBundle.GetContent(request);
				case ContentTypeState.Text:
				return (T)(object)request.downloadHandler.text;
				case ContentTypeState.Binary:
				case ContentTypeState.Video:
				return (T)(object)request.downloadHandler.data;
				default:
				throw new Exception("Unsupported content type.");
			}
		}

		// ContentType �Ľ� �޼���
		public static bool TryParseContentType(string contentType, out ContentTypeState contentTypeId) {
			contentTypeId = ContentTypeState.Unknown;

			//contentType�� �˼� ����
			if(string.IsNullOrEmpty(contentType))
				return false;

			// Content-Type�� ���� Ÿ�� �Ľ�
			string mainType = contentType.Split(';')[0].Trim().ToLower();

			if(mainType.StartsWith("text/") || mainType.Contains("json") || mainType.Contains("xml")) {
				contentTypeId = ContentTypeState.Text;
				return true;
			} else if(mainType.StartsWith("image/")) {
				contentTypeId = ContentTypeState.Image;
				return true;
			} else if(mainType.StartsWith("video/")) {
				contentTypeId = ContentTypeState.Video;
				return true;
			} else if(mainType.StartsWith("audio/")) {
				contentTypeId = ContentTypeState.Audio;
				return true;
			} else if(mainType.StartsWith("application/octet-stream") || mainType.Contains("assetbundle")) {
				contentTypeId = ContentTypeState.Asset;
				return true;
			} else {
				contentTypeId = ContentTypeState.Binary;
				return true;
			}
		}

		//1%�̻� �����ϰų� 1�ʰ� ������ 
		public static IEnumerator UpdateProgress(UnityWebRequestAsyncOperation asyncOperation, UnityAction<float> onProgress) {
			float lastProgress = 0f;
			float lastProgressUpdate = Time.time;

			while(!asyncOperation.isDone) {
				if(Time.time >= lastProgressUpdate + minUpdateInterval || Mathf.Abs(asyncOperation.progress - lastProgress) >= minProgressChange) {
					onProgress?.Invoke(asyncOperation.progress);
					lastProgress = asyncOperation.progress;
					lastProgressUpdate = Time.time;
				}
				yield return null;
			}
			// ��û �Ϸ� �� ������� 100%�� ����
			onProgress?.Invoke(1f);
		}

		public static async UniTask UpdateProgress(UnityWebRequestAsyncOperation asyncOperation, UnityAction<float> progress = null, CancellationToken token = default) {
			float lastProgress = 0f;
			float lastProgressUpdate = Time.time;

			while(!asyncOperation.isDone) {
				if(Time.time >= lastProgressUpdate + minUpdateInterval || Mathf.Abs(asyncOperation.progress - lastProgress) >= minProgressChange) {
					progress?.Invoke(asyncOperation.progress); // ����� ������Ʈ
					lastProgress = asyncOperation.progress;
					lastProgressUpdate = Time.time;
				}
				await UniTask.Yield(PlayerLoopTiming.Update, token); // ������ ��� (��� ��ū ����)
			}

			progress?.Invoke(1f);
		}

		public static string GetAudioMimeType(AudioContentType contentType) {
			return contentType switch {
				AudioContentType.MP3 => "audio/mpeg",
				AudioContentType.Wav => "audio/wav",
				AudioContentType.Ogg => "audio/ogg",
				_ => throw new NotSupportedException($"Unsupported Audio Content-Type: {contentType}")
			};
		}
		public static AudioType ConvertToUnityAudioType(AudioContentType contentType) {
			return contentType switch {
				AudioContentType.MP3 => AudioType.MPEG,
				AudioContentType.Wav => AudioType.WAV,
				AudioContentType.Ogg => AudioType.OGGVORBIS,
				_ => AudioType.UNKNOWN // �������� �ʴ� Ÿ���� UNKNOWN���� ó��
			};
		}

		public static string GetVideoMimeType(VideoContentType contentType) {
			return contentType switch {
				VideoContentType.Mp4 => "video/mp4",
				VideoContentType.Webm => "video/webm",
				_ => throw new NotSupportedException($"Unsupported Video Content-Type: {contentType}")
			};
		}

		public static string GetImageMimeType(ImageContentType contentType) {
			return contentType switch {
				ImageContentType.Png => "image/png",
				ImageContentType.Jpeg => "image/jpeg",
				_ => throw new NotSupportedException($"Unsupported Image Content-Type: {contentType}")
			};
		}

		public static IEnumerator ExecuteWithAuth(string authToken, Dictionary<string, string> headers, Func<IEnumerator> execute) {
			SetAuthHeader(ref headers, authToken);
			yield return execute();
		}

		public static async UniTask ExecuteWithAuth(string authToken, Dictionary<string, string> headers, Func<UniTask> execute) {
			SetAuthHeader(ref headers, authToken); // ���� ��� ����
			await execute(); // �񵿱� �۾� ����
		}

		public static void HandleJsonResponse<T>(ApiResponse<string> apiResponse, UnityAction<ApiResponse<T>> onComplete = null) {
			if(!apiResponse.Success) {
				onComplete?.Invoke(new FailureResponse<T>(apiResponse.ErrorMessage));
				return;
			}
			try {
				var parsedData = JsonParser.Parse<T>(apiResponse.Data);
				onComplete?.Invoke(new SuccessResponse<T>(parsedData));
			} catch(Exception ex) {
				onComplete?.Invoke(new FailureResponse<T>($"JSON �Ľ� ����: {ex.Message}"));
			}
		}

		public static T HandleJsonResponse<T>(string jsonResponse) {
			try {
				var parsedData = JsonParser.Parse<T>(jsonResponse);
				return parsedData;
			} catch(Exception ex) {
				throw new Exception($"JSON �Ľ� ����: {ex.Message}");
			}
		}

		public static void HandleVideoResponse(ApiResponse<byte[]> apiResponse, string savePath, UnityAction<ApiResponse<string>> onComplete = null) {
			if(!apiResponse.Success) {
				onComplete?.Invoke(new FailureResponse<string>(apiResponse.ErrorMessage));
				return;
			}

			try {
				File.WriteAllBytes(savePath, apiResponse.Data); // ���� ����
				Debug.Log($"Video saved at: {savePath}");
				onComplete?.Invoke(new SuccessResponse<string>(savePath)); // ���� �� ���� ��� ��ȯ
			} catch(Exception ex) {
				onComplete?.Invoke(new FailureResponse<string>($"Failed to save video: {ex.Message}"));
			}
		}

		public static string HandleVideoResponse(byte[] videoData, string savePath) {
			try {
				// ���� �����͸� ���Ϸ� ����
				File.WriteAllBytes(savePath, videoData);
				Debug.Log($"Video saved at: {savePath}");
				return savePath;
			} catch(Exception ex) {
				// ���� �߻� �� �������� ó�� ����
				throw new Exception($"Failed to save video: {ex.Message}");
			}
		}



		public static IEnumerator HandleNoneSend(Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			byte[] bodyData = new byte[0]; // �� ������
			yield return execute(bodyData);
		}

		public static async UniTask HandleNoneSend(Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			byte[] bodyData = new byte[0]; // �� ������
			await execute(bodyData);
		}


		public static IEnumerator HandleTextSend(string text, string contentType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			SetContentHeader(ref headers, contentType);
			byte[] bodyData = System.Text.Encoding.UTF8.GetBytes(text);
			yield return execute(bodyData);
		}

		public static async UniTask HandleTextSend(string text, string contentType, Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			SetContentHeader(ref headers, contentType);
			byte[] bodyData = System.Text.Encoding.UTF8.GetBytes(text);
			await execute(bodyData);
		}


		public static IEnumerator HandleBinarySend(byte[] binaryData, string contentType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			SetContentHeader(ref headers, contentType);
			yield return execute(binaryData);
		}

		public static async UniTask HandleBinarySend(byte[] binaryData, string contentType, Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			SetContentHeader(ref headers, contentType);
			await execute(binaryData);
		}

		public static IEnumerator HandleJsonSend<T>(T body, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			SetContentHeader(ref headers, "application/json");
			byte[] bodyData = JsonParser.ToByteArray(body);
			yield return execute(bodyData);
		}

		public static async UniTask HandleJsonSend<T>(T body, Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			SetContentHeader(ref headers, "application/json");
			byte[] bodyData = JsonParser.ToByteArray(body);
			await execute(bodyData);
		}

		public static IEnumerator HandleAudioSend(byte[] audioData, AudioContentType audioType, Dictionary<string, string> headers, Func<byte[], AudioType, IEnumerator> execute) {
			string contentType = GetAudioMimeType(audioType); // AudioContentType�� ���� MIME Ÿ�� ����
			SetContentHeader(ref headers, contentType); // ��� ����
			AudioType unityAudioType = ConvertToUnityAudioType(audioType); // Unity���� ����ϴ� AudioType���� ��ȯ
			yield return execute(audioData, unityAudioType); // �غ�� �����͸� ���� �ܰ�� ����
		}

		public static async UniTask HandleAudioSend(byte[] audioData, AudioContentType audioType, Dictionary<string, string> headers, Func<byte[], AudioType, UniTask> execute) {
			string contentType = GetAudioMimeType(audioType); // AudioContentType�� ���� MIME Ÿ�� ����
			SetContentHeader(ref headers, contentType); // ��� ����
			AudioType unityAudioType = ConvertToUnityAudioType(audioType); // Unity���� ����ϴ� AudioType���� ��ȯ
			await execute(audioData, unityAudioType);
		}

		public static IEnumerator HandleVideoSend(byte[] videoData, VideoContentType videoType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			string contentType = GetVideoMimeType(videoType);
			SetContentHeader(ref headers, contentType);
			yield return execute(videoData);
		}

		public static async UniTask HandleVideoSend(byte[] videoData, VideoContentType videoType, Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			string contentType = GetVideoMimeType(videoType);
			SetContentHeader(ref headers, contentType);
			await execute(videoData);
		}


		public static IEnumerator HandleImageSend(byte[] imageData, ImageContentType imageType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			string contentType = GetImageMimeType(imageType);
			SetContentHeader(ref headers, contentType);
			yield return execute(imageData);
		}

		public static async UniTask HandleImageSend(byte[] imageData, ImageContentType imageType, Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			string contentType = GetImageMimeType(imageType);
			SetContentHeader(ref headers, contentType);
			await execute(imageData);
		}
		public static IEnumerator HandleMultipartSend(MultipartFormData multipartData, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute) {
			SetContentHeader(ref headers, $"multipart/form-data; boundary={multipartData.Boundary}");
			byte[] bodyData = multipartData.ToBytes();
			yield return execute(bodyData);
		}

		public static async UniTask HandleMultipartSend(MultipartFormData multipartData, Dictionary<string, string> headers, Func<byte[], UniTask> execute) {
			SetContentHeader(ref headers, $"multipart/form-data; boundary={multipartData.Boundary}");
			byte[] bodyData = multipartData.ToBytes();
			await execute(bodyData);
		}

		//GetBase


	}
}