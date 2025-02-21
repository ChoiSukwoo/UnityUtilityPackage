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
		public static async UniTask<T> Get<T>(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, ContentTypeState expectedType = ContentTypeState.Unknown, CancellationToken cancellationToken = default)
		{
			using (UnityWebRequest request = UnityWebRequest.Get(url))
			{
				// 1. ��� ����
				SetRequestHeaders(request, headers);

				// 2. �ٿ�ε� �ڵ鷯 ����
				request.downloadHandler = CreateDownloadHandler(url, expectedType);

				// 3. ���� ���¸� ����� �α׿� ���
				Stopwatch stopwatch = Stopwatch.StartNew();
				RestApiDebug.Request(request, headers, stopwatch);

				// 4. ��û ���� �� ���� ���� ����
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				await UpdateProgress(asyncOperation, onProgress, cancellationToken);

				//������ Ÿ�� ����
				string contentType = request.GetResponseHeader("Content-Type");

				// 5. ��û �� Content-Type Ȯ��
				if (!ValidateContentType(request, ref expectedType))
				{
					RestApiDebug.Result(request, ContentTypeState.Unknown, stopwatch);
					throw new Exception($"Unrecognized or missing Content-Type: {contentType}");
				}

				// 6. ��û ����� ����� �α׿� ���
				RestApiDebug.Result(request, expectedType, stopwatch);

				// 7. ���� ó��
				if (request.result != UnityWebRequest.Result.Success)
					throw new Exception($"Api Request failed: {request.error}");

				// 8. ���� ó�� �� ��ȯ
				T result = await ParseResponse<T>(request, expectedType, contentType);

				return result;
			}
		}

		public static async UniTask<T> Post<T>(string url, byte[] bodyData, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, ContentTypeState expectedType = ContentTypeState.Unknown, CancellationToken cancellationToken = default)
		{

			using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
			{
				// 1. ��� ����
				SetRequestHeaders(request, headers);

				// 2. �ٵ� �����͸� ���ε� �ڵ鷯�� �߰�
				request.uploadHandler = new UploadHandlerRaw(bodyData);

				// 3. �ٿ�ε� �ڵ鷯 ����
				request.downloadHandler = CreateDownloadHandler(url, expectedType);

				// 4. ���� ���¸� ����ڿ��� �˸�
				Stopwatch stopwatch = Stopwatch.StartNew();
				RestApiDebug.Request(request, headers, stopwatch);

				// 5. ��û ���� �� ���� ���� ����
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				await UpdateProgress(asyncOperation, onProgress, cancellationToken);

				string contentType = request.GetResponseHeader("Content-Type");

				// 5. ��û �� Content-Type Ȯ��
				if (!ValidateContentType(request, ref expectedType))
				{
					RestApiDebug.Result(request, ContentTypeState.Unknown, stopwatch);
					throw new Exception($"Unrecognized or missing Content-Type: {contentType}");
				}

				// 7. ��� ���
				RestApiDebug.Result(request, expectedType, stopwatch);

				// 7. ���� ó��
				if (request.result != UnityWebRequest.Result.Success)
					throw new Exception($"Api Request failed: {request.error}");

				// 8. ���� ó�� �� ��ȯ
				return await ParseResponse<T>(request, expectedType, contentType);
			}
		}
	}
}