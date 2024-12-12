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
	internal static class PostAudio
	{
		/// <summary>Audio �����͸� POST ��û���� �����ϰ� �ؽ�Ʈ ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostAudioForText(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostAudioAsync<string>(url, body, ContentTypeState.Text, audioType, onProgress, headers, cancellationToken));
		}

		/// <summary>Audio �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static async UniTask<Res> PostAudioForJson<Res>(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostAudioAsync<string>(url, body, ContentTypeState.Text, audioType, onProgress, headers, cancellationToken);
				return HandleJsonResponse<Res>(jsonResponse); // JSON ���� �ڵ鸵
			});
		}

		/// <summary>Audio �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static async UniTask<byte[]> PostAudioForBinary(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostAudioAsync<byte[]>(url, body, ContentTypeState.Binary, audioType, onProgress, headers, cancellationToken));
		}

		/// <summary>Audio �����͸� POST ��û���� �����ϰ� ����� ������ ó���մϴ�.</summary>
		public static async UniTask<AudioClip> PostAudioForAudio(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostAudioAsync<AudioClip>(url, body, ContentTypeState.Audio, audioType, onProgress, headers, cancellationToken));
		}

		/// <summary>Audio �����͸� POST ��û���� �����ϰ� ���� ������ ó���մϴ�.</summary>
		public static async UniTask<string> PostAudioForVideo(string url, byte[] body, AudioContentType audioType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostAudioAsync<byte[]>(url, body, ContentTypeState.Video, audioType, onProgress, headers, cancellationToken);
				return await HandleVideoResponse(videoData, savePath, cancellationToken);
			});
		}

		/// <summary>Audio �����͸� POST ��û���� �����ϰ� �̹��� ������ ó���մϴ�.</summary>
		public static async UniTask<Texture2D> PostAudioForImage(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostAudioAsync<Texture2D>(url, body, ContentTypeState.Image, audioType, onProgress, headers, cancellationToken));
		}

		/// <summary>Audio �����͸� POST ��û���� �����ϴ� ���� �޼����Դϴ�.</summary>
		static async UniTask<T> PostAudioAsync<T>(string url, byte[] body, ContentTypeState expectContentType, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			//url ����
			ValidateUrl(url);
			//bodyData ����
			ValidateBody(body);
			// ��� ����
			headers = SetContentHeader(headers, GetContentTypeFromAudioContentType(audioType));
			// ��û ���� �� ���� ��ȯ
			return await Post<T>(url, body, onProgress, headers, expectContentType, cancellationToken);
		}
	}
}
