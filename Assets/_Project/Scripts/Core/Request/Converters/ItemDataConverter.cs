using System;
using System.Collections.Generic;
using System.Linq;
using LGamesDev.Core.Player;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace LGamesDev.Request.Converters
{
    public class ItemDataConverter : JsonConverter
    {
        //private string _rootPath = "Assets/_Project/";

        public override bool CanConvert(Type objectType)
        {
            return typeof(ItemData).IsAssignableFrom(objectType);
        }
        
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) {
                return null;
            }

            JObject jo = JObject.Load(reader);
            
            /*Item item = Create(objectType, jsonObject);*/

            ItemType? itemType = jo["itemType"]?.ToObject<ItemType>();
            ItemData item = itemType switch
            {
                ItemType.Item => new ItemData(),
                ItemType.LootBox => new LootBoxData(),
                ItemType.Equipment => new EquipmentData(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            /*if (item == null)
            {
                Debug.LogError("Deserialization of item is null, set to Item class by default.");
            }*/
            
            serializer.Populate(jo.CreateReader(), item ?? new ItemData());

            if (item.iconPath == null) 
                return item;

            Sprite sprite = Resources.Load<Sprite>(item.iconPath);//AssetDatabase.LoadAssetAtPath<Sprite>(_rootPath + iconPath + ".png");
            
            if (sprite == null) 
                return item;
            
            if (item != null) 
                item.icon = sprite;

            return item;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            /*Item item = (Item)value;

            JToken t = JToken.FromObject(item);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                o["type"] = item.GetType() == typeof(Equipment) ? "equipment" : "item";

                o.WriteTo(writer);
            }*/
            throw new NotImplementedException();
        }

        /*private Item Create(Type objectType, JObject jObject)
        {
            return FieldExists("spriteId", jObject) ? new Equipment() : new Item();
        }*/

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }
}
