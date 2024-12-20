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
	internal static class PostVideo
	{
		/// <summary>Video �����͸� POST ��û���� �����ϰ� �ؽ�Ʈ ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostVideoForText(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostVideoAsync<string>(url, body, ContentTypeState.Text, videoType, onProgress, headers, cancellationToken));
		}

		/// <summary>Video �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static async UniTask<Res> PostVideoForJson<Res>(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostVideoAsync<string>(url, body, ContentTypeState.Text, videoType, onProgress, headers, cancellationToken);
				return HandleJsonResponse<Res>(jsonResponse);
			});
		}

		/// <summary>Video �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static async UniTask<byte[]> PostVideoForBinary(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostVideoAsync<byte[]>(url, body, ContentTypeState.Binary, videoType, onProgress, headers, cancellationToken));
		}

		/// <summary>Video �����͸� POST ��û���� �����ϰ� ����� ������ ó���մϴ�.</summary>
		public static async UniTask<AudioClip> PostVideoForAudio(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostVideoAsync<AudioClip>(url, body, ContentTypeState.Audio, videoType, onProgress, headers, cancellationToken));
		}

		/// <summary>Video �����͸� POST ��û���� �����ϰ� ���� ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostVideoForVideo(string url, byte[] body, VideoContentType videoType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostVideoAsync<byte[]>(url, body, ContentTypeState.Video, videoType, onProgress, headers, cancellationToken);
				return await HandleVideoResponse(videoData, savePath, cancellationToken);
			});
		}

		/// <summary>Video �����͸� POST ��û���� �����ϰ� �̹��� ������ ó���մϴ�.</summary>
		public static async UniTask<Texture2D> PostVideoForTexture(string url, byte[] body, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostVideoAsync<Texture2D>(url, body, ContentTypeState.Image, videoType, onProgress, headers, cancellationToken));
		}

		/// <summary>Video �����͸� POST ��û���� �����ϴ� ���� �޼����Դϴ�.</summary>
		static async UniTask<T> PostVideoAsync<T>(string url, byte[] body, ContentTypeState expectContentType, VideoContentType videoType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			//url ����
			ValidateUrl(url);

			//bodyData ����
			ValidateBody(body);

			headers = SetContentHeader(headers, GetContentTypeFromVideoContentType(videoType));

			return await Post<T>(url, body, onProgress, headers, expectContentType, cancellationToken);
		}
	}
}
