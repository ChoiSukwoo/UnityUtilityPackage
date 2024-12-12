using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Suk.BinaryUtility;
using Suk.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

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

			switch (expectedType)
			{
				case ContentTypeState.Image:
					if (typeof(T) != typeof(Texture2D))
						throw new InvalidCastException($"ContentTypeState.Image는 Texture2D로 예상되지만, {typeof(T).Name}로 파싱을 시도하였습니다.");
					return (T)(object)DownloadHandlerTexture.GetContent(request);

				case ContentTypeState.Audio:
					if (typeof(T) != typeof(AudioClip))
						throw new InvalidCastException($"ContentTypeState.Audio는 AudioClip으로 예상되지만, {typeof(T).Name}로 파싱을 시도하였습니다.");
					byte[] audioData = request.downloadHandler.data;
					AudioType unityAudioType = GetAudioTypeFromContentType(contentType);
					return (T)(object)await AudioFileUtility.CreateAudioClipAsync(audioData, unityAudioType);

				case ContentTypeState.Asset:
					if (typeof(T) != typeof(AssetBundle))
						throw new InvalidCastException($"ContentTypeState.Asset는 AssetBundle로 예상되지만, {typeof(T).Name}로 파싱을 시도하였습니다.");
					return (T)(object)DownloadHandlerAssetBundle.GetContent(request);

				case ContentTypeState.Text:
					if (typeof(T) != typeof(string))
						throw new InvalidCastException($"ContentTypeState.Text는 string으로 예상되지만, {typeof(T).Name}로 파싱을 시도하였습니다.");
					return (T)(object)request.downloadHandler.text;

				case ContentTypeState.Binary:
				case ContentTypeState.Video:
					if (typeof(T) != typeof(byte[]))
						throw new InvalidCastException($"ContentTypeState.Binary 또는 ContentTypeState.Video는 byte[]로 예상되지만, {typeof(T).Name}로 파싱을 시도하였습니다.");
					return (T)(object)request.downloadHandler.data;

				default:
					throw new Exception($"지원되지 않는 ContentTypeState: {expectedType}");
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
					if (Time.time >= lastProgressUpdate + RestApiState.minUpdateInterval || Mathf.Abs(asyncOperation.progress - lastProgress) >= RestApiState.minProgressChange)
					{
						progress?.Invoke(asyncOperation.progress); // 진행률 업데이트
						lastProgress = asyncOperation.progress;
						lastProgressUpdate = Time.time;
					}

					// 프레임 대기 (취소 토큰 지원)
					await UniTask.Yield(PlayerLoopTiming.Update, token);
				}
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException($"Operation canceled 발생 [UpdateProgress] 작업이 취소 되었습니다.\n{ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"API를 통한 데이터 다운로드 작업 중 오류 발생\n {ex.Message}", ex);
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
			foreach (var (key, value) in body)
			{
				string encodedKey = Uri.EscapeDataString(key);
				string encodedValue = Uri.EscapeDataString(value);
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
			var parsedData = JsonParser.Parse<T>(jsonResponse);
			return parsedData;
		}

		/// <summary> 비디오 데이터를 파일로 저장합니다. </summary>
		public static async UniTask<string> HandleVideoResponse(byte[] videoData, string savePath, CancellationToken cancellationToken = default)
		{
			if (videoData == null || videoData.Length == 0)
				throw new ArgumentException("HandleVideoResponse Error : Video data is null or empty.");

			ValidatePath(savePath);

			// BinaryUtility를 사용해 비동기로 파일 저장
			await BinaryFileUtility.SaveBytesToFileAsync(videoData, savePath, cancellationToken);
			if (RestApiState.enableDebugLog)
				Debug.Log($"[RestApiUtility] HandleVideoResponse\nVideo saved successfully at: {savePath}");
			return savePath;
		}

		/// <summary> JSON를 바이트 배열로 변환하여 반환합니다. </summary>
		public static byte[] HandleJsonBody<T>(T body)
		{
			if (body == null)
				throw new ArgumentNullException(nameof(body), "Body 객체가 null입니다.");

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

			throw new NotSupportedException($"[RestApiUtility] 에서는 AudioContentType : {contentType} 타입을 지원하지 않습니다.");
		}

		/// <summary> Content-Type에서 AudioContentType을 추출합니다. </summary>
		public static AudioContentType GetAudioContentTypeFromContentType(string contentType)
		{
			if (RestApiModel.AudioContentTypeToContentType.TryGetByValue(contentType, out AudioContentType audioContentType))
				return audioContentType;

			throw new NotSupportedException($"[RestApiUtility] 에서는 오디오 string ContentType : {contentType} 타입을 지원하지 않습니다.");
		}

		/// <summary> AudioContentType에서 Unity AudioType을 추출합니다. </summary>
		public static AudioType GetAudioTypeFromAudioContentType(AudioContentType contentType)
		{
			if (RestApiModel.AudioContentTypeToAudioType.TryGetByKey(contentType, out AudioType audioType))
				return audioType;

			throw new NotSupportedException($"[RestApiUtility] 에서는 AudioContentType : {contentType} 타입을 지원하지 않습니다.");
		}

		/// <summary> Unity AudioType에서 AudioContentType을 추출합니다. </summary>
		public static AudioContentType GetAudioContentTypeFromAudioType(AudioType audioType)
		{
			if (RestApiModel.AudioContentTypeToAudioType.TryGetByValue(audioType, out AudioContentType contentType))
				return contentType;

			throw new NotSupportedException($"[RestApiUtility] 에서는 AudioType : {contentType} 타입을 지원하지 않습니다.");
		}

		/// <summary> Content-Type에서 Unity AudioType을 추출합니다. </summary>
		public static AudioType GetAudioTypeFromContentType(string contentType)
		{
			if (RestApiModel.ContentTypeToAudioType.TryGetByKey(contentType, out AudioType audioType))
				return audioType;

			throw new NotSupportedException($"[RestApiUtility] 에서는 오디오 string ContentType : {contentType} 타입을 지원하지 않습니다.");
		}

		/// <summary> Unity AudioType에서 Content-Type을 추출합니다. </summary>
		public static string GetContentTypeFromAudioType(AudioType audioType)
		{
			if (RestApiModel.ContentTypeToAudioType.TryGetByValue(audioType, out string contentType))
				return contentType;

			throw new NotSupportedException($"[RestApiUtility] 에서는 AudioType : {audioType} 타입을 지원하지 않습니다.");
		}

		/// <summary> VideoContentType에서 Content-Type(MIME Type)을 추출합니다. </summary>
		public static string GetContentTypeFromVideoContentType(VideoContentType contentType)
		{
			if (RestApiModel.VideoContentTypeToContentType.TryGetByKey(contentType, out string type))
				return type;

			throw new NotSupportedException($"[RestApiUtility] 에서는 VideoContentType : {contentType} 타입을 지원하지 않습니다.");
		}

		/// <summary> Content-Type(MIME Type)에서 VideoContentType을 추출합니다. </summary>
		public static VideoContentType GetVideoContentTypeFromContentType(string contentType)
		{
			if (RestApiModel.VideoContentTypeToContentType.TryGetByValue(contentType, out VideoContentType videoContentType))
				return videoContentType;

			throw new NotSupportedException($"[RestApiUtility] 에서는 비디오 string ContentType : {contentType} 타입을 지원하지 않습니다.");
		}

		/// <summary> ImageContentType에서 Content-Type(MIME Type)을 추출합니다. </summary>
		public static string GetContentTypeFromImageContentType(ImageContentType contentType)
		{
			if (RestApiModel.ImageContentTypeToContentType.TryGetByKey(contentType, out string type))
				return type;

			throw new NotSupportedException($"[RestApiUtility] 에서는 ImageContentType : {contentType} 타입을 지원하지 않습니다.");
		}

		/// <summary> Content-Type(MIME Type)에서 ImageContentType을 추출합니다. </summary>
		public static ImageContentType GetImageContentTypeFromContentType(string contentType)
		{
			if (RestApiModel.ImageContentTypeToContentType.TryGetByValue(contentType, out ImageContentType imageContentType))
				return imageContentType;

			throw new NotSupportedException($"[RestApiUtility] 에서는 이미지 string ContentType : {contentType} 타입을 지원하지 않습니다.");
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
				throw new ArgumentException(nameof(savePath), $"path url cannot be null or empty. : {savePath}");
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
				throw new OperationCanceledException($"[RestApi] Operation canceled 발생 작업이 취소 되었습니다.\n{ex.Message}", ex);
			}
			catch (Exception ex)
			{
				// 일반 예외를 로그 내용 포함한 새 예외로 상위에 전달
				throw new Exception($"[RestApi] Exception 발생\n{ex.Message}", ex);
			}
		}

		/// <summary> N회 재시도 가능한 고차 함수 </summary>
		public static async UniTask<T> RetryAsync<T>(Func<UniTask<T>> taskFactory, int retryCount = -1, float retryDelay = 1.0f)
		{
			int currentRetry = 0;
			retryCount = retryCount == -1 ? RestApiState.retryCount : retryCount;

			while (true)
			{
				try
				{
					return await taskFactory();
				}
				catch (OperationCanceledException ex)
				{
					throw ex;
				}
				catch (Exception ex)
				{
					currentRetry++;

					if (retryCount >= 0 && currentRetry > retryCount)
						throw new Exception($"RetryAsync failed after {retryCount} attempts.\n{ex.Message}", ex);

					Debug.LogWarning($"Retry {currentRetry}/{retryCount} - Waiting {retryDelay} seconds...");
					await UniTask.Delay(TimeSpan.FromSeconds(retryDelay));
				}
			}
		}
	}
}