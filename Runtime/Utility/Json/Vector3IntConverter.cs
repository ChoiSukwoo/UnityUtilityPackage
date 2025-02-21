using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Suk.Utility.Json
{
	public class Vector3IntConverter : JsonConverter<Vector3Int>
	{
		int ToConvert(float value) { return Mathf.RoundToInt(value); }

		// Json -> Vector3Int
		public override Vector3Int ReadJson(JsonReader reader, Type objectType, Vector3Int existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject jo = JObject.Load(reader);
			
			int x = ToConvert((float)jo["x"]);
			int y = ToConvert((float)jo["y"]);
			int z = ToConvert((float)jo["z"]);
			
			return new Vector3Int(x,y,z);
		}

		// Vector3Int -> Json
		public override void WriteJson(JsonWriter writer, Vector3Int value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("x");
			writer.WriteValue(ToConvert(value.x));
			writer.WritePropertyName("y");
			writer.WriteValue(ToConvert(value.y));
			writer.WritePropertyName("z");
			writer.WriteValue(ToConvert(value.z));
			writer.WriteEndObject();
		}
	}
}
