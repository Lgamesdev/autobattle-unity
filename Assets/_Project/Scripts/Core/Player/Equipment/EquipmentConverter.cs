using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class EquipmentConverter : JsonConverter<Equipment>
    {
        public override void WriteJson(JsonWriter writer, Equipment value, JsonSerializer serializer)
        {
            var jo = new JObject();
            var type = value.GetType();
            jo.Add("type", type.Name);

            foreach (var field in value.GetType()
                         .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var fieldValue = field.GetValue(value);
                if (fieldValue != null)
                {
                    if (field.FieldType == typeof(Sprite))
                    {
                        var sprite = (Sprite)fieldValue;
                        //var spriteData = SpriteData.FromSprite(sprite);
                        //jo.Add(field.Name, JToken.FromObject(spriteData));
                    }
                    else if (field.FieldType == typeof(Stat[]))
                    {
                        var stats = (Stat[])fieldValue;
                        jo.Add(field.Name, JToken.FromObject(stats));
                    }
                    else
                    {
                        jo.Add(field.Name, JToken.FromObject(fieldValue));
                    }
                }
            }

            jo.WriteTo(writer);
        }

        public override Equipment ReadJson(JsonReader reader, Type objectType, Equipment existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            Debug.Log(existingValue);

            //var spriteData = serializer.Deserialize<SpriteData>(reader);
            //var sprite = SpriteData.ToSprite(spriteData);


            //throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
            return existingValue;
        }
    }
}