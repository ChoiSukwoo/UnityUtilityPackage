using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi {
	internal static class PostForAudioResponse {

		/// <summary>요청 본문 없이 POST 요청을 보내고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostNoneForAudioResponse<T>(string url, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<AudioClip>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleNoneSend(headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType));
		}

		/// <summary>텍스트 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostTextForAudioResponse<T>(string url, string text, string contentType, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleTextSend(text, contentType, headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType));
		}

		/// <summary>바이너리 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostBinaryForAudioResponse<T>(string url, byte[] binaryData, string contentType, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleBinarySend(binaryData, contentType, headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType));
		}

		/// <summary>JSON 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostJsonForAudioResponse<TSend, T>(string url, TSend body, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleJsonSend(body, headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType));
		}

		/// <summary>오디오 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostAudioForAudioResponse<T>(string url, byte[] audioData, AudioContentType sendAudioType, AudioType receiveAudioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleAudioSend(audioData, sendAudioType, headers, (bodyData, unityAudioType) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, receiveAudioType));
		}

		/// <summary>비디오 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostVideoForAudioResponse<T>(string url, byte[] videoData, VideoContentType videoType, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleVideoSend(videoData, videoType, headers, (bodyData) => Post<string>(url, bodyData, (res) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType)));
		}

		/// <summary>이미지 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostImageForAudioResponse<T>(string url, byte[] imageData, ImageContentType imageType, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleImageSend(imageData, imageType, headers, (bodyData) => Post<string>(url, bodyData, (res) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType)));
		}

		/// <summary>멀티파트 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static IEnumerator PostMultipartForAudioResponse<T>(string url, MultipartFormData multipartData, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleMultipartSend(multipartData, headers, (bodyData) => Post<string>(url, bodyData, (res) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType)));
		}
	}
}