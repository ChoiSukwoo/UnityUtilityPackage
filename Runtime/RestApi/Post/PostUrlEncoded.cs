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
	internal static class PostUrlEncoded
	{
		/// <summary>x-www-form-urlencoded �����͸� POST ��û���� �����ϰ� �ؽ�Ʈ ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostUrlEncodedForText(string url, Dictionary<string, string> body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostUrlEncodedAsync<string>(url, body, ContentTypeState.Text, onProgress, headers, cancellationToken));
		}

		/// <summary>x-www-form-urlencoded �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static async UniTask<Res> PostUrlEncodedForJson<Res>(string url, Dictionary<string, string> body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostUrlEncodedAsync<string>(url, body, ContentTypeState.Text, onProgress, headers, cancellationToken);
				return HandleJsonResponse<Res>(jsonResponse);
			});
		}

		/// <summary>x-www-form-urlencoded �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static async UniTask<byte[]> PostUrlEncodedForBinary(string url, Dictionary<string, string> body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostUrlEncodedAsync<byte[]>(url, body, ContentTypeState.Binary, onProgress, headers, cancellationToken));
		}

		/// <summary>x-www-form-urlencoded �����͸� POST ��û���� �����ϰ� ����� ������ ó���մϴ�.</summary>
		public static async UniTask<AudioClip> PostUrlEncodedForAudio(string url, Dictionary<string, string> body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostUrlEncodedAsync<AudioClip>(url, body, ContentTypeState.Audio, onProgress, headers, cancellationToken));
		}

		/// <summary>x-www-form-urlencoded �����͸� POST ��û���� �����ϰ� ���� ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostUrlEncodedForVideo(string url, Dictionary<string, string> body, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostUrlEncodedAsync<byte[]>(url, body, ContentTypeState.Video, onProgress, headers, cancellationToken);
				return await HandleVideoResponse(videoData, savePath, cancellationToken);
			});
		}

		/// <summary>x-www-form-urlencoded �����͸� POST ��û���� �����ϰ� �̹��� ������ ó���մϴ�.</summary>
		public static async UniTask<Texture2D> PostUrlEncodedForTexture(string url, Dictionary<string, string> body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostUrlEncodedAsync<Texture2D>(url, body, ContentTypeState.Image, onProgress, headers, cancellationToken));
		}

		private static async UniTask<T> PostUrlEncodedAsync<T>(string url, Dictionary<string, string> body, ContentTypeState expectContentType, UnityAction<float> onProgress, Dictionary<string, string> headers, CancellationToken cancellationToken)
		{
			//url ����
			ValidateUrl(url);

			//bodyData ����
			byte[] bodyData = ConvertDictionaryToBytes(body);
			ValidateBody(bodyData);

			// ��� ����
			headers = SetContentHeader(headers, "application/x-www-form-urlencoded");

			// POST ��û ����
			return await Post<T>(url, bodyData, onProgress, headers, expectContentType, cancellationToken);
		}

	}
}
