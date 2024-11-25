using System;
using Newtonsoft.Json;

namespace Suk.Json
{

	public static class JsonParser
	{

		/// <summary>
		/// JSON 문자열을 지정된 타입으로 파싱하는 유틸리티 함수.
		/// </summary>
		/// <typeparam name="T">파싱할 데이터 타입</typeparam>
		/// <param name="json">JSON 문자열</param>
		/// <returns>파싱된 데이터</returns>
		/// <exception cref="ArgumentNullException">JSON 문자열이 null 또는 공백일 때 예외 발생</exception>
		/// <exception cref="JsonException">JSON 파싱 실패 시 예외 발생</exception>
		public static T Parse<T>(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
				throw new ArgumentNullException(nameof(json), "JSON 문자열이 null이거나 공백입니다.");
			try
			{
				return JsonConvert.DeserializeObject<T>(json);
			}
			catch (JsonException ex)
			{
				throw new JsonException($"JSON 파싱 실패: {ex.Message}", ex);
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