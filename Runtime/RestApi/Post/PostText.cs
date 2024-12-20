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

		/// <summary>단순 Text 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostTextForText(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostTextAsync<string>(url, body, ContentTypeState.Text, contentType, onProgress, headers, cancelToken));
		}

		/// <summary>단순 Text 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostTextForJson<Res>(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostTextAsync<string>(url, body, ContentTypeState.Text, contentType, onProgress, headers, cancelToken);
				return HandleJsonResponse<Res>(jsonResponse);
			});
		}

		/// <summary>단순 Text 데이터를 POST 요청으로 전송하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostTextForBinary(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostTextAsync<byte[]>(url, body, ContentTypeState.Binary, contentType, onProgress, headers, cancelToken));
		}

		/// <summary>단순 Text 데이터를 POST 요청으로 전송하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostTextForAudio(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostTextAsync<AudioClip>(url, body, ContentTypeState.Audio, contentType, onProgress, headers, cancelToken));
		}

		/// <summary>단순 Text 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostTextForVideo(string url, string body, string savePath, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostTextAsync<byte[]>(url, body, ContentTypeState.Video, contentType, onProgress, headers, cancelToken);
				return await HandleVideoResponse(videoData, savePath, cancelToken);
			});
		}

		/// <summary>단순 Text 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostTextForTexture(string url, string body, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			return await ErrorLogging(() => PostTextAsync<Texture2D>(url, body, ContentTypeState.Image, contentType, onProgress, headers, cancelToken));
		}

		/// <summary> PostText 공통 처리 함수</summary>
		static async UniTask<T> PostTextAsync<T>(string url, string body, ContentTypeState expectContentType, string contentType = "", UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancelToken = default)
		{
			//url 검증
			ValidateUrl(url);

			//bodyData 검증
			byte[] bodyData = ConvertTextToBytes(body);
			ValidateBody(bodyData);

			//헤더 적용
			headers = SetContentHeader(headers, string.IsNullOrEmpty(contentType) ? "text/plain" : contentType);

			T postResult = await Post<T>(url, bodyData, onProgress, headers, expectContentType, cancelToken);
			return postResult;
		}
	}
}
