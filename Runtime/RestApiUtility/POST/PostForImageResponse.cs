using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi
{
	internal static class RestApiPostImage
	{

		/// <summary>요청 본문 없이 POST 요청을 보내고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostNoneForImageResponse<T>(string url, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleNoneSend(headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}

		/// <summary>텍스트 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostTextForImageResponse<T>(string url, string text, string contentType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleTextSend(text, contentType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostBinaryForImageResponse<T>(string url, byte[] binaryData, string contentType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleBinarySend(binaryData, contentType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}

		/// <summary>JSON 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostJsonForImageResponse<TSend, T>(string url, TSend body, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleJsonSend(body, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}

		/// <summary>오디오 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostAudioForImageResponse<T>(string url, byte[] audioData, AudioContentType audioType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleAudioSend(audioData, audioType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image, audioType));
		}

		/// <summary>비디오 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostVideoForImageResponse<T>(string url, byte[] videoData, VideoContentType videoType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleVideoSend(videoData, videoType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}

		/// <summary>이미지 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostImageForImageResponse<T>(string url, byte[] imageData, ImageContentType imageType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleImageSend(imageData, imageType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}

		/// <summary>멀티파트 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostMultipartForImageResponse<T>(string url, MultipartFormData multipartData, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleMultipartSend(multipartData, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}
	}
}
