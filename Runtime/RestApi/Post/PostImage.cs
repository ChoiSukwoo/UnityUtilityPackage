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
	internal static class PostImage
	{
		/// <summary>Image 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostImageForText(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostImageAsync<string>(url, body, ContentTypeState.Text, imageType, onProgress, headers, cancellationToken));
		}

		/// <summary>Image 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostImageForJson<Res>(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostImageAsync<string>(url, body, ContentTypeState.Text, imageType, onProgress, headers, cancellationToken);
				return HandleJsonResponse<Res>(jsonResponse);
			});
		}

		/// <summary>Image 데이터를 POST 요청으로 전송하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostImageForBinary(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostImageAsync<byte[]>(url, body, ContentTypeState.Binary, imageType, onProgress, headers, cancellationToken));
		}

		/// <summary>Image 데이터를 POST 요청으로 전송하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostImageForAudio(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostImageAsync<AudioClip>(url, body, ContentTypeState.Audio, imageType, onProgress, headers, cancellationToken));
		}

		/// <summary>Image 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostImageForVideo(string url, byte[] body, ImageContentType imageType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostImageAsync<byte[]>(url, body, ContentTypeState.Video, imageType, onProgress, headers, cancellationToken);
				return await HandleVideoResponse(videoData, savePath, cancellationToken);
			});
		}

		/// <summary>Image 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostImageForImage(string url, byte[] body, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostImageAsync<Texture2D>(url, body, ContentTypeState.Image, imageType, onProgress, headers, cancellationToken));
		}

		/// <summary>Image 데이터를 POST 요청으로 전송하는 공통 메서드입니다.</summary>
		static async UniTask<T> PostImageAsync<T>(string url, byte[] body, ContentTypeState expectContentType, ImageContentType imageType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			//url 검증
			ValidateUrl(url);

			//bodyData 검증
			ValidateBody(body);

			headers = SetContentHeader(headers, GetContentTypeFromImageContentType(imageType));

			return await Post<T>(url, body, onProgress, headers, expectContentType, cancellationToken);
		}
	}
}
