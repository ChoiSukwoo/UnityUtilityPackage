using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Suk.Utility.Json
{
	public class Vector3Converter : JsonConverter<Vector3>
	{

		// Json -> Vector3
		public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			JObject jo = JObject.Load(reader);

			float x = (float)jo["x"];
			float y = (float)jo["y"];
			float z = (float)jo["z"];

			return new Vector3(x, y, z);
		}

		// Vector3 -> Json
		public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
		{
			writer.WriteStartObject();

			writer.WritePropertyName("x");
			writer.WriteValue(value.x);
			writer.WritePropertyName("y");
			writer.WriteValue(value.y);
			writer.WritePropertyName("z");
			writer.WriteValue(value.z);

			writer.WriteEndObject();
		}
	}

}
