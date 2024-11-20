using Newtonsoft.Json;
using System;

namespace Suk.Json {

	public static class JsonParser {
		public static T Parse<T>(string json) {
			if(string.IsNullOrWhiteSpace(json))
				return Activator.CreateInstance<T>();
			return JsonConvert.DeserializeObject<T>(json);
		}

		public static string Serialize<T>(T obj) {
			return JsonConvert.SerializeObject(obj);
		}

		public static byte[] ToByteArray<T>(T obj) {
			string jsonString = Serialize(obj);
			return System.Text.Encoding.UTF8.GetBytes(jsonString);
		}
	}
}