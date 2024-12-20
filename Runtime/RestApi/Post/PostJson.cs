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
	internal static class PostJson
	{

		/// <summary>�ܼ� Text �����͸� POST ��û���� �����ϰ� �ؽ�Ʈ ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostJsonForText<Req>(string url, Req body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostJsonAsync<Req, string>(url, body, ContentTypeState.Text, onProgress, headers, cancelToken));
		}

		/// <summary>JSON �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static async UniTask<Res> PostJsonForJson<Req, Res>(string url, Req body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostJsonAsync<Req, string>(url, body, ContentTypeState.Text, onProgress, headers, cancelToken);
				return HandleJsonResponse<Res>(jsonResponse);
			});
		}

		/// <summary>JSON �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static async UniTask<byte[]> PostJsonForBinary<Req>(string url, Req body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(async () => await PostJsonAsync<Req, byte[]>(url, body, ContentTypeState.Binary, onProgress, headers, cancelToken));
		}

		/// <summary>JSON �����͸� POST ��û���� �����ϰ� ����� ������ ó���մϴ�.</summary>
		public static async UniTask<AudioClip> PostJsonForAudio<Req>(string url, Req body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostJsonAsync<Req, AudioClip>(url, body, ContentTypeState.Audio, onProgress, headers, cancelToken));
		}

		/// <summary>JSON �����͸� POST ��û���� �����ϰ� ���� ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostJsonForVideo<Req>(string url, Req body, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostJsonAsync<Req, byte[]>(url, body, ContentTypeState.Video, onProgress, headers, cancelToken);
				return await HandleVideoResponse(videoData, savePath, cancelToken);
			});
		}

		/// <summary>JSON �����͸� POST ��û���� �����ϰ� �̹��� ������ ó���մϴ�.</summary>
		public static async UniTask<Texture2D> PostJsonForTexture<Req>(string url, Req body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostJsonAsync<Req, Texture2D>(url, body, ContentTypeState.Image, onProgress, headers, cancelToken));
		}

		/// <summary>JSON �����͸� POST ��û���� �����ϰ� �ؽ�Ʈ ������ ó���մϴ�.</summary>
		static async UniTask<Res> PostJsonAsync<Req, Res>(string url, Req body, ContentTypeState expectContentType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			//url ����
			ValidateUrl(url);

			//bodyData ����
			byte[] bodyData = HandleJsonBody(body);
			ValidateBody(bodyData);

			//��� ����
			headers = SetContentHeader(headers, "application/json");

			// POST ��û ����
			return await Post<Res>(url, bodyData, onProgress, headers, expectContentType, cancelToken);
		}
	}
}
