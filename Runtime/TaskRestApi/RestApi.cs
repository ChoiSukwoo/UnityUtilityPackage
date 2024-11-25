using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Suk.RestApi
{
	public static class TaskRestApi
	{
		#region Get
		public static async UniTask<T> GetJsonWithAuth<T>(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetJsonWithAuth<T>(url, authToken, onProgress, headers, cancellationToken);

		public static async UniTask<T> GetJson<T>(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetJson<T>(url, onProgress, headers, cancellationToken);

		public static async UniTask<Texture2D> GetTextureWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetTextureWithAuth(url, authToken, onProgress, headers, cancellationToken);

		public static async UniTask<Texture2D> GetTexture(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetTexture(url, onProgress, headers, cancellationToken);

		public static async UniTask<AudioClip> GetAudioWithAuth(string url, string authToken, AudioContentType audioType = AudioContentType.MP3, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetAudioWithAuth(url, authToken, audioType, onProgress, headers, cancellationToken);

		public static async UniTask<AudioClip> GetAudio(string url, AudioContentType audioType = AudioContentType.MP3, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetAudio(url, audioType, onProgress, headers, cancellationToken);

		public static async UniTask GetVideoWithAuth(string url, string savePath, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetVideoWithAuth(url, savePath, authToken, onProgress, headers, cancellationToken);

		public static async UniTask GetVideo(string url, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetVideo(url, savePath, onProgress, headers, cancellationToken);

		public static async UniTask<AssetBundle> GetAssetWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetAssetWithAuth(url, authToken, onProgress, headers, cancellationToken);

		public static async UniTask<AssetBundle> GetAsset(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetAsset(url, onProgress, headers, cancellationToken);

		public static async UniTask<string> GetTextWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetTextWithAuth(url, authToken, onProgress, headers, cancellationToken);

		public static async UniTask<string> GetText(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetText(url, onProgress, headers, cancellationToken);

		public static async UniTask<byte[]> GetBinaryWithAuth(string url, string authToken, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetBinaryWithAuth(url, authToken, onProgress, headers, cancellationToken);

		public static async UniTask<byte[]> GetBinary(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
			=> await TaskRestApiGet.GetBinary(url, onProgress, headers, cancellationToken);

		#endregion
	}
}
