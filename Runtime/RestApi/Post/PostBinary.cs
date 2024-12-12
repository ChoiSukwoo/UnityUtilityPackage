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
	internal static class PostBinary
	{
		/// <summary>���̳ʸ� �����͸� POST ��û���� �����ϰ� �ؽ�Ʈ ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostBinaryForText(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostBinaryAsync<string>(url, body, contentType, onProgress, headers, cancellationToken));
		}

		/// <summary>���̳ʸ� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static async UniTask<Res> PostBinaryForJson<Res>(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostBinaryAsync<string>(url, body, contentType, onProgress, headers, cancellationToken);
				return HandleJsonResponse<Res>(jsonResponse);
			});
		}

		/// <summary>���̳ʸ� �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static async UniTask<byte[]> PostBinaryForBinary(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostBinaryAsync<byte[]>(url, body, contentType, onProgress, headers, cancellationToken));
		}

		/// <summary>���̳ʸ� �����͸� POST ��û���� �����ϰ� ����� ������ ó���մϴ�.</summary>
		public static async UniTask<AudioClip> PostBinaryForAudio(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostBinaryAsync<AudioClip>(url, body, contentType, onProgress, headers, cancellationToken));
		}

		/// <summary>���̳ʸ� �����͸� POST ��û���� �����ϰ� ���� ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostBinaryForVideo(string url, byte[] body, string contentType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostBinaryAsync<byte[]>(url, body, contentType, onProgress, headers, cancellationToken);
				return await HandleVideoResponse(videoData, savePath, cancellationToken);
			});
		}

		/// <summary>���̳ʸ� �����͸� POST ��û���� �����ϰ� �̹��� ������ ó���մϴ�.</summary>
		public static async UniTask<Texture2D> PostBinaryForImage(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostBinaryAsync<Texture2D>(url, body, contentType, onProgress, headers, cancellationToken));
		}

		/// <summary>���̳ʸ� �����͸� POST ��û���� �����ϴ� ���� �޼����Դϴ�.</summary>
		static async UniTask<T> PostBinaryAsync<T>(string url, byte[] body, string contentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			ValidateUrl(url);
			ValidateBody(body);
			ValidateContentType(contentType);

			headers = SetContentHeader(headers, contentType);

			// ��û ���� �� ���� ��ȯ
			return await Post<T>(url, body, onProgress, headers, ContentTypeState.Text, cancellationToken);
		}
	}
}
