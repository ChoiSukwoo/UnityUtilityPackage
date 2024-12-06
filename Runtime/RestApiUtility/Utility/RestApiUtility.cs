using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Suk.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using static Suk.RestApi.RestApiState;

namespace Suk.RestApi
{
	internal static class RestApiUtility
	{

		#region MediaTypeConvert
		/// <summary> AudioContentType���� Content-Type�� �����մϴ�. </summary>
		public static string GetContentTypeFromAudioContentType(AudioContentType contentType)
		{
			if (RestApiModel.AudioContentTypeToContentType.TryGetByKey(contentType, out string type))
				return type;

			throw new NotSupportedException($"[RestApiUtility] GetContentTypeFromAudioContentType\nUnsupported AudioContentType: {contentType}");
		}

		/// <summary> Content-Type���� AudioContentType�� �����մϴ�. </summary>
		public static AudioContentType GetAudioContentTypeFromContentType(string contentType)
		{
			if (RestApiModel.AudioContentTypeToContentType.TryGetByValue(contentType, out AudioContentType audioContentType))
				return audioContentType;

			return AudioContentType.Unknown;
		}

		/// <summary> AudioContentType���� Unity AudioType�� �����մϴ�. </summary>
		public static AudioType GetAudioTypeFromAudioContentType(AudioContentType contentType)
		{
			if (RestApiModel.AudioContentTypeToAudioType.TryGetByKey(contentType, out AudioType audioType))
				return audioType;

			return AudioType.UNKNOWN;
		}

		/// <summary> Unity AudioType���� AudioContentType�� �����մϴ�. </summary>
		public static AudioContentType GetAudioContentTypeFromAudioType(AudioType audioType)
		{
			if (RestApiModel.AudioContentTypeToAudioType.TryGetByValue(audioType, out AudioContentType contentType))
				return contentType;

			return AudioContentType.Unknown;
		}

		/// <summary> Content-Type���� Unity AudioType�� �����մϴ�. </summary>
		public static AudioType GetAudioTypeFromContentType(string contentType)
		{
			if (RestApiModel.ContentTypeToAudioType.TryGetByKey(contentType, out AudioType audioType))
				return audioType;

			return AudioType.UNKNOWN;
		}

		/// <summary> Unity AudioType���� Content-Type�� �����մϴ�. </summary>
		public static string GetContentTypeFromAudioType(AudioType audioType)
		{
			if (RestApiModel.ContentTypeToAudioType.TryGetByValue(audioType, out string contentType))
				return contentType;

			throw new NotSupportedException($"[RestApiUtility] GetContentTypeFromAudioType\nUnsupported Unity AudioType: {audioType}");
		}

		/// <summary> VideoContentType���� Content-Type(MIME Type)�� �����մϴ�. </summary>
		public static string GetContentTypeFromVideoContentType(VideoContentType contentType)
		{
			if (RestApiModel.VideoContentTypeToContentType.TryGetByKey(contentType, out string type))
				return type;

			throw new NotSupportedException($"[RestApiUtility] GetContentTypeFromVideoContentType\nUnsupported VideoContentType: {contentType}");
		}

		/// <summary> Content-Type(MIME Type)���� VideoContentType�� �����մϴ�. </summary>
		public static VideoContentType GetVideoContentTypeFromContentType(string contentType)
		{
			if (RestApiModel.VideoContentTypeToContentType.TryGetByValue(contentType, out VideoContentType videoContentType))
				return videoContentType;

			return VideoContentType.Unknown;
		}

		/// <summary> ImageContentType���� Content-Type(MIME Type)�� �����մϴ�. </summary>
		public static string GetContentTypeFromImageContentType(ImageContentType contentType)
		{
			if (RestApiModel.ImageContentTypeToContentType.TryGetByKey(contentType, out string type))
				return type;

			throw new NotSupportedException($"[RestApiUtility] GetContentTypeFromImageContentType\nUnsupported ImageContentType: {contentType}");
		}

		/// <summary> Content-Type(MIME Type)���� ImageContentType�� �����մϴ�. </summary>
		public static ImageContentType GetImageContentTypeFromContentType(string contentType)
		{
			if (RestApiModel.ImageContentTypeToContentType.TryGetByValue(contentType, out ImageContentType imageContentType))
				return imageContentType;

			return ImageContentType.Unknown;
		}
		#endregion

		#region Old
		//���� ��ū ���
		public static void SetAuthHeader(ref Dictionary<string, string> headers, string authToken)
		{
			if (headers == null)
				headers = new Dictionary<string, string>();
			headers["Authorization"] = $"Bearer {authToken}";
		}

		public static void SetContentHeader(ref Dictionary<string, string> headers, string contentType)
		{
			if (headers == null)
				headers = new Dictionary<string, string>();
			headers["Content-Type"] = contentType;
		}

		//1%�̻� �����ϰų� 1�ʰ� ������ 
		public static IEnumerator UpdateProgress(UnityWebRequestAsyncOperation asyncOperation, UnityAction<float> onProgress)
		{
			float lastProgress = 0f;
			float lastProgressUpdate = Time.time;

			while (!asyncOperation.isDone)
			{
				if (Time.time >= lastProgressUpdate + minUpdateInterval || Mathf.Abs(asyncOperation.progress - lastProgress) >= minProgressChange)
				{
					onProgress?.Invoke(asyncOperation.progress);
					lastProgress = asyncOperation.progress;
					lastProgressUpdate = Time.time;
				}
				yield return null;
			}
			// ��û �Ϸ� �� ������� 100%�� ����
			onProgress?.Invoke(1f);
		}

