using System;
using System.Collections.Generic;
using System.Linq;

namespace Suk
{
	public static class UriUtility
	{
		/// <summary>URL을 파싱하여 Uri 객체를 반환합니다.</summary>
		/// <returns>Uri 객체</returns>
		public static Uri ParseUri(string url)
		{
			if (Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult))
			{
				return uriResult;
			}
			else
			{
				throw new UriFormatException("유효하지 않은 URL 형식입니다.");
			}
		}

		/// <summary>URL의 쿼리 매개변수를 딕셔너리로 반환합니다.</summary>
		/// <returns>쿼리 매개변수를 담은 딕셔너리</returns>
		public static Dictionary<string, string> GetQueryParameters(string url)
		{
			Uri uri = ParseUri(url);
			var queryParameters = new Dictionary<string, string>();

			string query = uri.Query;
			if (string.IsNullOrEmpty(query))
				return queryParameters;

			// '?' 문자 제거
			query = query.TrimStart('?');

			var pairs = query.Split('&');
			foreach (var pair in pairs)
			{
				var keyValue = pair.Split('=');
				if (keyValue.Length == 2)
				{
					string key = Decode(keyValue[0]);
					string value = Decode(keyValue[1]);
					queryParameters[key] = value;
				}
			}

			return queryParameters;
		}

		/// <summary>특정 쿼리 매개변수의 값을 가져옵니다.</summary>
		/// <returns>매개변수의 값, 존재하지 않으면 Empty</returns>
		public static string GetQueryParameter(string url, string parameter)
		{
			var parameters = GetQueryParameters(url);
			if (parameters.TryGetValue(parameter, out string value))
				return value;
			return string.Empty;
		}

		/// <summary>딕셔너리를 기반으로 URL을 생성합니다.</summary>
		/// <returns>생성된 URL</returns>
		public static string BuildUrl(string baseUrl, Dictionary<string, string> parameters)
		{
			if (string.IsNullOrEmpty(baseUrl))
				throw new ArgumentException("baseUrl은 null 또는 빈 문자열일 수 없습니다.", nameof(baseUrl));

			UriBuilder uriBuilder = new UriBuilder(baseUrl);

			// 쿼리 문자열을 생성합니다.
			var queryParameters = new List<string>();
			foreach (var param in parameters)
			{
				string encodedKey = Encode(param.Key);
				string encodedValue = Encode(param.Value);
				queryParameters.Add($"{encodedKey}={encodedValue}");
			}

			uriBuilder.Query = string.Join("&", queryParameters);
			return uriBuilder.ToString();
		}

		/// <summary>딕셔너리를 기반으로 URL을 생성합니다.</summary>
		/// <returns>생성된 URL</returns>
		public static string BuildUrl(string[] segments, Dictionary<string, string> parameters = null)
		{
			// 파라미터 검증
			if (segments == null || segments.Length == 0)
				throw new ArgumentException("segments는 최소 하나의 값이 필요합니다.", nameof(segments));

			// 첫 번째 세그먼트를 baseUrl로 설정
			string baseUrl = segments[0].Trim();
			if (string.IsNullOrEmpty(baseUrl))
				throw new ArgumentException("첫 번째 세그먼트(baseUrl)는 null 또는 빈 문자열일 수 없습니다.", nameof(segments));

			UriBuilder uriBuilder = new UriBuilder(baseUrl);

			// 나머지 세그먼트들을 경로로 추가
			string combinedPath = uriBuilder.Path.TrimEnd('/');
			for (int i = 1; i < segments.Length; i++)
			{
				string segment = segments[i].Trim();
				if (!string.IsNullOrEmpty(segment))
					combinedPath += "/" + Uri.EscapeDataString(segment.Trim('/'));
			}

			uriBuilder.Path = combinedPath;

			// 파라미터 추가
			if (parameters != null && parameters.Count > 0)
			{
				var query = string.Join("&", parameters.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
				uriBuilder.Query = query;
			}

			return uriBuilder.Uri.ToString();
		}


		public static string BuildUrl(params string[] segments)
		{
			// 파라미터 검증
			if (segments == null || segments.Length == 0)
				throw new ArgumentException("segments는 최소 하나의 값이 필요합니다.", nameof(segments));

			// 첫 번째 세그먼트를 baseUrl로 설정
			string baseUrl = segments[0].Trim();
			if (string.IsNullOrEmpty(baseUrl))
				throw new ArgumentException("첫 번째 세그먼트(baseUrl)는 null 또는 빈 문자열일 수 없습니다.", nameof(segments));

			UriBuilder uriBuilder = new UriBuilder(baseUrl);

			// 나머지 세그먼트를 경로로 추가
			string combinedPath = uriBuilder.Path.TrimEnd('/');
			for (int i = 1; i < segments.Length; i++)
			{
				string segment = segments[i].Trim();
				if (!string.IsNullOrEmpty(segment))
					combinedPath += "/" + Uri.EscapeDataString(segment.Trim('/'));
			}

			uriBuilder.Path = combinedPath;
			return uriBuilder.Uri.ToString();
		}

		/// <summary> URL을 인코딩합니다. (특수문자=>%16진수 변환) </summary>
		/// <returns>인코딩된 문자열</returns>
		public static string Encode(string input)
		{
			return Uri.EscapeDataString(input);
		}

		/// <summary>URL을 디코딩합니다. (%16진수=>특수문자 변환)
		/// <returns>디코딩된 문자열</returns>
		public static string Decode(string input)
		{
			return Uri.UnescapeDataString(input);
		}

		/// <summary>URL의 유효성을 검사합니다. </summary>
		/// <returns>유효한 URL이면 true, 그렇지 않으면 false</returns>
		public static bool Validate(string url)
		{
			return Uri.TryCreate(url, UriKind.Absolute, out _);
		}
	}
}