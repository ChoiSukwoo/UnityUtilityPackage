using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Vector3Converter : JsonConverter<Vector3>
{
	// Json -> Vector3
	public override Vector3 ReadJson(JsonReader reader, System.Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		JObject jo = JObject.Load(reader);
		float x = Mathf.RoundToInt((float)jo["x"] * 100f) / 100f; // 0.01 단위 반올림
		float y = Mathf.RoundToInt((float)jo["y"] * 100f) / 100f; // 0.01 단위 반올림
		float z = Mathf.RoundToInt((float)jo["z"] * 100f) / 100f; // 0.01 단위 반올림
		return new Vector3(x, y, z);
	}

	// Vector3 -> Json
	public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("x");
		writer.WriteValue(Mathf.RoundToInt(value.x * 100f) / 100f); // 0.01 단위 반올림
		writer.WritePropertyName("y");
		writer.WriteValue(Mathf.RoundToInt(value.y * 100f) / 100f); // 0.01 단위 반올림
		writer.WritePropertyName("z");
		writer.WriteValue(Mathf.RoundToInt(value.z * 100f) / 100f); // 0.01 단위 반올림
		writer.WriteEndObject();
	}
}
