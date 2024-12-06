using System;
using Newtonsoft.Json;

namespace Suk.Json
{

	public static class JsonParser
	{

		/// <summary> JSON 문자열을 지정된 타입으로 파싱하는 유틸리티 함수.</summary>
		public static T Parse<T>(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
				throw new ArgumentNullException(nameof(json), "[JsonParser] Parse\nJSON 문자열이 null이거나 공백입니다.");

			try
			{
				return JsonConvert.DeserializeObject<T>(json);
			}
			catch (JsonException ex)
			{
				throw new JsonException($"[JsonParser] Serialize\nJSON 직렬화 실패: {ex.Message}", ex);
			}
		}


		public static string Serialize<T>(T obj)
		{
			return JsonConvert.SerializeObject(obj);
		}

		public static byte[] ToByteArray<T>(T obj)
		{
			string jsonString = Serialize(obj);
			return System.Text.Encoding.UTF8.GetBytes(jsonString);
		}
	}
}