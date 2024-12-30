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
	internal static class PostMultiform
	{
		/// <summary>Multipart �����͸� POST ��û���� �����ϰ� �ؽ�Ʈ ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostMultiformForText(string url, MultipartFormData body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostMultiformAsync<string>(url, body, ContentTypeState.Text, onProgress, headers, cancellationToken));
		}

		/// <summary>Multipart �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static async UniTask<Res> PostMultiformForJson<Res>(string url, MultipartFormData body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostMultiformAsync<string>(url, body, ContentTypeState.Text, onProgress, headers, cancellationToken);
				return HandleJsonResponse<Res>(jsonResponse);
			});
		}

		/// <summary>Multipart �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static async UniTask<byte[]> PostMultiformForBinary(string url, MultipartFormData body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostMultiformAsync<byte[]>(url, body, ContentTypeState.Binary, onProgress, headers, cancellationToken));
		}

		/// <summary>Multipart �����͸� POST ��û���� �����ϰ� ����� ������ ó���մϴ�.</summary>
		public static async UniTask<AudioClip> PostMultiformForAudio(string url, MultipartFormData body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostMultiformAsync<AudioClip>(url, body, ContentTypeState.Audio, onProgress, headers, cancellationToken));
		}

		/// <summary>Multipart �����͸� POST ��û���� �����ϰ� ���� ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostMultiformForVideo(string url, MultipartFormData body, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostMultiformAsync<byte[]>(url, body, ContentTypeState.Video, onProgress, headers, cancellationToken);
				return await HandleVideoResponse(videoData, savePath, cancellationToken);
			});
		}

		/// <summary>Multipart �����͸� POST ��û���� �����ϰ� �̹��� ������ ó���մϴ�.</summary>
		public static async UniTask<Texture2D> PostMultiformForTexture(string url, MultipartFormData body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostMultiformAsync<Texture2D>(url, body, ContentTypeState.Image, onProgress, headers, cancellationToken));
		}

		/// <summary>Multipart �����͸� POST ��û���� �����ϴ� ���� �޼����Դϴ�.</summary>
		private static async UniTask<T> PostMultiformAsync<T>(string url, MultipartFormData body, ContentTypeState expectContentType, UnityAction<float> onProgress, Dictionary<string, string> headers, CancellationToken cancellationToken)
		{
			//url ����
			ValidateUrl(url);

			//bodyData ����
			byte[] bodyData = body.ToBytes();
			ValidateBody(bodyData);

			//��� ����
			headers = SetContentHeader(headers, $"multipart/form-data; boundary={body.Boundary}");

			return await Post<T>(url, bodyData, onProgress, headers, expectContentType, cancellationToken);
		}
	}
}
