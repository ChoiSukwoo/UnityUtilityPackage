using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi {
	internal static class RestApiPostBinary {

		/// <summary>��û ���� ���� POST ��û�� ������ ���̳ʸ� ������ ó���մϴ�.</summary>
		public static IEnumerator PostNoneForBinaryResponse(string url, UnityAction<ApiResponse<byte[]>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleNoneSend(headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Binary));
		}

		/// <summary>�ؽ�Ʈ �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static IEnumerator PostTextForBinaryResponse(string url, string text, string contentType, UnityAction<ApiResponse<byte[]>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleTextSend(text, contentType, headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Binary));
		}

		/// <summary>���̳ʸ� �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static IEnumerator PostBinaryForBinaryResponse(string url, byte[] binaryData, string contentType, UnityAction<ApiResponse<byte[]>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleBinarySend(binaryData, contentType, headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Binary));
		}

		/// <summary>JSON �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static IEnumerator PostJsonForBinaryResponse<TSend>(string url, TSend body, UnityAction<ApiResponse<byte[]>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleJsonSend(body, headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Binary));
		}

		/// <summary>����� �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static IEnumerator PostAudioForBinaryResponse(string url, byte[] audioData, AudioContentType audioType, UnityAction<ApiResponse<byte[]>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleAudioSend(audioData, audioType, headers, (bodyData, unityAudioType) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Binary, unityAudioType));
		}

		/// <summary>���� �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static IEnumerator PostVideoForBinaryResponse(string url, byte[] videoData, VideoContentType videoType, UnityAction<ApiResponse<byte[]>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleVideoSend(videoData, videoType, headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Binary));
		}

		/// <summary>�̹��� �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static IEnumerator PostImageForBinaryResponse(string url, byte[] imageData, ImageContentType imageType, UnityAction<ApiResponse<byte[]>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleImageSend(imageData, imageType, headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Binary));
		}

		/// <summary>��Ƽ��Ʈ �����͸� POST ��û���� �����ϰ� ���̳ʸ� ������ ó���մϴ�.</summary>
		public static IEnumerator PostMultipartForBinaryResponse(string url, MultipartFormData multipartData, UnityAction<ApiResponse<byte[]>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null) {
			yield return HandleMultipartSend(multipartData, headers, (bodyData) => Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Binary));
		}

	}
}