		public static IEnumerator ExecuteWithAuth(string authToken, Dictionary<string, string> headers, Func<IEnumerator> execute)
		{
			SetAuthHeader(ref headers, authToken);
			yield return execute();
		}


		public static void HandleJsonResponse<T>(ApiResponse<string> apiResponse, UnityAction<ApiResponse<T>> onComplete = null)
		{
			if (!apiResponse.Success)
			{
				onComplete?.Invoke(new FailureResponse<T>(apiResponse.ErrorMessage));
				return;
			}

			try
			{
				var parsedData = JsonParser.Parse<T>(apiResponse.Data);
				onComplete?.Invoke(new SuccessResponse<T>(parsedData));
			}
			catch (Exception ex)
			{
				onComplete?.Invoke(new FailureResponse<T>($"JSON �Ľ� ����: {ex.Message}"));
			}
		}


		public static void HandleVideoResponse(ApiResponse<byte[]> apiResponse, string savePath, UnityAction<ApiResponse<string>> onComplete = null)
		{
			if (!apiResponse.Success)
			{
				onComplete?.Invoke(new FailureResponse<string>(apiResponse.ErrorMessage));
				return;
			}

			try
			{
				File.WriteAllBytes(savePath, apiResponse.Data); // ���� ����
				Debug.Log($"Video saved at: {savePath}");
				onComplete?.Invoke(new SuccessResponse<string>(savePath)); // ���� �� ���� ��� ��ȯ
			}
			catch (Exception ex)
			{
				onComplete?.Invoke(new FailureResponse<string>($"Failed to save video: {ex.Message}"));
			}
		}

		public static string HandleVideoResponse(byte[] videoData, string savePath)
		{
			try
			{
				// ���� �����͸� ���Ϸ� ����
				File.WriteAllBytes(savePath, videoData);
				Debug.Log($"Video saved at: {savePath}");
				return savePath;
			}
			catch (Exception ex)
			{
				// ���� �߻� �� �������� ó�� ����
				throw new Exception($"Failed to save video: {ex.Message}");
			}
		}


		public static IEnumerator HandleNoneSend(Dictionary<string, string> headers, Func<byte[], IEnumerator> execute)
		{
			byte[] bodyData = new byte[0]; // �� ������
			yield return execute(bodyData);
		}

		public static IEnumerator HandleTextSend(string text, string contentType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute)
		{
			SetContentHeader(ref headers, contentType);
			byte[] bodyData = System.Text.Encoding.UTF8.GetBytes(text);
			yield return execute(bodyData);
		}

		public static IEnumerator HandleBinarySend(byte[] binaryData, string contentType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute)
		{
			SetContentHeader(ref headers, contentType);
			yield return execute(binaryData);
		}

		public static IEnumerator HandleJsonSend<T>(T body, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute)
		{
			SetContentHeader(ref headers, "application/json");
			byte[] bodyData = JsonParser.ToByteArray(body);
			yield return execute(bodyData);
		}

		public static IEnumerator HandleAudioSend(byte[] audioData, AudioContentType audioType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute)
		{
			string contentType = GetContentTypeFromAudioContentType(audioType); // AudioContentType�� ���� MIME Ÿ�� ����
			SetContentHeader(ref headers, contentType); // ��� ����
			yield return execute(audioData); // �غ�� �����͸� ���� �ܰ�� ����
		}

		public static IEnumerator HandleVideoSend(byte[] videoData, VideoContentType videoType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute)
		{
			string contentType = GetContentTypeFromVideoContentType(videoType);
			SetContentHeader(ref headers, contentType);
			yield return execute(videoData);
		}

		public static IEnumerator HandleImageSend(byte[] imageData, ImageContentType imageType, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute)
		{
			string contentType = GetContentTypeFromImageContentType(imageType);
			SetContentHeader(ref headers, contentType);
			yield return execute(imageData);
		}

		public static IEnumerator HandleMultipartSend(MultipartFormData multipartData, Dictionary<string, string> headers, Func<byte[], IEnumerator> execute)
		{
			SetContentHeader(ref headers, $"multipart/form-data; boundary={multipartData.Boundary}");
			byte[] bodyData = multipartData.ToBytes();
			yield return execute(bodyData);
		}

		public static async UniTask HandleAudioSend(byte[] audioData, AudioContentType audioType, Dictionary<string, string> headers, Func<byte[], UniTask> execute)
		{
			string contentType = GetContentTypeFromAudioContentType(audioType); // AudioContentType�� ���� MIME Ÿ�� ����
			SetContentHeader(ref headers, contentType); // ��� ����
			await execute(audioData);
		}

		public static async UniTask HandleVideoSend(byte[] videoData, VideoContentType videoType, Dictionary<string, string> headers, Func<byte[], UniTask> execute)
		{
			string contentType = GetContentTypeFromVideoContentType(videoType);
			SetContentHeader(ref headers, contentType);
			await execute(videoData);
		}

		public static async UniTask HandleImageSend(byte[] imageData, ImageContentType imageType, Dictionary<string, string> headers, Func<byte[], UniTask> execute)
		{
			string contentType = GetContentTypeFromImageContentType(imageType);
			SetContentHeader(ref headers, contentType);
			await execute(imageData);
		}

		public static async UniTask HandleMultipartSend(MultipartFormData multipartData, Dictionary<string, string> headers, Func<byte[], UniTask> execute)
		{
			SetContentHeader(ref headers, $"multipart/form-data; boundary={multipartData.Boundary}");
			byte[] bodyData = multipartData.ToBytes();
			await execute(bodyData);
		}
		#endregion
	}
}