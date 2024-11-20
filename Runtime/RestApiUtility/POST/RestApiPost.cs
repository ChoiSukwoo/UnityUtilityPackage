using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using static Suk.RestApi.RestApiUtility;

namespace Suk.RestApi.Post {
	internal static class RestApiPost {

		public static IEnumerator Post<T>(
				string url,
				byte[] bodyData,
				UnityAction<ApiResponse<T>> onComplete,
				UnityAction<float> onProgress = null,
				Dictionary<string, string> headers = null,
				ContentTypeState expectedType = ContentTypeState.Unknown,
				AudioType audioType = AudioType.UNKNOWN) {
			using(UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST)) {
				// 1. ��� ����
				SetRequestHeaders(request, headers);

				// 2. �ٵ� �����͸� ���ε� �ڵ鷯�� �߰�
				request.uploadHandler = new UploadHandlerRaw(bodyData);

				// 3. �ٿ�ε� �ڵ鷯 ����
				request.downloadHandler = CreateDownloadHandler(url, expectedType, audioType);

				// 4. ���� ���¸� ����ڿ��� �˸�
				RestApiDebug.Request(request, headers);

				// 5. ��û ���� �� ���� ���� ����
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				// 6. ��û �� Content-Type Ȯ�� (expectedType�� Unknown�� ���)
				if(!ValidateContentType(request, ref expectedType)) {
					string contentType = request.GetResponseHeader("Content-Type");
					onComplete?.Invoke(new FailureResponse<T>($"Unrecognized or missing Content-Type: {contentType}"));
					yield break;
				}

				// 7. ��� ���
				RestApiDebug.Result(request, expectedType);

				// 8. ��û ���� ���� Ȯ��
				if(request.result != UnityWebRequest.Result.Success) {
					onComplete?.Invoke(new FailureResponse<T>(request.error));
					yield break;
				}

				// 9. ���� ó�� �� ����/���� ó��
				try {
					T response = ParseResponse<T>(request, expectedType);
					onComplete?.Invoke(new SuccessResponse<T>(response));
				} catch(System.Exception ex) {
					onComplete?.Invoke(new FailureResponse<T>($"Failed to parse response: {ex.Message}"));
				}
			}
		}

	}





}
