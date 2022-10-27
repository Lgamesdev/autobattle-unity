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
    public class BaseCharacterItemConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IBaseCharacterItem).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            // Using a nullable bool here in case "isDefaultItem" is not present on an item
            bool? isDefaultItem = jo["item"]?.ToObject<Item>()?.isDefaultItem;

            IBaseCharacterItem characterItem;
            if (isDefaultItem.GetValueOrDefault())
            {                
                characterItem = new CharacterItem();
            }
            else
            {
                characterItem = new CharacterEquipment();
            }

            serializer.Converters.Add(new ItemConverter());
            serializer.Populate(jo.CreateReader(), characterItem);

            return characterItem;
        }
        
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
