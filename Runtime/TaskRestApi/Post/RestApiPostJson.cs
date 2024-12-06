using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;
using static Suk.RestApi.TaskRestApiUtility;

namespace Suk.RestApi
{
	internal static class TaskRestApiPostJson
	{

		/// <summary>JSON �����͸� POST ��û���� �����ϰ� JSON ������ ó���մϴ�.</summary>
		public static async UniTask<Res> PostJsonForJsonResponse<Req, Res>(string url, Req body, UnityAction<float> onProgress = null, Dictionary<string, string> headers = null, CancellationToken cancellationToken = default)
		{
			if (string.IsNullOrWhiteSpace(url))
				throw new ArgumentNullException(nameof(url), "[RestApiPostJson] PostJsonForJsonResponse\nURL�� null�̰ų� ��� �ֽ��ϴ�.");

			if (body == null)
				throw new ArgumentNullException(nameof(body), "[RestApiPostJson] PostJsonForJsonResponse\n��û �ٵ� null�Դϴ�.");

			try
			{
				headers = SetContentHeader(headers, "application/json");
				byte[] bodyData = HandleJsonBody(body);
				string jsonString = await Post<string>(url, bodyData, onProgress, headers, ContentTypeState.Text, cancellationToken);
				return HandleJsonResponse<Res>(jsonString);
			}
			catch (Exception ex)
			{
				throw new Exception($"[RestApiUtility] PostJsonForJsonResponse\n {ex.Message}", ex);
			}
		}


		public static Dictionary<string, string> SetContentHeader(Dictionary<string, string> headers, string contentType)
		{
			if (headers == null)
				headers = new Dictionary<string, string>();
			headers["Content-Type"] = contentType;
			return headers;
		}
	}
}