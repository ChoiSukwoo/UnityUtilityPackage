using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Suk.RestApi;
using UnityEngine;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;

namespace Suk
{
	internal static class PostImage
	{
		/// <summary>Image �����͸� POST ��û���� �����ϰ� �ؽ�Ʈ ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostImageForText(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostImageAsync<string>(url, body, ContentTypeState.Text, imageType, onProgress, headers, cancellationToken));
		}

		/// <summary>Image �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static async UniTask<Res> PostImageForJson<Res>(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostImageAsync<string>(url, body, ContentTypeState.Text, imageType, onProgress, headers, cancellationToken);
				return HandleJsonResponse<Res>(jsonResponse);
			});
		}

		/// <summary>Image �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static async UniTask<byte[]> PostImageForBinary(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostImageAsync<byte[]>(url, body, ContentTypeState.Binary, imageType, onProgress, headers, cancellationToken));
		}

		/// <summary>Image �����͸� POST ��û���� �����ϰ� ����� ������ ó���մϴ�.</summary>
		public static async UniTask<AudioClip> PostImageForAudio(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostImageAsync<AudioClip>(url, body, ContentTypeState.Audio, imageType, onProgress, headers, cancellationToken));
		}

		/// <summary>Image �����͸� POST ��û���� �����ϰ� ���� ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostImageForVideo(string url, byte[] body, ImageContentType imageType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostImageAsync<byte[]>(url, body, ContentTypeState.Video, imageType, onProgress, headers, cancellationToken);
				return await HandleVideoResponse(videoData, savePath, cancellationToken);
			});
		}

		/// <summary>Image �����͸� POST ��û���� �����ϰ� �̹��� ������ ó���մϴ�.</summary>
		public static async UniTask<Texture2D> PostImageForImage(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostImageAsync<Texture2D>(url, body, ContentTypeState.Image, imageType, onProgress, headers, cancellationToken));
		}

		/// <summary>Image �����͸� POST ��û���� �����ϴ� ���� �޼����Դϴ�.</summary>
		static async UniTask<T> PostImageAsync<T>(string url, byte[] body, ContentTypeState expectContentType, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			//url ����
			ValidateUrl(url);

			//bodyData ����
			ValidateBody(body);

			headers = SetContentHeader(headers, GetContentTypeFromImageContentType(imageType));

			return await Post<T>(url, body, onProgress, headers, expectContentType, cancellationToken);
		}
	}
}
