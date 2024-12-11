using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Suk.BinaryUtility;
using Suk.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using static Suk.RestApi.RestApiState;

namespace Suk.RestApi
{
	internal static class RestApiUtility
	{
		/// <summary> 인증 토큰을 설정한 새 헤더를 반환합니다 </summary>
		public static Dictionary<string, string> SetAuthHeader(Dictionary<string, string> headers, string authToken)
		{
			var newHeaders = headers != null ? new Dictionary<string, string>(headers) : new Dictionary<string, string>();
			newHeaders["Authorization"] = $"Bearer {authToken}";
			return newHeaders;
		}

		/// <summary> Content-Type 헤더를 설정한 새 헤더를 반환합니다 </summary>
		public static Dictionary<string, string> SetContentHeader(Dictionary<string, string> headers, string contentType)
		{
			var newHeaders = headers != null ? new Dictionary<string, string>(headers) : new Dictionary<string, string>();
			newHeaders["Content-Type"] = contentType;
			return newHeaders;
		}

		/// <summary> UnityWebRequest 객체에 지정된 헤더 값을 설정합니다. </summary>
		public static void SetRequestHeaders(UnityWebRequest request, Dictionary<string, string> headers)
		{
			//header 가 없을시 처리할게 없음
			if (headers == null)
				return;
			//헤더값을 request에 등록
			foreach ((string key, string value) in headers)
				request.SetRequestHeader(key, value);
		}

		/// <summary> URL 및 컨텐츠 유형에 따라 적합한 DownloadHandler를 생성합니다. </summary>
		public static DownloadHandler CreateDownloadHandler(string url, ContentTypeState expectedType)
		{
			switch (expectedType)
			{
				case ContentTypeState.Image:
					return new DownloadHandlerTexture();
				case ContentTypeState.Asset:
					return new DownloadHandlerAssetBundle(url, 0);
				default:
					return new DownloadHandlerBuffer(); // 기본 핸들러
			}
		}

		/// <summary>  UnityWebRequest의 Content-Type 헤더를 기반으로 예상되는 ContentTypeState를 검증합니다. </summary>
		public static bool ValidateContentType(UnityWebRequest request, ref ContentTypeState expectedType)
		{
			// 예상 유형이 Unknown이면 Content-Type을 확인하여 갱신
			if (expectedType == ContentTypeState.Unknown)
			{
				string contentType = request.GetResponseHeader("Content-Type");

				// Content-Type이 비어있거나 유효하지 않으면 false 반환
				if (string.IsNullOrEmpty(contentType) || !TryParseContentType(contentType, out expectedType))
					return false;
			}
			return true;
		}

		public static bool TryParseContentType(string contentType, out ContentTypeState contentTypeId)
		{
			contentTypeId = ContentTypeState.Unknown;

			// Content-Type이 null이거나 비어 있는 경우
			if (string.IsNullOrEmpty(contentType))
				return false;

			// Content-Type의 메인 타입을 파싱
			string mainType = contentType.Split(';')[0].Trim().ToLower();

			if (mainType.StartsWith("text/") || mainType.Contains("json") || mainType.Contains("xml"))
				contentTypeId = ContentTypeState.Text;
			else if (mainType.StartsWith("image/"))
				contentTypeId = ContentTypeState.Image;
			else if (mainType.StartsWith("video/"))
				contentTypeId = ContentTypeState.Video;
			else if (mainType.StartsWith("audio/"))
				contentTypeId = ContentTypeState.Audio;
			else if (mainType.StartsWith("application/octet-stream") || mainType.Contains("assetbundle"))
				contentTypeId = ContentTypeState.Asset;
			else
				contentTypeId = ContentTypeState.Binary;

			return true;
		}

