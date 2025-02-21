using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Suk.BinaryUtility;
using Suk.Utility.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Suk.RestApi
{
	internal static class RestApiUtility
	{
		/// <summary> ���� ��ū�� ������ �� ����� ��ȯ�մϴ� </summary>
		public static Dictionary<string, string> SetAuthHeader(Dictionary<string, string> headers, string authToken)
		{
			var newHeaders = headers != null ? new Dictionary<string, string>(headers) : new Dictionary<string, string>();
			newHeaders["Authorization"] = $"Bearer {authToken}";
			return newHeaders;
		}

		/// <summary> Content-Type ����� ������ �� ����� ��ȯ�մϴ� </summary>
		public static Dictionary<string, string> SetContentHeader(Dictionary<string, string> headers, string contentType)
		{
			var newHeaders = headers != null ? new Dictionary<string, string>(headers) : new Dictionary<string, string>();
			newHeaders["Content-Type"] = contentType;
			return newHeaders;
		}

		/// <summary> UnityWebRequest ��ü�� ������ ��� ���� �����մϴ�. </summary>
		public static void SetRequestHeaders(UnityWebRequest request, Dictionary<string, string> headers)
		{
			//header �� ������ ó���Ұ� ����
			if (headers == null)
				return;
			//������� request�� ���
			foreach ((string key, string value) in headers)
			{
				request.SetRequestHeader(key, value);
			}
		}

		/// <summary> URL �� ������ ������ ���� ������ DownloadHandler�� �����մϴ�. </summary>
		public static DownloadHandler CreateDownloadHandler(string url, ContentTypeState expectedType)
		{
			switch (expectedType)
			{
				case ContentTypeState.Image:
					return new DownloadHandlerTexture();
				case ContentTypeState.Asset:
					return new DownloadHandlerAssetBundle(url, 0);
				default:
					return new DownloadHandlerBuffer(); // �⺻ �ڵ鷯
			}
		}

		/// <summary>  UnityWebRequest�� Content-Type ����� ������� ����Ǵ� ContentTypeState�� �����մϴ�. </summary>
		public static bool ValidateContentType(UnityWebRequest request, ref ContentTypeState expectedType)
		{
			// ���� ������ Unknown�̸� Content-Type�� Ȯ���Ͽ� ����
			if (expectedType == ContentTypeState.Unknown)
			{
				string contentType = request.GetResponseHeader("Content-Type");

				// Content-Type�� ����ְų� ��ȿ���� ������ false ��ȯ
				if (string.IsNullOrEmpty(contentType) || !TryParseContentType(contentType, out expectedType))
					return false;
			}
			return true;
		}

		public static bool TryParseContentType(string contentType, out ContentTypeState contentTypeId)
		{
			contentTypeId = ContentTypeState.Unknown;

			// Content-Type�� null�̰ų� ��� �ִ� ���
			if (string.IsNullOrEmpty(contentType))
				return false;

			// Content-Type�� ���� Ÿ���� �Ľ�
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

		/// <summary> UnityWebRequest ���� �����͸� ����� ContentTypeState�� ���� �Ľ��մϴ�. </summary>
		public static async UniTask<T> ParseResponse<T>(UnityWebRequest request, ContentTypeState expectedType, string contentType)
		{

			if (request == null)
				throw new ArgumentNullException(nameof(request), "UnityWebRequest cannot be null.");

			switch (expectedType)
			{
				case ContentTypeState.Image:
					if (typeof(T) != typeof(Texture2D))
						throw new InvalidCastException($"ContentTypeState.Image�� Texture2D�� ���������, {typeof(T).Name}�� �Ľ��� �õ��Ͽ����ϴ�.");
					return (T)(object)DownloadHandlerTexture.GetContent(request);

				case ContentTypeState.Audio:
					if (typeof(T) != typeof(AudioClip))
						throw new InvalidCastException($"ContentTypeState.Audio�� AudioClip���� ���������, {typeof(T).Name}�� �Ľ��� �õ��Ͽ����ϴ�.");
					byte[] audioData = request.downloadHandler.data;
					AudioType unityAudioType = GetAudioTypeFromContentType(contentType);
					return (T)(object)await AudioFileUtility.CreateAudioClipAsync(audioData, unityAudioType);

				case ContentTypeState.Asset:
					if (typeof(T) != typeof(AssetBundle))
						throw new InvalidCastException($"ContentTypeState.Asset�� AssetBundle�� ���������, {typeof(T).Name}�� �Ľ��� �õ��Ͽ����ϴ�.");
					return (T)(object)DownloadHandlerAssetBundle.GetContent(request);

				case ContentTypeState.Text:
					if (typeof(T) != typeof(string))
						throw new InvalidCastException($"ContentTypeState.Text�� string���� ���������, {typeof(T).Name}�� �Ľ��� �õ��Ͽ����ϴ�.");
					return (T)(object)request.downloadHandler.text;

				case ContentTypeState.Binary:
				case ContentTypeState.Video:
					if (typeof(T) != typeof(byte[]))
						throw new InvalidCastException($"ContentTypeState.Binary �Ǵ� ContentTypeState.Video�� byte[]�� ���������, {typeof(T).Name}�� �Ľ��� �õ��Ͽ����ϴ�.");
					return (T)(object)request.downloadHandler.data;

				default:
					throw new Exception($"�������� �ʴ� ContentTypeState: {expectedType}");
			}
		}

		/// <summary>UnityWebRequest ���� ���¸� �񵿱�� ������Ʈ�մϴ�. </summary>
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
						progress?.Invoke(asyncOperation.progress); // ����� ������Ʈ
						lastProgress = asyncOperation.progress;
						lastProgressUpdate = Time.time;
					}

					// ������ ��� (��� ��ū ����)
					await UniTask.Yield(PlayerLoopTiming.Update, token);
				}
			}
			catch (OperationCanceledException ex)
			{
				throw new OperationCanceledException($"Operation canceled �߻� [UpdateProgress] �۾��� ��� �Ǿ����ϴ�.\n{ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"API�� ���� ������ �ٿ�ε� �۾� �� ���� �߻�\n {ex.Message}", ex);
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
			// �ܰ� 2: FormData ��ȯ
			if (body == null || body.Count == 0)
				throw new ArgumentException("Form data cannot be null or empty.");

			// Key-Value ���� ���ڵ��Ͽ� List<string>�� ����
			List<string> encodedDataParts = new List<string>();
			foreach (var (key, value) in body)
			{
				string encodedKey = Uri.EscapeDataString(key);
				string encodedValue = Uri.EscapeDataString(value);
				encodedDataParts.Add($"{encodedKey}={encodedValue}");
			}

			// ���յ� ���ڿ��� ����
			string encodedDataString = string.Join("&", encodedDataParts);

			// UTF8 ����Ʈ �迭�� ��ȯ
			return Encoding.UTF8.GetBytes(encodedDataString);
		}
		#endregion

		#region Handler
		/// <summary> JSON ���� �����͸� ������ ���׸� Ÿ������ �Ľ��մϴ�. </summary>
		public static T HandleJsonResponse<T>(string jsonResponse)
		{
			var parsedData = JsonParser.Parsing<T>(jsonResponse);
			return parsedData;
		}

		/// <summary> ���� �����͸� ���Ϸ� �����մϴ�. </summary>
		public static async UniTask<string> HandleVideoResponse(byte[] videoData, string savePath, CancellationToken cancellationToken = default)
		{
			if (videoData == null || videoData.Length == 0)
				throw new ArgumentException("HandleVideoResponse Error : Video data is null or empty.");

			ValidatePath(savePath);

			// BinaryUtility�� ����� �񵿱�� ���� ����
			await BinaryFileUtility.SaveBytesToFileAsync(videoData, savePath, cancellationToken);
			if (RestApiState.enableDebugLog)
				Debug.Log($"[RestApiUtility] HandleVideoResponse\nVideo saved successfully at: {savePath}");
			return savePath;
		}

		/// <summary> JSON�� ����Ʈ �迭�� ��ȯ�Ͽ� ��ȯ�մϴ�. </summary>
		public static byte[] HandleJsonBody<T>(T body)
		{
			if (body == null)
				throw new ArgumentNullException(nameof(body), "Body ��ü�� null�Դϴ�.");

			byte[] bodyData = JsonParser.SerializeToByteArray(body);
			return bodyData;
		}
		#endregion

		#region MediaTypeConvert
		/// <summary> AudioContentType���� Content-Type�� �����մϴ�. </summary>
		public static string GetContentTypeFromAudioContentType(AudioContentType contentType)
		{
			if (RestApiModel.AudioContentTypeToContentType.TryGetByKey(contentType, out string type))
				return type;

			throw new NotSupportedException($"[RestApiUtility] ������ AudioContentType : {contentType} Ÿ���� �������� �ʽ��ϴ�.");
		}

		/// <summary> Content-Type���� AudioContentType�� �����մϴ�. </summary>
		public static AudioContentType GetAudioContentTypeFromContentType(string contentType)
		{
			if (RestApiModel.AudioContentTypeToContentType.TryGetByValue(contentType, out AudioContentType audioContentType))
				return audioContentType;

			throw new NotSupportedException($"[RestApiUtility] ������ ����� string ContentType : {contentType} Ÿ���� �������� �ʽ��ϴ�.");
		}

		/// <summary> AudioContentType���� Unity AudioType�� �����մϴ�. </summary>
		public static AudioType GetAudioTypeFromAudioContentType(AudioContentType contentType)
		{
			if (RestApiModel.AudioContentTypeToAudioType.TryGetByKey(contentType, out AudioType audioType))
				return audioType;

			throw new NotSupportedException($"[RestApiUtility] ������ AudioContentType : {contentType} Ÿ���� �������� �ʽ��ϴ�.");
		}

		/// <summary> Unity AudioType���� AudioContentType�� �����մϴ�. </summary>
		public static AudioContentType GetAudioContentTypeFromAudioType(AudioType audioType)
		{
			if (RestApiModel.AudioContentTypeToAudioType.TryGetByValue(audioType, out AudioContentType contentType))
				return contentType;

			throw new NotSupportedException($"[RestApiUtility] ������ AudioType : {contentType} Ÿ���� �������� �ʽ��ϴ�.");
		}

		/// <summary> Content-Type���� Unity AudioType�� �����մϴ�. </summary>
		public static AudioType GetAudioTypeFromContentType(string contentType)
		{
			if (RestApiModel.ContentTypeToAudioType.TryGetByKey(contentType, out AudioType audioType))
				return audioType;

			throw new NotSupportedException($"[RestApiUtility] ������ ����� string ContentType : {contentType} Ÿ���� �������� �ʽ��ϴ�.");
		}

		/// <summary> Unity AudioType���� Content-Type�� �����մϴ�. </summary>
		public static string GetContentTypeFromAudioType(AudioType audioType)
		{
			if (RestApiModel.ContentTypeToAudioType.TryGetByValue(audioType, out string contentType))
				return contentType;

			throw new NotSupportedException($"[RestApiUtility] ������ AudioType : {audioType} Ÿ���� �������� �ʽ��ϴ�.");
		}

		/// <summary> VideoContentType���� Content-Type(MIME Type)�� �����մϴ�. </summary>
		public static string GetContentTypeFromVideoContentType(VideoContentType contentType)
		{
			if (RestApiModel.VideoContentTypeToContentType.TryGetByKey(contentType, out string type))
				return type;

			throw new NotSupportedException($"[RestApiUtility] ������ VideoContentType : {contentType} Ÿ���� �������� �ʽ��ϴ�.");
		}

		/// <summary> Content-Type(MIME Type)���� VideoContentType�� �����մϴ�. </summary>
		public static VideoContentType GetVideoContentTypeFromContentType(string contentType)
		{
			if (RestApiModel.VideoContentTypeToContentType.TryGetByValue(contentType, out VideoContentType videoContentType))
				return videoContentType;

			throw new NotSupportedException($"[RestApiUtility] ������ ���� string ContentType : {contentType} Ÿ���� �������� �ʽ��ϴ�.");
		}

		/// <summary> ImageContentType���� Content-Type(MIME Type)�� �����մϴ�. </summary>
		public static string GetContentTypeFromImageContentType(ImageContentType contentType)
		{
			if (RestApiModel.ImageContentTypeToContentType.TryGetByKey(contentType, out string type))
				return type;

			throw new NotSupportedException($"[RestApiUtility] ������ ImageContentType : {contentType} Ÿ���� �������� �ʽ��ϴ�.");
		}

		/// <summary> Content-Type(MIME Type)���� ImageContentType�� �����մϴ�. </summary>
		public static ImageContentType GetImageContentTypeFromContentType(string contentType)
		{
			if (RestApiModel.ImageContentTypeToContentType.TryGetByValue(contentType, out ImageContentType imageContentType))
				return imageContentType;

			throw new NotSupportedException($"[RestApiUtility] ������ �̹��� string ContentType : {contentType} Ÿ���� �������� �ʽ��ϴ�.");
		}
		#endregion

		#region Validate
		/// <summary>URL ��ȿ�� �˻�</summary>
		public static void ValidateUrl(string url)
		{
			if (string.IsNullOrWhiteSpace(url))
				throw new ArgumentNullException(nameof(url), "URL�� null�̰ų� ��� �ֽ��ϴ�.");
			if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
				throw new ArgumentException("��ȿ���� ���� URL �����Դϴ�.", nameof(url));
		}

		/// <summary>Content-Type ��ȿ�� �˻�</summary>
		public static void ValidateContentType(string contentType)
		{
			if (string.IsNullOrWhiteSpace(contentType))
				throw new ArgumentNullException(nameof(contentType), "Content-Type�� null�̰ų� ��� �ֽ��ϴ�.");
		}

		/// <summary>Body ��ȿ�� �˻�</summary>
		public static void ValidateBody(byte[] body)
		{
			if (body == null)
				throw new ArgumentNullException(nameof(body), "��û �ٵ� null�Դϴ�.");
		}

		/// <summary>Path ��ȿ�� �˻�</summary>
		public static void ValidatePath(string savePath)
		{
			if (string.IsNullOrWhiteSpace(savePath))
				throw new ArgumentException(nameof(savePath), $"path url cannot be null or empty. : {savePath}");

			// 'file://' URL ó��
			if (Uri.TryCreate(savePath, UriKind.Absolute, out Uri uri) && uri.IsFile)
				savePath = uri.LocalPath; // 'file:///' URL�� ���� ���� ��η� ��ȯ

			// ��ΰ� ���� ������� Ȯ��
			if (!Path.IsPathRooted(savePath))
				throw new ArgumentException($"The path is not a valid absolute path. Given path: {savePath}");

		}
		#endregion

		/// <summary>���� ���� �α� �Լ�</summary>
		public static async UniTask<T> ErrorLogging<T>(Func<UniTask<T>> taskFunc)
		{
			try
			{
				return await taskFunc();
			}
			catch (OperationCanceledException ex)
			{
				// ��� ���ܸ� �α� ���� ������ �� ���ܷ� ������ ����
				throw new OperationCanceledException($"[RestApi] Operation canceled �߻� �۾��� ��� �Ǿ����ϴ�.\n{ex.Message}", ex);
			}
			catch (Exception ex)
			{
				// �Ϲ� ���ܸ� �α� ���� ������ �� ���ܷ� ������ ����
				throw new Exception($"[RestApi] Exception �߻�\n{ex.Message}", ex);
			}
		}

		/// <summary> Nȸ ��õ� ������ ���� �Լ� </summary>
		public static async UniTask<T> RetryAsync<T>(Func<UniTask<T>> taskFactory, int retryCount = -1, float retryDelay = 1.0f)
		{
			int currentRetry = 1;
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

		/// <summary> ���� ��ο��� �̹��� Ÿ���� ��ȯ�մϴ�. </summary>
		public static ImageContentType GetImageContentType(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
				return ImageContentType.Unknown;

			string extension = Path.GetExtension(filePath)?.ToLowerInvariant();
			return extension switch
			{
				".png" => ImageContentType.Png,
				".jpg" => ImageContentType.Jpeg,
				".jpeg" => ImageContentType.Jpeg,
				_ => ImageContentType.Unknown
			};
		}

		/// <summary> ���� ��ο��� ����� Ÿ���� ��ȯ�մϴ�. </summary>
		public static AudioContentType GetAudioContentType(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
				return AudioContentType.Unknown;

			string extension = Path.GetExtension(filePath)?.ToLowerInvariant();
			return extension switch
			{
				".mp3" => AudioContentType.MP3,
				".wav" => AudioContentType.Wav,
				".ogg" => AudioContentType.Ogg,
				_ => AudioContentType.Unknown
			};
		}

		/// <summary> ���� ��ο��� ���� Ÿ���� ��ȯ�մϴ�. </summary>
		public static VideoContentType GetVideoContentType(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
				return VideoContentType.Unknown;

			string extension = Path.GetExtension(filePath)?.ToLowerInvariant();
			return extension switch
			{
				".mp4" => VideoContentType.Mp4,
				".webm" => VideoContentType.Webm,
				_ => VideoContentType.Unknown
			};
		}
	}
}