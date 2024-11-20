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
				// 1. 헤더 설정
				SetRequestHeaders(request, headers);

				// 2. 바디 데이터를 업로드 핸들러에 추가
				request.uploadHandler = new UploadHandlerRaw(bodyData);

				// 3. 다운로드 핸들러 설정
				request.downloadHandler = CreateDownloadHandler(url, expectedType, audioType);

				// 4. 전송 상태를 사용자에게 알림
				RestApiDebug.Request(request, headers);

				// 5. 요청 시작 및 전송 상태 추적
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				// 6. 요청 후 Content-Type 확인 (expectedType이 Unknown일 경우)
				if(!ValidateContentType(request, ref expectedType)) {
					string contentType = request.GetResponseHeader("Content-Type");
					onComplete?.Invoke(new FailureResponse<T>($"Unrecognized or missing Content-Type: {contentType}"));
					yield break;
				}

				// 7. 결과 출력
				RestApiDebug.Result(request, expectedType);

				// 8. 요청 성공 여부 확인
				if(request.result != UnityWebRequest.Result.Success) {
					onComplete?.Invoke(new FailureResponse<T>(request.error));
					yield break;
				}

				// 9. 응답 처리 및 성공/실패 처리
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
