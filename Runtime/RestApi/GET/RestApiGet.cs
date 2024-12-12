using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi
{
	internal static class RestApiGet
	{
		public static async UniTask<T> GetJson<T>(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				ValidateUrl(url);
				string jsonString = await Get<string>(url, onProgress, headers, ContentTypeState.Text, cancellationToken);
				return HandleJsonResponse<T>(jsonString);
			});
		}

		public static async UniTask<Texture2D> GetTexture(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				ValidateUrl(url);
				return await Get<Texture2D>(url, onProgress, headers, ContentTypeState.Image, cancellationToken);
			});
		}

		public static async UniTask<AudioClip> GetAudio(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				ValidateUrl(url);
				return await Get<AudioClip>(url, onProgress, headers, ContentTypeState.Audio, cancellationToken);
			});
		}

		public static async UniTask<AssetBundle> GetAsset(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				ValidateUrl(url);
				return await Get<AssetBundle>(url, onProgress, headers, ContentTypeState.Asset, cancellationToken);
			});
		}

		public static async UniTask<string> GetVideo(string url, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				ValidateUrl(url);
				ValidatePath(savePath);
				byte[] videoData = await Get<byte[]>(url, onProgress, headers, ContentTypeState.Video, cancellationToken);
				return await HandleVideoResponse(videoData, savePath, cancellationToken);
			});
		}

		public static async UniTask<string> GetText(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				ValidateUrl(url);
				return await Get<string>(url, onProgress, headers, ContentTypeState.Text, cancellationToken);
			});
		}

		public static async UniTask<byte[]> GetBinary(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				ValidateUrl(url);
				return await Get<byte[]>(url, onProgress, headers, ContentTypeState.Binary, cancellationToken);
			});
		}
	}
}