		/// <summary> UnityWebRequest 응답 데이터를 예상된 ContentTypeState에 따라 파싱합니다. </summary>
		public static async UniTask<T> ParseResponse<T>(UnityWebRequest request, ContentTypeState expectedType, string contentType)
		{

			if (request == null)
				throw new ArgumentNullException(nameof(request), "UnityWebRequest cannot be null.");

			try
			{
				switch (expectedType)
				{
					case ContentTypeState.Image:
						if (typeof(T) != typeof(Texture2D))
							throw new InvalidCastException("Image 유형에 대해 Texture2D로 캐스팅이 필요합니다.");
						return (T)(object)DownloadHandlerTexture.GetContent(request);
					case ContentTypeState.Audio:
						if (typeof(T) != typeof(AudioClip))
							throw new InvalidCastException("Audio 유형에 대해 AudioClip으로 캐스팅이 필요합니다.");
						byte[] audioData = request.downloadHandler.data;
						AudioType unityAudioType = GetAudioTypeFromContentType(contentType);
						return (T)(object)await AudioFileUtility.CreateAudioClipAsync(audioData, unityAudioType);
					case ContentTypeState.Asset:
						if (typeof(T) != typeof(AssetBundle))
							throw new InvalidCastException("Asset 유형에 대해 AssetBundle로 캐스팅이 필요합니다.");
						return (T)(object)DownloadHandlerAssetBundle.GetContent(request);
					case ContentTypeState.Text:
						if (typeof(T) != typeof(string))
							throw new InvalidCastException("Text 유형에 대해 string으로 캐스팅이 필요합니다.");
						return (T)(object)request.downloadHandler.text;
					case ContentTypeState.Binary:
					case ContentTypeState.Video:
						if (typeof(T) != typeof(byte[]))
							throw new InvalidCastException("Binary 또는 Video 유형에 대해 byte[]로 캐스팅이 필요합니다.");
						return (T)(object)request.downloadHandler.data;
					default:
						throw new Exception($"지원되지 않는 ContentTypeState: {expectedType}");
				}
			}
			catch (InvalidCastException ex)
			{
				throw new InvalidCastException($"ParseResponse\n{ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"ParseResponse\n알 수 없는 에러 발생: {ex.Message}", ex);
			}
		}

		/// <summary>UnityWebRequest 진행 상태를 비동기로 업데이트합니다. </summary>
		public static async UniTask UpdateProgress(UnityWebRequestAsyncOperation asyncOperation, UnityAction<float> progress = null, CancellationToken token = default)
		{
			float lastProgress = 0f;
			float lastProgressUpdate = Time.time;

			try
			{
				while (!asyncOperation.isDone)
				{
					if (Time.time >= lastProgressUpdate + minUpdateInterval || Mathf.Abs(asyncOperation.progress - lastProgress) >= minProgressChange)
					{
						progress?.Invoke(asyncOperation.progress); // 진행률 업데이트
						lastProgress = asyncOperation.progress;
						lastProgressUpdate = Time.time;
					}

					// 프레임 대기 (취소 토큰 지원)
					await UniTask.Yield(PlayerLoopTiming.Update, token);
				}
			}
			catch (OperationCanceledException)
			{
				throw new OperationCanceledException("UpdateProgress Error : API를 통한 데이터 다운로드 작업이 취소되었습니다.", token);
			}
			catch (Exception ex)
			{
				throw new Exception($"UpdateProgress Error : API를 통한 데이터 다운로드 작업 중 오류 발생\n {ex.Message}", ex);
			}

			progress?.Invoke(1f);
		}


		#region DataConvert
		public static byte[] ConvertTextToBytes(string text)
		{
			return Encoding.UTF8.GetBytes(text);
		}

		public static byte[] ConvertDictionaryToBytes(Dictionary<string, string> body)
		{
			// 단계 2: FormData 변환
			if (body == null || body.Count == 0)
				throw new ArgumentException("Form data cannot be null or empty.");

			// Key-Value 쌍을 인코딩하여 List<string>에 저장
			List<string> encodedDataParts = new List<string>();
			foreach (KeyValuePair<string, string> kvp in body)
			{
				string encodedKey = Uri.EscapeDataString(Convert.ToString(kvp.Key ?? string.Empty));
				string encodedValue = Uri.EscapeDataString(Convert.ToString(kvp.Value ?? string.Empty));
				encodedDataParts.Add($"{encodedKey}={encodedValue}");
			}

			// 조합된 문자열을 생성
			string encodedDataString = string.Join("&", encodedDataParts);

			// UTF8 바이트 배열로 변환
			return Encoding.UTF8.GetBytes(encodedDataString);
		}
		#endregion

		#region Handler
		/// <summary> JSON 응답 데이터를 지정된 제네릭 타입으로 파싱합니다. </summary>
		public static T HandleJsonResponse<T>(string jsonResponse)
		{
			try
			{
				var parsedData = JsonParser.Parse<T>(jsonResponse);
				return parsedData;
			}
			catch (Exception ex)
			{
				throw new Exception($"HandleJsonResponse\n {ex.Message}", ex);
			}
		}

		/// <summary> 비디오 데이터를 파일로 저장합니다. </summary>
		public static async UniTask<string> HandleVideoResponse(byte[] videoData, string savePath, CancellationToken cancellationToken = default)
		{
			if (videoData == null || videoData.Length == 0)
				throw new ArgumentException("HandleVideoResponse Error : Video data is null or empty.");

			ValidatePath(savePath);

			try
			{
				// BinaryUtility를 사용해 비동기로 파일 저장
				await BinaryFileUtility.SaveBytesToFileAsync(videoData, savePath, cancellationToken);

				Debug.Log($"[RestApiUtility] HandleVideoResponse\nVideo saved successfully at: {savePath}");
				return savePath;
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException($"[RestApiUtility] HandleVideoResponse\nVideo save operation was canceled.", ex);
			}
			catch (Exception ex)
			{
				throw new IOException($"[RestApiUtility] HandleVideoResponse\nFailed to save video: {ex.Message}", ex);
			}
		}

		/// <summary> JSON를 바이트 배열로 변환하여 반환합니다. </summary>
		public static byte[] HandleJsonBody<T>(T body)
		{
			if (body == null)
				throw new ArgumentNullException(nameof(body), "[RestApiUtility] HandleJsonBody\nBody 객체가 null입니다.");

			byte[] bodyData = JsonParser.ToByteArray(body);
			return bodyData;
		}
		#endregion

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

		#region Validate

		/// <summary>URL 유효성 검사</summary>
		public static void ValidateUrl(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
				throw new ArgumentNullException(nameof(url), "URL이 null이거나 비어 있습니다.");
			if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
				throw new ArgumentException("유효하지 않은 URL 형식입니다.", nameof(url));
		}

		/// <summary>Content-Type 유효성 검사</summary>
		public static void ValidateContentType(string contentType)
		{
			if (string.IsNullOrWhiteSpace(contentType))
				throw new ArgumentNullException(nameof(contentType), "Content-Type이 null이거나 비어 있습니다.");
		}

		/// <summary>Body 유효성 검사</summary>
		public static void ValidateBody(byte[] body)
		{
			if (body == null)
				throw new ArgumentNullException(nameof(body), "요청 바디가 null입니다.");
		}

		/// <summary>Path 유효성 검사</summary>
		public static void ValidatePath(string savePath)
		{
			if (string.IsNullOrWhiteSpace(savePath))
				throw new ArgumentException("path url cannot be null or empty.", nameof(savePath));
		}
		#endregion

		/// <summary>공통 예외 로깅 함수</summary>
		public static async UniTask<T> ErrorLogging<T>(Func<UniTask<T>> taskFunc)
		{
			try
			{
				return await taskFunc();
			}
			catch (OperationCanceledException ex)
			{
				// 취소 예외를 로그 내용 포함한 새 예외로 상위에 전달
				throw new OperationCanceledException($"[RestApiUtility] Operation canceled 발생\n{ex.Message}", ex);
			}
			catch (Exception ex)
			{
				// 일반 예외를 로그 내용 포함한 새 예외로 상위에 전달
				throw new Exception($"[RestApiUtility] Exception 발생\n{ex.Message}", ex);
			}
		}
	}
}