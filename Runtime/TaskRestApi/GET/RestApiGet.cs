using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Suk.Json;
using UnityEngine;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi
{
	internal static class TaskRestApiGet
	{

		public static async UniTask<T> GetJsonWithAuth<T>(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await GetJson<T>(url, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<T> GetJson<T>(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			string jsonString = await Get<string>(url, onProgress, headers, ContentTypeState.Text, cancellationToken);
			return JsonParser.Parse<T>(jsonString);
		}

		public static async UniTask<Texture2D> GetTextureWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await GetTexture(url, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<Texture2D> GetTexture(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await Get<Texture2D>(url, onProgress, headers, ContentTypeState.Image, cancellationToken);

		public static async UniTask<AudioClip> GetAudioWithAuth(string url, string authToken, AudioContentType audioType = AudioContentType.MP3, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await GetAudio(url, audioType, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<AudioClip> GetAudio(string url, AudioContentType audioType = AudioContentType.Auto, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await Get<AudioClip>(url, onProgress, headers, ContentTypeState.Audio, cancellationToken, audioType);

		public static async UniTask<AssetBundle> GetAssetWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await GetAsset(url, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<AssetBundle> GetAsset(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await Get<AssetBundle>(url, onProgress, headers, ContentTypeState.Asset, cancellationToken);

		public static async UniTask<string> GetVideoWithAuth(string url, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await GetVideo(url, savePath, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<string> GetVideo(string url, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(savePath))
				throw new ArgumentException("Save path cannot be null or empty.");
			byte[] videoData = await Get<byte[]>(url, onProgress, headers, ContentTypeState.Video, cancellationToken);
			return await HandleVideoResponse(videoData, savePath, cancellationToken);
		}

		public static async UniTask<string> GetTextWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await GetText(url, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<string> GetText(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await Get<string>(url, onProgress, headers, ContentTypeState.Text, cancellationToken);

		public static async UniTask<byte[]> GetBinaryWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await GetBinary(url, onProgress, SetAuthHeader(headers, authToken), cancellationToken);

		public static async UniTask<byte[]> GetBinary(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await Get<byte[]>(url, onProgress, headers, ContentTypeState.Binary, cancellationToken);
	}
}
