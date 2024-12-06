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
	internal static class TaskRestApiUtility
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
				request.SetRequestHeader(key, value);
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

			try
			{
				switch (expectedType)
				{
					case ContentTypeState.Image:
						if (typeof(T) != typeof(Texture2D))
							throw new InvalidCastException("Image ������ ���� Texture2D�� ĳ������ �ʿ��մϴ�.");
						return (T)(object)DownloadHandlerTexture.GetContent(request);
					case ContentTypeState.Audio:
						if (typeof(T) != typeof(AudioClip))
							throw new InvalidCastException("Audio ������ ���� AudioClip���� ĳ������ �ʿ��մϴ�.");
						byte[] audioData = request.downloadHandler.data;
						AudioType unityAudioType = GetAudioTypeFromContentType(contentType);
						return (T)(object)await AudioFileUtility.CreateAudioClipAsync(audioData, unityAudioType);
					case ContentTypeState.Asset:
						if (typeof(T) != typeof(AssetBundle))
							throw new InvalidCastException("Asset ������ ���� AssetBundle�� ĳ������ �ʿ��մϴ�.");
						return (T)(object)DownloadHandlerAssetBundle.GetContent(request);
					case ContentTypeState.Text:
						if (typeof(T) != typeof(string))
							throw new InvalidCastException("Text ������ ���� string���� ĳ������ �ʿ��մϴ�.");
						return (T)(object)request.downloadHandler.text;
					case ContentTypeState.Binary:
					case ContentTypeState.Video:
						if (typeof(T) != typeof(byte[]))
							throw new InvalidCastException("Binary �Ǵ� Video ������ ���� byte[]�� ĳ������ �ʿ��մϴ�.");
						return (T)(object)request.downloadHandler.data;
					default:
						throw new Exception($"�������� �ʴ� ContentTypeState: {expectedType}");
				}
			}
			catch (InvalidCastException ex)
			{
				throw new InvalidCastException($"[RestApiUtility] ParseResponse\n{ex.Message}", ex);
			}
			catch (Exception ex)
			{
				throw new Exception($"[RestApiUtility] ParseResponse\n�� �� ���� ���� �߻�: {ex.Message}", ex);
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
					if (Time.time >= lastProgressUpdate + minUpdateInterval || Mathf.Abs(asyncOperation.progress - lastProgress) >= minProgressChange)
					{
						progress?.Invoke(asyncOperation.progress); // ����� ������Ʈ
						lastProgress = asyncOperation.progress;
						lastProgressUpdate = Time.time;
					}

					// ������ ��� (��� ��ū ����)
					await UniTask.Yield(PlayerLoopTiming.Update, token);
				}
			}
			catch (OperationCanceledException)
			{
				throw new OperationCanceledException("[RestApiUtility] UpdateProgress\nAPI�� ���� ������ �ٿ�ε� �۾��� ��ҵǾ����ϴ�.", token);
			}
			catch (Exception ex)
			{
				throw new Exception($"[RestApiUtility] UpdateProgress\nAPI�� ���� ������ �ٿ�ε� �۾� �� ���� �߻�: {ex.Message}", ex);
			}

			progress?.Invoke(1f);
		}

		/// <summary> JSON ���� �����͸� ������ ���׸� Ÿ������ �Ľ��մϴ�. </summary>
		public static T HandleJsonResponse<T>(string jsonResponse)
		{
			try
			{
				var parsedData = JsonParser.Parse<T>(jsonResponse);
				return parsedData;
			}
			catch (Exception ex)
			{
				throw new Exception($"[RestApiUtility] HandleJsonResponse\n {ex.Message}", ex);
			}
		}

		/// <summary> ���� �����͸� ���Ϸ� �����մϴ�. </summary>
		public static async UniTask<string> HandleVideoResponse(byte[] videoData, string savePath, CancellationToken cancellationToken = default)
		{
			if (videoData == null || videoData.Length == 0)
				throw new ArgumentException("[RestApiUtility] HandleVideoResponse\nVideo data is null or empty.");

			if (string.IsNullOrWhiteSpace(savePath))
				throw new ArgumentException("[RestApiUtility] HandleVideoResponse\nSave path cannot be null or empty.");

			try
			{
				// BinaryUtility�� ����� �񵿱�� ���� ����
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

		/// <summary> ������ ������ �ʿ� ���� ��û�� ó���մϴ�. </summary>
		public static byte[] HandleNoneBody()
		{
			byte[] bodyData = new byte[0]; // �� ������
			return bodyData;
		}

		/// <summary> Text�� ����Ʈ �迭�� ��ȯ�Ͽ� ��ȯ�մϴ�. </summary>
		public static byte[] HandleTextBody(string text, string contentType, Dictionary<string, string> headers)
		{
			byte[] bodyData = Encoding.UTF8.GetBytes(text);
			return bodyData;
		}

		/// <summary> JSON�� ����Ʈ �迭�� ��ȯ�Ͽ� ��ȯ�մϴ�. </summary>
		public static byte[] HandleJsonBody<T>(T body, Dictionary<string, string> headers)
		{
			if (body == null)
				throw new ArgumentNullException(nameof(body), "[RestApiUtility] HandleJsonBody\nBody ��ü�� null�Դϴ�.");

			byte[] bodyData = JsonParser.ToByteArray(body);
			return bodyData;
		}

		/// <summary> MultipartFormData�� ����Ʈ �迭�� ��ȯ�Ͽ� ��ȯ�մϴ�. </summary>
		public static byte[] HandleMultipartBody(MultipartFormData multipartData)
		{
			byte[] bodyData = multipartData.ToBytes();
			return bodyData;
		}

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
	}
}