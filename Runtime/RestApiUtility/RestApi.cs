using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Suk.RestApi
{
	public static class RestApi
	{

		#region Get
		public static IEnumerator GetJsonWithAuth<T>(string url, string authToken, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
			=> RestApiGet.GetJsonWithAuth(url, authToken, onComplete, onProgress, headers);

		public static IEnumerator GetJson<T>(string url, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
			=> RestApiGet.GetJson(url, onComplete, onProgress, headers);

		public static IEnumerator GetTextureWithAuth(string url, string authToken, UnityAction<ApiResponse<Texture2D>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
			=> RestApiGet.GetTextureWithAuth(url, authToken, onComplete, onProgress, headers);

		public static IEnumerator GetTexture(string url, UnityAction<ApiResponse<Texture2D>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
			=> RestApiGet.GetTexture(url, onComplete, onProgress, headers);

		public static IEnumerator GetAudioWithAuth(string url, string authToken, AudioContentType audioType = AudioContentType.MP3, UnityAction<ApiResponse<AudioClip>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		=> RestApiGet.GetAudioWithAuth(url, authToken, audioType, onComplete, onProgress, headers);

		public static IEnumerator GetAudio(string url, AudioContentType audioType = AudioContentType.MP3, UnityAction<ApiResponse<AudioClip>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
			=> RestApiGet.GetAudio(url, audioType, onComplete, onProgress, headers);

		public static IEnumerator GetVideoWithAuth(string url, string savePath, string authToken, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
			=> RestApiGet.GetVideoWithAuth(url, savePath, authToken, onComplete, onProgress, headers);

		public static IEnumerator GetVideo(string url, string savePath, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
			=> RestApiGet.GetVideo(url, savePath, onComplete, onProgress, headers);

		public static IEnumerator GetAssetWithAuth(string url, string authToken, UnityAction<ApiResponse<AssetBundle>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
	=> RestApiGet.GetAssetWithAuth(url, authToken, onComplete, onProgress, headers);

		public static IEnumerator GetAsset(string url, UnityAction<ApiResponse<AssetBundle>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
			=> RestApiGet.GetAsset(url, onComplete, onProgress, headers);

		public static IEnumerator GetTextWithAuth(string url, string authToken, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
			=> RestApiGet.GetTextWithAuth(url, authToken, onComplete, onProgress, headers);

		public static IEnumerator GetText(string url, UnityAction<ApiResponse<string>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
			=> RestApiGet.GetText(url, onComplete, onProgress, headers);

		public static IEnumerator GetBinaryWithAuth(string url, string authToken, UnityAction<ApiResponse<byte[]>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
			=> RestApiGet.GetBinaryWithAuth(url, authToken, onComplete, onProgress, headers);

		public static IEnumerator GetBinary(string url, UnityAction<ApiResponse<byte[]>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
			=> RestApiGet.GetBinary(url, onComplete, onProgress, headers);
		#endregion

	}
}
