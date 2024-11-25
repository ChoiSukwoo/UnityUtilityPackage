using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi
{
	internal static class RestApiPostImage
	{

		/// <summary>��û ���� ���� POST ��û�� ������ JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostNoneForImageResponse<T>(string url, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleNoneSend(headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}

		/// <summary>�ؽ�Ʈ �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostTextForImageResponse<T>(string url, string text, string contentType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleTextSend(text, contentType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}

		/// <summary>���̳ʸ� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostBinaryForImageResponse<T>(string url, byte[] binaryData, string contentType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleBinarySend(binaryData, contentType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}

		/// <summary>JSON �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostJsonForImageResponse<TSend, T>(string url, TSend body, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleJsonSend(body, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}

		/// <summary>����� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostAudioForImageResponse<T>(string url, byte[] audioData, AudioContentType audioType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleAudioSend(audioData, audioType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image, audioType));
		}

		/// <summary>���� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostVideoForImageResponse<T>(string url, byte[] videoData, VideoContentType videoType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleVideoSend(videoData, videoType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}

		/// <summary>�̹��� �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostImageForImageResponse<T>(string url, byte[] imageData, ImageContentType imageType, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleImageSend(imageData, imageType, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}

		/// <summary>��Ƽ��Ʈ �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static IEnumerator PostMultipartForImageResponse<T>(string url, MultipartFormData multipartData, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null)
		{
			yield return HandleMultipartSend(multipartData, headers, (bodyData) =>
					Post(url, bodyData, onComplete, onProgress, headers, ContentTypeState.Image));
		}
	}
}
