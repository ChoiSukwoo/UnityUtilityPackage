using System;
using Newtonsoft.Json;

namespace Suk.Utility.Json
{
	public static class JsonParser
	{
		/// <summary> JSON 문자열을 객체로 변환하는 메서드입니다. </summary>
		public static T Parsing<T>(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
				throw new ArgumentNullException(nameof(json), "[JsonParser]\nJSON 문자열이 null이거나 비어 있습니다.");
			try {
				return JsonConvert.DeserializeObject<T>(json);
			} catch (JsonException ex) {
				throw new JsonException($"\n[JsonParser] JSON 파싱 오류\n======JSON======\n{json}\n======Error Message======\n{ex.Message}\n======Error InnerException======\n{ex.InnerException}");
			}
		}

		/// <summary> 객체를 JSON 문자열로 직렬화하는 메서드입니다. </summary>
		public static string Serialize<T>(T obj)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj), "[JsonParser]\n직렬화할 객체가 null입니다.");
			try {
				return JsonConvert.SerializeObject(obj, Formatting.Indented);
			} catch (Exception ex) {
				throw new JsonException($"\n[JsonParser] JSON 직렬화 오류\n======Object======\n{obj}\n======Error Message======\n{ex.Message}\n======Error InnerException======\n{ex.InnerException}");
			}
		}

		/// <summary> 바이트 배열을 문자열로 변환하여 JSON 객체로 파싱하는 메서드입니다. </summary>
		public static T ParsingToByteArray<T>(byte[] byteArray, System.Text.Encoding encoding = null)
		{
			encoding ??= System.Text.Encoding.UTF8; // 기본값은 UTF-8
			string jsonString = encoding.GetString(byteArray);
			return Parsing<T>(jsonString);
		}

		/// <summary> 객체를 바이트 배열로 변환하는 메서드입니다. UTF-8 인코딩을 사용합니다. </summary>
		public static byte[] SerializeToByteArray<T>(T obj, System.Text.Encoding encoding = null)
		{
			encoding ??= System.Text.Encoding.UTF8; // 기본값은 UTF-8
			string jsonString = Serialize(obj);
			return encoding.GetBytes(jsonString);
		}
		
		
	}
}
