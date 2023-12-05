using System;
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

            ItemType? itemType = jo["item"]?.ToObject<Item>()?.itemType;
            //ItemType? itemType = jo.ToObject<Item>()?.itemType;
            //Debug.Log("jo : " + jo);
            //Debug.Log("itemType : " + itemType + " \n generic item types : " + ItemType.Item + "\n" + ItemType.LootBox + "\n" + ItemType.Equipment);
            
            IBaseCharacterItem characterItem = itemType switch
            {
                ItemType.Item => new CharacterItem(),
                ItemType.LootBox => new CharacterLootBox(),
                ItemType.Equipment => new CharacterEquipment(),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            //Debug.Log("characterItem : " + characterItem);
            
            /*if (characterItem == null)
            {
                Debug.LogError("Deserialization of characterItem is null, set to CharacterItem class by default.");
            }*/

            serializer.Converters.Add(new ItemConverter());
            serializer.Populate(jo.CreateReader(), characterItem ?? new CharacterItem());
            

            return characterItem;
        }
        
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
