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
	internal static class PostText
	{

		/// <summary>�ܼ� Text �����͸� POST ��û���� �����ϰ� �ؽ�Ʈ ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostTextForText(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostTextAsync<string>(url, body, ContentTypeState.Text, contentType, onProgress, headers, cancelToken));
		}

		/// <summary>�ܼ� Text �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static async UniTask<Res> PostTextForJson<Res>(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostTextAsync<string>(url, body, ContentTypeState.Text, contentType, onProgress, headers, cancelToken);
				return HandleJsonResponse<Res>(jsonResponse);
			});
		}

		/// <summary>�ܼ� Text �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static async UniTask<byte[]> PostTextForBinary(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostTextAsync<byte[]>(url, body, ContentTypeState.Binary, contentType, onProgress, headers, cancelToken));
		}

		/// <summary>�ܼ� Text �����͸� POST ��û���� �����ϰ� ����� ������ ó���մϴ�.</summary>
		public static async UniTask<AudioClip> PostTextForAudio(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostTextAsync<AudioClip>(url, body, ContentTypeState.Audio, contentType, onProgress, headers, cancelToken));
		}

		/// <summary>�ܼ� Text �����͸� POST ��û���� �����ϰ� ���� ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostTextForVideo(string url, string body, string savePath, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostTextAsync<byte[]>(url, body, ContentTypeState.Video, contentType, onProgress, headers, cancelToken);
				return await HandleVideoResponse(videoData, savePath, cancelToken);
			});
		}

		/// <summary>�ܼ� Text �����͸� POST ��û���� �����ϰ� �̹��� ������ ó���մϴ�.</summary>
		public static async UniTask<Texture2D> PostTextForTexture(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostTextAsync<Texture2D>(url, body, ContentTypeState.Image, contentType, onProgress, headers, cancelToken));
		}

		/// <summary> PostText ���� ó�� �Լ�</summary>
		static async UniTask<T> PostTextAsync<T>(string url, string body, ContentTypeState expectContentType, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			//url ����
			ValidateUrl(url);

			//bodyData ����
			byte[] bodyData = ConvertTextToBytes(body);
			ValidateBody(bodyData);

			//��� ����
			headers = SetContentHeader(headers, string.IsNullOrEmpty(contentType) ? "text/plain" : contentType);

			T postResult = await Post<T>(url, bodyData, onProgress, headers, expectContentType, cancelToken);
			return postResult;
		}
	}
}
