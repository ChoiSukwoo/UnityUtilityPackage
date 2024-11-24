using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi {
	internal static class RestApiGet {

		public static IEnumerator GetJsonWithAuth<T>(string url, string authToken, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return ExecuteWithAuth(authToken, headers, () => GetJson(url, onComplete, onProgress, headers));
		}

		public static IEnumerator GetJson<T>(string url, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return Get<string>(url, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text);
		}

		public static IEnumerator GetTextureWithAuth(string url, string authToken, UnityAction<ApiResponse<Texture2D>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return ExecuteWithAuth(authToken, headers, () => GetTexture(url, onComplete, onProgress, headers));
		}

		public static IEnumerator GetTexture(string url, UnityAction<ApiResponse<Texture2D>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return Get(url, onComplete, onProgress, headers, ContentTypeState.Image);
		}

		public static IEnumerator GetAudioWithAuth(string url, string authToken, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<AudioClip>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return ExecuteWithAuth(authToken, headers, () => GetAudio(url, audioType, onComplete, onProgress, headers));
		}

		public static IEnumerator GetAudio(string url, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<AudioClip>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return Get(url, onComplete, onProgress, headers, ContentTypeState.Audio, audioType);
		}

		public static IEnumerator GetAssetWithAuth(string url, string authToken, UnityAction<ApiResponse<AssetBundle>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return ExecuteWithAuth(authToken, headers, () => GetAsset(url, onComplete, onProgress, headers));
		}

		public static IEnumerator GetAsset(string url, UnityAction<ApiResponse<AssetBundle>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return Get(url, onComplete, onProgress, headers, ContentTypeState.Asset);
		}

		public static IEnumerator GetVideoWithAuth(string url, string savePath, string authToken, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return ExecuteWithAuth(authToken, headers, () => GetVideo(url, savePath, onComplete, onProgress, headers));
		}

		public static IEnumerator GetVideo(string url, string savePath, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			if(string.IsNullOrWhiteSpace(savePath)) {
				onComplete?.Invoke(new FailureResponse<string>("Save path cannot be null or empty."));
				yield break;
			}
			yield return Get<byte[]>(url, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video);
		}

		public static IEnumerator GetTextWithAuth(string url, string authToken, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return ExecuteWithAuth(authToken, headers, () => GetText(url, onComplete, onProgress, headers));
		}

		public static IEnumerator GetText(string url, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return Get(url, onComplete, onProgress, headers, ContentTypeState.Text);
		}

		public static IEnumerator GetBinaryWithAuth(string url, string authToken, UnityAction<ApiResponse<byte[]>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return ExecuteWithAuth(authToken, headers, () => GetBinary(url, onComplete, onProgress, headers));
		}

		public static IEnumerator GetBinary(string url, UnityAction<ApiResponse<byte[]>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return Get(url, onComplete, onProgress, headers, ContentTypeState.Binary);
		}

	}
}
