using System;
using System.Collections.Generic;
using System.Linq;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace LGamesDev.Request.Converters
{
    public class FighterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IFighter).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            // Using a nullable int here in case "experience" is not present on the fighter
            int? experience = jo["experience"]?.ToObject<int>();

            IFighter fighter;
            if (experience.HasValue)
            {                
                fighter = new Character();
            }
            else
            {
                fighter = new Hero();
            }

            //serializer.Converters.Add(new ItemConverter());
            serializer.Populate(jo.CreateReader(), fighter);

            return fighter;
        }
        
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
