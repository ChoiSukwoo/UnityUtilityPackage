using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

// Bounds 커스텀 컨버터
public class BoundsConverter : JsonConverter<Bounds>
{
    // Json -> Bounds
    public override Bounds ReadJson(JsonReader reader, System.Type objectType, Bounds existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jo = JObject.Load(reader);

        // center
        var centerToken = jo["center"];
        if (centerToken == null)
            return default;
        
        float cx = (float)centerToken["x"];
        float cy = (float)centerToken["y"];
        float cz = (float)centerToken["z"];
        Vector3 center = new Vector3(cx, cy, cz);

        // size
        var sizeToken = jo["size"];
        float sx = (float)sizeToken["x"];
        float sy = (float)sizeToken["y"];
        float sz = (float)sizeToken["z"];
        Vector3 size = new Vector3(sx, sy, sz);
        return new Bounds(center, size);
    }

    // Bounds -> Json
    public override void WriteJson(JsonWriter writer, Bounds value, JsonSerializer serializer)
    {
        writer.WriteStartObject();

        writer.WritePropertyName("center");
        // Vector3 직렬화 -> Vector3Converter 직접 호출하는 대신, JsonSerializer를 이용할 수도 있습니다.
        writer.WriteStartObject();
        writer.WritePropertyName("x"); writer.WriteValue(Mathf.RoundToInt(value.center.x * 100f) / 100f);
        writer.WritePropertyName("y"); writer.WriteValue(Mathf.RoundToInt(value.center.y * 100f) / 100f);
        writer.WritePropertyName("z"); writer.WriteValue(Mathf.RoundToInt(value.center.z * 100f) / 100f);
        writer.WriteEndObject();

        writer.WritePropertyName("size");
        writer.WriteStartObject();
        writer.WritePropertyName("x"); writer.WriteValue(Mathf.RoundToInt(value.size.x * 100f) / 100f);
        writer.WritePropertyName("y"); writer.WriteValue(Mathf.RoundToInt(value.size.y * 100f) / 100f);
        writer.WritePropertyName("z"); writer.WriteValue(Mathf.RoundToInt(value.size.z * 100f) / 100f);
        writer.WriteEndObject();

        writer.WriteEndObject();
    }
}