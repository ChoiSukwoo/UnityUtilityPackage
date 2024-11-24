using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;


namespace Suk.RestApi {
	internal static class RestApiPostVideo {

		/// <summary>요청 본문 없이 POST 요청을 보내고 비디오 응답을 저장합니다.</summary>
		public static IEnumerator PostNoneForVideoResponse(string url, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleNoneSend(headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

		/// <summary>텍스트 데이터를 POST 요청으로 전송하고 비디오 응답을 저장합니다.</summary>
		public static IEnumerator PostTextForVideoResponse(string url, string text, string contentType, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleTextSend(text, contentType, headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 비디오 응답을 저장합니다.</summary>
		public static IEnumerator PostBinaryForVideoResponse(string url, byte[] binaryData, string contentType, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleBinarySend(binaryData, contentType, headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

		/// <summary>JSON 데이터를 POST 요청으로 전송하고 비디오 응답을 저장합니다.</summary>
		public static IEnumerator PostJsonForVideoResponse<TSend>(string url, TSend body, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleJsonSend(body, headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

		/// <summary>오디오 데이터를 POST 요청으로 전송하고 비디오 응답을 저장합니다.</summary>
		public static IEnumerator PostAudioForVideoResponse(string url, byte[] audioData, AudioContentType audioType, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleAudioSend(audioData, audioType, headers, (bodyData, unityAudioType) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video, unityAudioType));
		}

		/// <summary>비디오 데이터를 POST 요청으로 전송하고 비디오 응답을 저장합니다.</summary>
		public static IEnumerator PostVideoForVideoResponse(string url, byte[] videoData, VideoContentType videoType, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleVideoSend(videoData, videoType, headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

		/// <summary>이미지 데이터를 POST 요청으로 전송하고 비디오 응답을 저장합니다.</summary>
		public static IEnumerator PostImageForVideoResponse(string url, byte[] imageData, ImageContentType imageType, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleImageSend(imageData, imageType, headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

		/// <summary>멀티파트 데이터를 POST 요청으로 전송하고 비디오 응답을 저장합니다.</summary>
		public static IEnumerator PostMultipartForVideoResponse(string url, MultipartFormData multipartData, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleMultipartSend(multipartData, headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

	}
}
