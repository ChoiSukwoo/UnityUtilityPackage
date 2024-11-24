using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi {
	internal static class PostForJsonResponse {

		/// <summary>��û ���� ���� POST ��û�� ������ JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostNoneForJsonResponse<T>(string url, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleNoneSend(headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}

		/// <summary>�ؽ�Ʈ �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostTextForJsonResponse<T>(string url, string text, string contentType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleTextSend(text, contentType, headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}

		/// <summary>���̳ʸ� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostBinaryForJsonResponse<T>(string url, byte[] binaryData, string contentType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleBinarySend(binaryData, contentType, headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}

		/// <summary>JSON �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostJsonForJsonResponse<TSend, T>(string url, TSend body, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleJsonSend(body, headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}

		/// <summary>����� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostAudioForJsonResponse<T>(string url, byte[] audioData, AudioContentType audioType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleAudioSend(audioData, audioType, headers, (bodyData, unityAudioType) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text, unityAudioType));
		}

		/// <summary>���� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostVideoForJsonResponse<T>(string url, byte[] videoData, VideoContentType videoType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleVideoSend(videoData, videoType, headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}

		/// <summary>�̹��� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostImageForJsonResponse<T>(string url, byte[] imageData, ImageContentType imageType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleImageSend(imageData, imageType, headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}

		/// <summary>��Ƽ��Ʈ �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostMultipartForJsonResponse<T>(string url, MultipartFormData multipartData, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleMultipartSend(multipartData, headers, (bodyData) => Post<string>(url, bodyData, (res) => HandleJsonResponse(res, onComplete), onProgress, headers, ContentTypeState.Text));
		}
	}
}