using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using static Suk.RestApi.Post.RestApiPost;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi.Post.Josn {
	internal static class RestApiPostJson {

		public static IEnumerator PostNoneForJsonResponse<T>(string url, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleNoneSend(headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}

		public static IEnumerator PostTextForJsonResponse<T>(string url, string text, string contentType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleTextSend(text, contentType, headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}

		// 바이너리 데이터 POST
		public static IEnumerator PostBinaryForJsonResponse<T>(string url, byte[] binaryData, string contentType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleBinarySend(binaryData, contentType, headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}

		public static IEnumerator PostJsonForJsonResponse<TSend, T>(string url, TSend body, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleJsonSend(body, headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}

		public static IEnumerator PostAudioForJsonResponse<T>(string url, byte[] audioData, AudioContentType audioType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleAudioSend(audioData, audioType, headers, (bodyData, unityAudioType) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text, unityAudioType));
		}

		// Send 비디오 Receive Json 
		public static IEnumerator PostVideoForJsonResponse<T>(string url, byte[] videoData, VideoContentType videoType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleVideoSend(videoData, videoType, headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}


		// Send 이미지 Receive Json 
		public static IEnumerator PostImageForJsonResponse<T>(string url, byte[] imageData, ImageContentType imageType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleImageSend(imageData, imageType, headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}

		// 멀티파트 데이터 POST
		public static IEnumerator PostMultipartForJsonResponse<T>(string url, MultipartFormData multipartData, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleMultipartSend(multipartData, headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}
	}
}