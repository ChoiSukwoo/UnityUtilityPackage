using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi {
	internal static class RestApiPostText {

		/// <summary>요청 본문 없이 POST 요청을 보내고 텍스트 응답을 처리합니다.</summary>
		public static IEnumerator PostNoneForTextResponse(string url, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleNoneSend(headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Text));
		}

		/// <summary>텍스트 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static IEnumerator PostTextForTextResponse(string url, string text, string contentType, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleTextSend(text, contentType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Text));
		}

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static IEnumerator PostBinaryForTextResponse(string url, byte[] binaryData, string contentType, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleBinarySend(binaryData, contentType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Text));
		}

		/// <summary>JSON 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static IEnumerator PostJsonForTextResponse<TSend>(string url, TSend body, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleJsonSend(body, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Text));
		}

		/// <summary>오디오 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static IEnumerator PostAudioForTextResponse(string url, byte[] audioData, AudioContentType audioType, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleAudioSend(audioData, audioType, headers, (bodyData, unityAudioType) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Text, unityAudioType));
		}

		/// <summary>비디오 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static IEnumerator PostVideoForTextResponse(string url, byte[] videoData, VideoContentType videoType, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleVideoSend(videoData, videoType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Text));
		}

		/// <summary>이미지 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static IEnumerator PostImageForTextResponse(string url, byte[] imageData, ImageContentType imageType, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleImageSend(imageData, imageType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Text));
		}

		/// <summary>멀티파트 데이터를 POST 요청으로 전송하고 텍스트 응답을 처리합니다.</summary>
		public static IEnumerator PostMultipartForTextResponse(string url, MultipartFormData multipartData, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleMultipartSend(multipartData, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Text));
		}
	}
}
