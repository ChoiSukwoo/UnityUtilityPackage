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
	internal static class PostMultipart
	{
		/// <summary>Multipart 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostMultipartForText(string url, MultipartFormData body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostMultipartAsync<string>(url, body, ContentTypeState.Text, onProgress, headers, cancellationToken));
		}

		/// <summary>Multipart 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostMultipartForJson<Res>(string url, MultipartFormData body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostMultipartAsync<string>(url, body, ContentTypeState.Text, onProgress, headers, cancellationToken);
				return HandleJsonResponse<Res>(jsonResponse);
			});
		}

		/// <summary>Multipart 데이터를 POST 요청으로 전송하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostMultipartForBinary(string url, MultipartFormData body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostMultipartAsync<byte[]>(url, body, ContentTypeState.Binary, onProgress, headers, cancellationToken));
		}

		/// <summary>Multipart 데이터를 POST 요청으로 전송하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostMultipartForAudio(string url, MultipartFormData body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostMultipartAsync<AudioClip>(url, body, ContentTypeState.Audio, onProgress, headers, cancellationToken));
		}

		/// <summary>Multipart 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostMultipartForVideo(string url, MultipartFormData body, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostMultipartAsync<byte[]>(url, body, ContentTypeState.Video, onProgress, headers, cancellationToken);
				return await HandleVideoResponse(videoData, savePath, cancellationToken);
			});
		}

		/// <summary>Multipart 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostMultipartForImage(string url, MultipartFormData body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostMultipartAsync<Texture2D>(url, body, ContentTypeState.Image, onProgress, headers, cancellationToken));
		}

		/// <summary>Multipart 데이터를 POST 요청으로 전송하는 공통 메서드입니다.</summary>
		private static async UniTask<T> PostMultipartAsync<T>(string url, MultipartFormData body, ContentTypeState expectContentType, UnityAction<float> onProgress, Dictionary<string, string> headers, CancellationToken cancellationToken)
		{
			//url 검증
			ValidateUrl(url);

			//bodyData 검증
			byte[] bodyData = body.ToBytes();
			ValidateBody(bodyData);

			//헤더 적용
			headers = SetContentHeader(headers, $"multipart/form-data; boundary={body.Boundary}");

			return await Post<T>(url, bodyData, onProgress, headers, expectContentType, cancellationToken);
		}
	}
}
