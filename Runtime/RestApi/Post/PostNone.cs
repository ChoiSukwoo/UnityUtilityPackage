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
	internal static class PostNone
	{
		/// <summary>빈 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostNoneForText(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostNoneAsync<string>(url, ContentTypeState.Text, onProgress, headers, cancelToken));
		}

		/// <summary>빈 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostNoneForJson<Res>(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostNoneAsync<string>(url, ContentTypeState.Text, onProgress, headers, cancelToken);
				return HandleJsonResponse<Res>(jsonResponse);
			});
		}

		/// <summary>빈 데이터를 POST 요청으로 전송하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostNoneForBinary(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostNoneAsync<byte[]>(url, ContentTypeState.Binary, onProgress, headers, cancelToken));
		}

		/// <summary>빈 데이터를 POST 요청으로 전송하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostNoneForAudio(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostNoneAsync<AudioClip>(url, ContentTypeState.Audio, onProgress, headers, cancelToken));
		}

		/// <summary>빈 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostNoneForVideo(string url, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostNoneAsync<byte[]>(url, ContentTypeState.Video, onProgress, headers, cancelToken);
				return await HandleVideoResponse(videoData, savePath, cancelToken);
			});
		}

		/// <summary>빈 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostNoneForTexture(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostNoneAsync<Texture2D>(url, ContentTypeState.Image, onProgress, headers, cancelToken));
		}

		/// <summary> PostNone 공통 처리 함수</summary>
		static async UniTask<T> PostNoneAsync<T>(string url, ContentTypeState contentTypeState, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			ValidateUrl(url);
			return await Post<T>(url, new byte[0], onProgress, headers, contentTypeState, cancelToken);
		}
	}
}