using System;
using System.Collections.Generic;
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
		/// �񵿱������� HTTP GET ��û�� �����ϴ� ��ƿ��Ƽ �Լ�.
		/// UnityWebRequest�� ����ϸ� UniTask�� ������� ��û �����, ��� ����, ������ ���� ���� �� ���� ó�� ����� �����մϴ�.
		/// </summary>
		/// <typeparam name="T">���� �����͸� ��ȯ�� Ÿ��</typeparam>
		/// <param name="url">��û�� URL</param>
		/// <param name="onProgress">��û ������� ��ȯ�ϴ� �ݹ� (�ɼ�)</param>
		/// <param name="headers">��û ����� ������ ��ųʸ� (�ɼ�)</param>
		/// <param name="expectedType">
		/// ���信�� �����ϴ� ������ ���� (�⺻��: ContentTypeState.Unknown).
		/// **���:** `ContentTypeState.Unknown` ���·� �����ϸ� Content-Type�� �������� ������,
		/// ������ Content-Type ����� ���������� ��� ���ܰ� �߻��� �� �ֽ��ϴ�.
		/// �ùٸ� ContentTypeState ���� �����ϴ� ���� ����˴ϴ�.
		/// </param>
		/// <param name="audioType">����� �������� ����� AudioType (�⺻��: AudioType.UNKNOWN)</param>
		/// <param name="cancellationToken">��û�� ����ϱ� ���� CancellationToken (�ɼ�)</param>
		/// <returns>���� �����͸� ������ Ÿ������ ��ȯ�ϴ� UniTask</returns>
		public static async UniTask<T> Get<T>(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, ContentTypeState expectedType = ContentTypeState.Unknown, CancellationToken cancellationToken = default)
		{
			using (UnityWebRequest request = UnityWebRequest.Get(url))
			{
				// 1. ��� ����
				SetRequestHeaders(request, headers);

				// 2. �ٿ�ε� �ڵ鷯 ����
				request.downloadHandler = CreateDownloadHandler(url, expectedType);

				// 3. ���� ���¸� ����� �α׿� ���
				RestApiDebug.Request(request, headers);

				// 4. ��û ���� �� ���� ���� ����
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				await UpdateProgress(asyncOperation, onProgress, cancellationToken);

				//������ Ÿ�� ����
				string contentType = request.GetResponseHeader("Content-Type");

				// 5. ��û �� Content-Type Ȯ��
				if (!ValidateContentType(request, ref expectedType))
				{
					RestApiDebug.Result(request, ContentTypeState.Unknown);
					throw new Exception($"Unrecognized or missing Content-Type: {contentType}");
				}

				// 6. ��û ����� ����� �α׿� ���
				RestApiDebug.Result(request, expectedType);

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
				RestApiDebug.Request(request, headers);

				// 5. ��û ���� �� ���� ���� ����
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				await UpdateProgress(asyncOperation, onProgress, cancellationToken);

				string contentType = request.GetResponseHeader("Content-Type");

				// 5. ��û �� Content-Type Ȯ��
				if (!ValidateContentType(request, ref expectedType))
				{
					RestApiDebug.Result(request, ContentTypeState.Unknown);
					throw new Exception($"Unrecognized or missing Content-Type: {contentType}");
				}

				// 7. ��� ���
				RestApiDebug.Result(request, expectedType);

				// 7. ���� ó��
				if (request.result != UnityWebRequest.Result.Success)
					throw new Exception($"Api Request failed: {request.error}");

				// 8. ���� ó�� �� ��ȯ
				return await ParseResponse<T>(request, expectedType, contentType);
			}
		}
	}
}