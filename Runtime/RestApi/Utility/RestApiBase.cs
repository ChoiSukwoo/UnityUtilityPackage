using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.Networking;
using static Suk.RestApi.RestApiUtility;


namespace Suk.RestApi
{
	internal static class RestApiBase
	{
		/// <summary>
		/// 비동기적으로 HTTP GET 요청을 수행하는 유틸리티 함수.
		/// UnityWebRequest를 사용하며 UniTask를 기반으로 요청 진행률, 헤더 설정, 콘텐츠 유형 검증 및 응답 처리 기능을 제공합니다.
		/// </summary>
		/// <typeparam name="T">응답 데이터를 변환할 타입</typeparam>
		/// <param name="url">요청할 URL</param>
		/// <param name="onProgress">요청 진행률을 반환하는 콜백 (옵션)</param>
		/// <param name="headers">요청 헤더를 설정할 딕셔너리 (옵션)</param>
		/// <param name="expectedType">
		/// 응답에서 예상하는 콘텐츠 유형 (기본값: ContentTypeState.Unknown).
		/// **경고:** `ContentTypeState.Unknown` 상태로 실행하면 Content-Type이 검증되지 않으며,
		/// 서버의 Content-Type 헤더가 비정상적인 경우 예외가 발생할 수 있습니다.
		/// 올바른 ContentTypeState 값을 지정하는 것이 권장됩니다.
		/// </param>
		/// <param name="audioType">오디오 데이터인 경우의 AudioType (기본값: AudioType.UNKNOWN)</param>
		/// <param name="cancellationToken">요청을 취소하기 위한 CancellationToken (옵션)</param>
		/// <returns>응답 데이터를 지정된 타입으로 반환하는 UniTask</returns>
		public static async UniTask<T> Get<T>(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, ContentTypeState expectedType = ContentTypeState.Unknown, CancellationToken cancellationToken = default)
		{
			using (UnityWebRequest request = UnityWebRequest.Get(url))
			{
				// 1. 헤더 설정
				SetRequestHeaders(request, headers);

				// 2. 다운로드 핸들러 설정
				request.downloadHandler = CreateDownloadHandler(url, expectedType);

				// 3. 전송 상태를 디버깅 로그에 출력
				Stopwatch stopwatch = Stopwatch.StartNew();
				RestApiDebug.Request(request, headers, stopwatch);

				// 4. 요청 시작 및 전송 상태 추적
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				await UpdateProgress(asyncOperation, onProgress, cancellationToken);

				//콘텐츠 타입 추출
				string contentType = request.GetResponseHeader("Content-Type");

				// 5. 요청 후 Content-Type 확인
				if (!ValidateContentType(request, ref expectedType))
				{
					RestApiDebug.Result(request, ContentTypeState.Unknown, stopwatch);
					throw new Exception($"Unrecognized or missing Content-Type: {contentType}");
				}

				// 6. 요청 결과를 디버깅 로그에 출력
				RestApiDebug.Result(request, expectedType, stopwatch);

				// 7. 실패 처리
				if (request.result != UnityWebRequest.Result.Success)
					throw new Exception($"Api Request failed: {request.error}");

				// 8. 응답 처리 및 반환
				T result = await ParseResponse<T>(request, expectedType, contentType);

				return result;
			}
		}

		public static async UniTask<T> Post<T>(string url, byte[] bodyData, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, ContentTypeState expectedType = ContentTypeState.Unknown, CancellationToken cancellationToken = default)
		{

			using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
			{
				// 1. 헤더 설정
				SetRequestHeaders(request, headers);

				// 2. 바디 데이터를 업로드 핸들러에 추가
				request.uploadHandler = new UploadHandlerRaw(bodyData);

				// 3. 다운로드 핸들러 설정
				request.downloadHandler = CreateDownloadHandler(url, expectedType);

				// 4. 전송 상태를 사용자에게 알림
				Stopwatch stopwatch = Stopwatch.StartNew();
				RestApiDebug.Request(request, headers, stopwatch);

				// 5. 요청 시작 및 전송 상태 추적
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				await UpdateProgress(asyncOperation, onProgress, cancellationToken);

				string contentType = request.GetResponseHeader("Content-Type");

				// 5. 요청 후 Content-Type 확인
				if (!ValidateContentType(request, ref expectedType))
				{
					RestApiDebug.Result(request, ContentTypeState.Unknown, stopwatch);
					throw new Exception($"Unrecognized or missing Content-Type: {contentType}");
				}

				// 7. 결과 출력
				RestApiDebug.Result(request, expectedType, stopwatch);

				// 7. 실패 처리
				if (request.result != UnityWebRequest.Result.Success)
					throw new Exception($"Api Request failed: {request.error}");

				// 8. 응답 처리 및 반환
				return await ParseResponse<T>(request, expectedType, contentType);
			}
		}
	}
}