using System;
using System.Collections.Generic;

public static class UriUtility {
	/// <summary>URL�� �Ľ��Ͽ� Uri ��ü�� ��ȯ�մϴ�.</summary>
	/// <returns>Uri ��ü</returns>
	public static Uri ParseUri(string url) {
		if(Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)) {
			return uriResult;
		} else {
			throw new UriFormatException("��ȿ���� ���� URL �����Դϴ�.");
		}
	}

	/// <summary>URL�� ���� �Ű������� ��ųʸ��� ��ȯ�մϴ�.</summary>
	/// <returns>���� �Ű������� ���� ��ųʸ�</returns>
	public static Dictionary<string, string> GetQueryParameters(string url) {
		Uri uri = ParseUri(url);
		var queryParameters = new Dictionary<string, string>();

		string query = uri.Query;
		if(string.IsNullOrEmpty(query))
			return queryParameters;

		// '?' ���� ����
		query = query.TrimStart('?');

		var pairs = query.Split('&');
		foreach(var pair in pairs) {
			var keyValue = pair.Split('=');
			if(keyValue.Length == 2) {
				string key = Decode(keyValue[0]);
				string value = Decode(keyValue[1]);
				queryParameters[key] = value;
			}
		}

		return queryParameters;
	}

	/// <summary>Ư�� ���� �Ű������� ���� �����ɴϴ�.</summary>
	/// <returns>�Ű������� ��, �������� ������ Empty</returns>
	public static string GetQueryParameter(string url, string parameter) {
		var parameters = GetQueryParameters(url);
		if(parameters.TryGetValue(parameter, out string value))
			return value;
		return string.Empty;
	}

	/// <summary>��ųʸ��� ������� URL�� �����մϴ�.</summary>
	/// <returns>������ URL</returns>
	public static string BuildUrl(string baseUrl, Dictionary<string, string> parameters) {
		if(string.IsNullOrEmpty(baseUrl))
			throw new ArgumentException("baseUrl�� null �Ǵ� �� ���ڿ��� �� �����ϴ�.", nameof(baseUrl));

		UriBuilder uriBuilder = new UriBuilder(baseUrl);

		// ���� ���ڿ��� �����մϴ�.
		var queryParameters = new List<string>();
		foreach(var param in parameters) {
			string encodedKey = Encode(param.Key);
			string encodedValue = Encode(param.Value);
			queryParameters.Add($"{encodedKey}={encodedValue}");
		}

		uriBuilder.Query = string.Join("&", queryParameters);
		return uriBuilder.ToString();
	}

	/// <summary>��ųʸ��� ������� URL�� �����մϴ�.</summary>
	/// <returns>������ URL</returns>
	public static string BuildUrl(string baseUrl, List<string> pathSegments, Dictionary<string, string> parameters = null) {
		// �⺻ URL�� ��������Ʈ�� �����Ͽ� ��θ� �����մϴ�.
		if(string.IsNullOrEmpty(baseUrl))
			throw new ArgumentException("baseUrl�� null �Ǵ� �� ���ڿ��� �� �����ϴ�.", nameof(baseUrl));

		UriBuilder uriBuilder = new UriBuilder(baseUrl);

		// ���� ��ο� ��������Ʈ�� ��Ĩ�ϴ�.
		string combinedPath = uriBuilder.Path.TrimEnd('/');

		if(pathSegments != null && pathSegments.Count > 0) {
			foreach(var endpoint in pathSegments) {
				if(!string.IsNullOrEmpty(endpoint)) {
					combinedPath += "/" + Uri.EscapeDataString(endpoint.Trim('/'));
				}
			}
		}

		uriBuilder.Path = combinedPath;
		string url = uriBuilder.Uri.ToString();
		return BuildUrl(url, parameters);
	}

	/// <summary> URL�� ���ڵ��մϴ�. (Ư������=>%16���� ��ȯ) </summary>
	/// <returns>���ڵ��� ���ڿ�</returns>
	public static string Encode(string input) {
		return Uri.EscapeDataString(input);
	}

	/// <summary>URL�� ���ڵ��մϴ�. (%16����=>Ư������ ��ȯ)
	/// <returns>���ڵ��� ���ڿ�</returns>
	public static string Decode(string input) {
		return Uri.UnescapeDataString(input);
	}

	/// <summary>URL�� ��ȿ���� �˻��մϴ�. </summary>
	/// <returns>��ȿ�� URL�̸� true, �׷��� ������ false</returns>
	public static bool Validate(string url) {
		return Uri.TryCreate(url, UriKind.Absolute, out _);
	}
}