using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi {
	internal static class PostForAudioResponse {

		/// <summary>��û ���� ���� POST ��û�� ������ JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostNoneForAudioResponse<T>(string url, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<AudioClip>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleNoneSend(headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType));
		}

		/// <summary>�ؽ�Ʈ �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostTextForAudioResponse<T>(string url, string text, string contentType, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleTextSend(text, contentType, headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType));
		}

		/// <summary>���̳ʸ� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostBinaryForAudioResponse<T>(string url, byte[] binaryData, string contentType, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleBinarySend(binaryData, contentType, headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType));
		}

		/// <summary>JSON �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostJsonForAudioResponse<TSend, T>(string url, TSend body, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleJsonSend(body, headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType));
		}

		/// <summary>����� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostAudioForAudioResponse<T>(string url, byte[] audioData, AudioContentType sendAudioType, AudioType receiveAudioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleAudioSend(audioData, sendAudioType, headers, (bodyData, unityAudioType) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, receiveAudioType));
		}

		/// <summary>���� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostVideoForAudioResponse<T>(string url, byte[] videoData, VideoContentType videoType, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleVideoSend(videoData, videoType, headers, (bodyData) => Post<string>(url, bodyData, (res) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType)));
		}

		/// <summary>�̹��� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostImageForAudioResponse<T>(string url, byte[] imageData, ImageContentType imageType, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleImageSend(imageData, imageType, headers, (bodyData) => Post<string>(url, bodyData, (res) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType)));
		}

		/// <summary>��Ƽ��Ʈ �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostMultipartForAudioResponse<T>(string url, MultipartFormData multipartData, AudioType audioType = AudioType.MPEG, UnityAction<ApiResponse<T>> onComplete = null, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleMultipartSend(multipartData, headers, (bodyData) => Post<string>(url, bodyData, (res) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Audio, audioType)));
		}
	}
}