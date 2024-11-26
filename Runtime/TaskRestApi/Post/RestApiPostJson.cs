using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Suk.Json;
using UnityEngine.Events;
using static Suk.RestApi.RestApiBase;

namespace Suk.RestApi
{
	internal static class TaskRestApiPostJson
	{

		/// <summary>JSON 데이터를 POST 요청으로 전송하고 JSON 응답을 처리합니다.</summary>
		public static async UniTask<Res> PostJsonForJsonResponse<Req, Res>(
			string url,
			Req body,
			UnityAction<float> onProgress = null,
			Dictionary<string, string> headers = null,
			CancellationToken cancellationToken = default
		)
		{
			headers = SetContentHeader(headers, "application/json");
			byte[] bodyData = HandleJsonSendAs(body);
			string jsonString = await Post<string>(url, bodyData, onProgress, headers, ContentTypeState.Text, cancellationToken);
			return JsonParser.Parse<Res>(jsonString);
		}


		public static byte[] HandleJsonSendAs<T>(T body)
		{
			byte[] bodyData = JsonParser.ToByteArray(body);
			return bodyData;
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