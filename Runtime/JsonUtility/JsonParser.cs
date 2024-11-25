using System;
using Newtonsoft.Json;

namespace Suk.Json
{

	public static class JsonParser
	{

		/// <summary>
		/// JSON ���ڿ��� ������ Ÿ������ �Ľ��ϴ� ��ƿ��Ƽ �Լ�.
		/// </summary>
		/// <typeparam name="T">�Ľ��� ������ Ÿ��</typeparam>
		/// <param name="json">JSON ���ڿ�</param>
		/// <returns>�Ľ̵� ������</returns>
		/// <exception cref="ArgumentNullException">JSON ���ڿ��� null �Ǵ� ������ �� ���� �߻�</exception>
		/// <exception cref="JsonException">JSON �Ľ� ���� �� ���� �߻�</exception>
		public static T Parse<T>(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
				throw new ArgumentNullException(nameof(json), "JSON ���ڿ��� null�̰ų� �����Դϴ�.");
			try
			{
				return JsonConvert.DeserializeObject<T>(json);
			}
			catch (JsonException ex)
			{
				throw new JsonException($"JSON �Ľ� ����: {ex.Message}", ex);
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