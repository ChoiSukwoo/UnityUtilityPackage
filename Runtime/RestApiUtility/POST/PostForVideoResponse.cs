using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;


namespace Suk.RestApi {
	internal static class RestApiPostVideo {

		/// <summary>��û ���� ���� POST ��û�� ������ ���� ������ �����մϴ�.</summary>
		public static IEnumerator PostNoneForVideoResponse(string url, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleNoneSend(headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

		/// <summary>�ؽ�Ʈ �����͸� POST ��û���� �����ϰ� ���� ������ �����մϴ�.</summary>
		public static IEnumerator PostTextForVideoResponse(string url, string text, string contentType, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleTextSend(text, contentType, headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

		/// <summary>���̳ʸ� �����͸� POST ��û���� �����ϰ� ���� ������ �����մϴ�.</summary>
		public static IEnumerator PostBinaryForVideoResponse(string url, byte[] binaryData, string contentType, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleBinarySend(binaryData, contentType, headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

		/// <summary>JSON �����͸� POST ��û���� �����ϰ� ���� ������ �����մϴ�.</summary>
		public static IEnumerator PostJsonForVideoResponse<TSend>(string url, TSend body, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleJsonSend(body, headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

		/// <summary>����� �����͸� POST ��û���� �����ϰ� ���� ������ �����մϴ�.</summary>
		public static IEnumerator PostAudioForVideoResponse(string url, byte[] audioData, AudioContentType audioType, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleAudioSend(audioData, audioType, headers, (bodyData, unityAudioType) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video, unityAudioType));
		}

		/// <summary>���� �����͸� POST ��û���� �����ϰ� ���� ������ �����մϴ�.</summary>
		public static IEnumerator PostVideoForVideoResponse(string url, byte[] videoData, VideoContentType videoType, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleVideoSend(videoData, videoType, headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

		/// <summary>�̹��� �����͸� POST ��û���� �����ϰ� ���� ������ �����մϴ�.</summary>
		public static IEnumerator PostImageForVideoResponse(string url, byte[] imageData, ImageContentType imageType, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleImageSend(imageData, imageType, headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

		/// <summary>��Ƽ��Ʈ �����͸� POST ��û���� �����ϰ� ���� ������ �����մϴ�.</summary>
		public static IEnumerator PostMultipartForVideoResponse(string url, MultipartFormData multipartData, string savePath, UnityAction<ApiResponse<string>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleMultipartSend(multipartData, headers, (bodyData) => Post<byte[]>(url, bodyData, (res) => HandleVideoResponse(res, savePath, onComplete), onProgress, headers, ContentTypeState.Video));
		}

	}
}
