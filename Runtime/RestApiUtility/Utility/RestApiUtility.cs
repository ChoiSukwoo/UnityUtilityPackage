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
		/// <summary> AudioContentType에서 Content-Type을 추출합니다. </summary>
		public static string GetContentTypeFromAudioContentType(AudioContentType contentType)
		{
			if (RestApiModel.AudioContentTypeToContentType.TryGetByKey(contentType, out string type))
				return type;

			throw new NotSupportedException($"[RestApiUtility] GetContentTypeFromAudioContentType\nUnsupported AudioContentType: {contentType}");
		}

		/// <summary> Content-Type에서 AudioContentType을 추출합니다. </summary>
		public static AudioContentType GetAudioContentTypeFromContentType(string contentType)
		{
			if (RestApiModel.AudioContentTypeToContentType.TryGetByValue(contentType, out AudioContentType audioContentType))
				return audioContentType;

			return AudioContentType.Unknown;
		}

		/// <summary> AudioContentType에서 Unity AudioType을 추출합니다. </summary>
		public static AudioType GetAudioTypeFromAudioContentType(AudioContentType contentType)
		{
			if (RestApiModel.AudioContentTypeToAudioType.TryGetByKey(contentType, out AudioType audioType))
				return audioType;

			return AudioType.UNKNOWN;
		}

		/// <summary> Unity AudioType에서 AudioContentType을 추출합니다. </summary>
		public static AudioContentType GetAudioContentTypeFromAudioType(AudioType audioType)
		{
			if (RestApiModel.AudioContentTypeToAudioType.TryGetByValue(audioType, out AudioContentType contentType))
				return contentType;

			return AudioContentType.Unknown;
		}

		/// <summary> Content-Type에서 Unity AudioType을 추출합니다. </summary>
		public static AudioType GetAudioTypeFromContentType(string contentType)
		{
			if (RestApiModel.ContentTypeToAudioType.TryGetByKey(contentType, out AudioType audioType))
				return audioType;

			return AudioType.UNKNOWN;
		}

		/// <summary> Unity AudioType에서 Content-Type을 추출합니다. </summary>
		public static string GetContentTypeFromAudioType(AudioType audioType)
		{
			if (RestApiModel.ContentTypeToAudioType.TryGetByValue(audioType, out string contentType))
				return contentType;

			throw new NotSupportedException($"[RestApiUtility] GetContentTypeFromAudioType\nUnsupported Unity AudioType: {audioType}");
		}

		/// <summary> VideoContentType에서 Content-Type(MIME Type)을 추출합니다. </summary>
		public static string GetContentTypeFromVideoContentType(VideoContentType contentType)
		{
			if (RestApiModel.VideoContentTypeToContentType.TryGetByKey(contentType, out string type))
				return type;

			throw new NotSupportedException($"[RestApiUtility] GetContentTypeFromVideoContentType\nUnsupported VideoContentType: {contentType}");
		}

		/// <summary> Content-Type(MIME Type)에서 VideoContentType을 추출합니다. </summary>
		public static VideoContentType GetVideoContentTypeFromContentType(string contentType)
		{
			if (RestApiModel.VideoContentTypeToContentType.TryGetByValue(contentType, out VideoContentType videoContentType))
				return videoContentType;

			return VideoContentType.Unknown;
		}

		/// <summary> ImageContentType에서 Content-Type(MIME Type)을 추출합니다. </summary>
		public static string GetContentTypeFromImageContentType(ImageContentType contentType)
		{
			if (RestApiModel.ImageContentTypeToContentType.TryGetByKey(contentType, out string type))
				return type;

			throw new NotSupportedException($"[RestApiUtility] GetContentTypeFromImageContentType\nUnsupported ImageContentType: {contentType}");
		}

		/// <summary> Content-Type(MIME Type)에서 ImageContentType을 추출합니다. </summary>
		public static ImageContentType GetImageContentTypeFromContentType(string contentType)
		{
			if (RestApiModel.ImageContentTypeToContentType.TryGetByValue(contentType, out ImageContentType imageContentType))
				return imageContentType;

			return ImageContentType.Unknown;
		}
		#endregion

		#region Old
		//인증 토큰 헤더
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

		//1%이상 증가하거나 1초가 지날시 
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
			// 요청 완료 시 진행률을 100%로 설정
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
				onComplete?.Invoke(new FailureResponse<T>($"JSON 파싱 오류: {ex.Message}"));
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
				File.WriteAllBytes(savePath, apiResponse.Data); // 파일 저장
				Debug.Log($"Video saved at: {savePath}");
				onComplete?.Invoke(new SuccessResponse<string>(savePath)); // 성공 시 저장 경로 반환
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
				// 비디오 데이터를 파일로 저장
				File.WriteAllBytes(savePath, videoData);
				Debug.Log($"Video saved at: {savePath}");
				return savePath;
			}
			catch (Exception ex)
			{
				// 예외 발생 시 상위에서 처리 가능
				throw new Exception($"Failed to save video: {ex.Message}");
			}
		}


		public static IEnumerator HandleNoneSend(Dictionary<string, string> headers, Func<byte[], IEnumerator> execute)
		{
			byte[] bodyData = new byte[0]; // 빈 데이터
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
			string contentType = GetContentTypeFromAudioContentType(audioType); // AudioContentType에 따라 MIME 타입 결정
			SetContentHeader(ref headers, contentType); // 헤더 설정
			yield return execute(audioData); // 준비된 데이터를 다음 단계로 전달
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
			string contentType = GetContentTypeFromAudioContentType(audioType); // AudioContentType에 따라 MIME 타입 결정
			SetContentHeader(ref headers, contentType); // 헤더 설정
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