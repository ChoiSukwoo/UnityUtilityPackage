using System;
using System.Collections;
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

		public static IEnumerator Get<T>(string url, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, ContentTypeState expectedType = ContentTypeState.Unknown, AudioContentType audioContentType = AudioContentType.MP3)
		{
			using (UnityWebRequest request = UnityWebRequest.Get(url))
			{
				// 1. ��� ����
				SetRequestHeaders(request, headers);

				// 2. �ٿ�ε� �ڵ鷯 ����
				request.downloadHandler = CreateDownloadHandler(url, expectedType, audioContentType);

				// 3. ���� ���¸� ����ڿ��� �˸�
				RestApiDebug.Request(request, headers);

				// 4. ��û ���� �� ���� ���� ����
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				yield return UpdateProgress(asyncOperation, onProgress);

				// 5. ��û �� Content-Type Ȯ�� (expectedType�� Unknown�� ���)
				if (!ValidateContentType(request, ref expectedType))
				{
					string contentType = request.GetResponseHeader("Content-Type");
					onComplete?.Invoke(new FailureResponse<T>($"Unrecognized or missing Content-Type: {contentType}"));
					yield break;
				}

				// 6. ��� ���
				RestApiDebug.Result(request, expectedType);

				// 7. ��û ���� ���� Ȯ��
				if (request.result != UnityWebRequest.Result.Success)
				{
					onComplete?.Invoke(new FailureResponse<T>(request.error));
					yield break;
				}

				// 8. ���� ó�� �� ����/���� ó��
				try
				{
					T response = ParseResponse<T>(request, expectedType);
					onComplete?.Invoke(new SuccessResponse<T>(response));
				}
				catch (System.Exception ex)
				{
					onComplete?.Invoke(new FailureResponse<T>($"Failed to parse response: {ex.Message}"));
				}
			}
		}


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
		public static async UniTask<T> Get<T>(string url, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, ContentTypeState expectedType = ContentTypeState.Unknown, CancellationToken cancellationToken = default, AudioContentType audioContentType = AudioContentType.MP3)
		{
			using (UnityWebRequest request = UnityWebRequest.Get(url))
			{
				// 1. ��� ����
				SetRequestHeaders(request, headers);

				// 2. �ٿ�ε� �ڵ鷯 ����
				request.downloadHandler = CreateDownloadHandler(url, expectedType, audioContentType);

				// 3. ���� ���¸� ����� �α׿� ���
				RestApiDebug.Request(request, headers);

				// 4. ��û ���� �� ���� ���� ����
				UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
				await UpdateProgress(asyncOperation, onProgress, cancellationToken);

				// 5. ��û �� Content-Type Ȯ��
				if (!ValidateContentType(request, ref expectedType))
				{
					string contentType = request.GetResponseHeader("Content-Type");
					throw new Exception($"Unrecognized or missing Content-Type: {contentType}");
				}

				// 6. ��û ��� Ȯ��
				RestApiDebug.Result(request, expectedType);

				// 7. ���� ó��
				if (request.result != UnityWebRequest.Result.Success)
					throw new Exception($"Request failed: {request.error}");

				// 8. ���� ó�� �� ��ȯ
				return ParseResponse<T>(request, expectedType);
			}
		}

		public static IEnumerator Post<T>(string url, byte[] bodyData, UnityAction<ApiResponse<T>> onComplete, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, ContentTypeState expectedType = ContentTypeState.Unknown, AudioContentType audioType = AudioContentType.MP3)
		{
			using (UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST))
			{
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
				if (!ValidateContentType(request, ref expectedType))
				{
					string contentType = request.GetResponseHeader("Content-Type");
					onComplete?.Invoke(new FailureResponse<T>($"Unrecognized or missing Content-Type: {contentType}"));
					yield break;
				}

				// 7. ��� ���
				RestApiDebug.Result(request, expectedType);

				// 8. ��û ���� ���� Ȯ��
				if (request.result != UnityWebRequest.Result.Success)
				{
					onComplete?.Invoke(new FailureResponse<T>(request.error));
					yield break;
				}

				// 9. ���� ó�� �� ����/���� ó��
				try
				{
					T response = ParseResponse<T>(request, expectedType);
					onComplete?.Invoke(new SuccessResponse<T>(response));
				}
				catch (System.Exception ex)
				{
					onComplete?.Invoke(new FailureResponse<T>($"Failed to parse response: {ex.Message}"));
				}
			}
		}

	}
}