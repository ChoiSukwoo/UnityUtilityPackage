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
		/// <summary>Audio 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static async UniTask<string> PostAudioForText(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostAudioAsync<string>(url, body, ContentTypeState.Text, audioType, onProgress, headers, cancellationToken));
		}

		/// <summary>Audio 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostAudioForJson<Res>(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				string jsonResponse = await PostAudioAsync<string>(url, body, ContentTypeState.Text, audioType, onProgress, headers, cancellationToken);
				return HandleJsonResponse<Res>(jsonResponse); // JSON 응답 핸들링
			});
		}

		/// <summary>Audio 데이터를 POST 요청으로 전송하고 바이너리 응답을 처리합니다.</summary>
		public static async UniTask<byte[]> PostAudioForBinary(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostAudioAsync<byte[]>(url, body, ContentTypeState.Binary, audioType, onProgress, headers, cancellationToken));
		}

		/// <summary>Audio 데이터를 POST 요청으로 전송하고 오디오 응답을 처리합니다.</summary>
		public static async UniTask<AudioClip> PostAudioForAudio(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostAudioAsync<AudioClip>(url, body, ContentTypeState.Audio, audioType, onProgress, headers, cancellationToken));
		}

		/// <summary>Audio 데이터를 POST 요청으로 전송하고 비디오 응답을 처리합니다.</summary>
		public static async UniTask<string> PostAudioForVideo(string url, byte[] body, AudioContentType audioType, string savePath, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(async () =>
			{
				byte[] videoData = await PostAudioAsync<byte[]>(url, body, ContentTypeState.Video, audioType, onProgress, headers, cancellationToken);
				return await HandleVideoResponse(videoData, savePath, cancellationToken);
			});
		}

		/// <summary>Audio 데이터를 POST 요청으로 전송하고 이미지 응답을 처리합니다.</summary>
		public static async UniTask<Texture2D> PostAudioForImage(string url, byte[] body, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			return await ErrorLogging(() => PostAudioAsync<Texture2D>(url, body, ContentTypeState.Image, audioType, onProgress, headers, cancellationToken));
		}

		/// <summary>Audio 데이터를 POST 요청으로 전송하는 공통 메서드입니다.</summary>
		static async UniTask<T> PostAudioAsync<T>(string url, byte[] body, ContentTypeState expectContentType, AudioContentType audioType, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			//url 검증
			ValidateUrl(url);
			//bodyData 검증
			ValidateBody(body);
			// 헤더 설정
			headers = SetContentHeader(headers, GetContentTypeFromAudioContentType(audioType));
			// 요청 전송 및 응답 반환
			return await Post<T>(url, body, onProgress, headers, expectContentType, cancellationToken);
		}
	}
}
