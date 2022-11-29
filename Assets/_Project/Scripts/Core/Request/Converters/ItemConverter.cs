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
    public class ItemConverter : JsonConverter
    {
        //private string _rootPath = "Assets/_Project/";

        public override bool CanConvert(Type objectType)
        {
            return typeof(Item).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Item item = (Item)value;
            
            JToken t = JToken.FromObject(item);

            if (t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JObject o = (JObject)t;
                o["type"] = item.GetType() == typeof(Equipment) ? "equipment" : "item";
                //o["iconPath"] = AssetDatabase.GetAssetPath(item.icon);

                /*if (item.GetType() == typeof(Equipment))
                {
                    o["equipmentSlot"] = ((Equipment)item).equipmentSlot.ToString();
                }*/
                
                /*IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

                o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));*/

                o.WriteTo(writer);
            }
            
            /*writer.WritePropertyName("iconPath");
            writer.WriteValue(AssetDatabase.GetAssetPath(item.icon));*/
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) {
                return null;
            }

            JObject jsonObject = JObject.Load(reader);

            Item item = Create(objectType, jsonObject);

            serializer.Populate(jsonObject.CreateReader(), item);

            string iconPath = jsonObject["iconPath"]?.ToString();

            if (iconPath == null) 
                return item;

            Sprite sprite = Resources.Load<Sprite>(iconPath);//AssetDatabase.LoadAssetAtPath<Sprite>(_rootPath + iconPath + ".png");
            if (sprite != null)
                item.icon = sprite;

            return item;
        }
        
        private Item Create(Type objectType, JObject jObject)
        {
            return FieldExists("spriteId", jObject) ? new Equipment() : new Item();
        }

        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
        
        private List<Sprite> GetSpritesInLibraryFromLabelId(int spriteId, EquipmentSlot equipSlot)
        {
            SpriteLibrary spriteLib = SpriteLibManager.Instance.currentLibrary;
            List<Sprite> sprites = new List<Sprite>();

            if (equipSlot == EquipmentSlot.Chest) 
            {
                foreach (SpriteResolver resolver in CharacterManager.Instance.activeCharacter.chestResolvers) 
                {
                    string category = resolver.GetCategory();
                    List<string> labels = spriteLib.spriteLibraryAsset.GetCategoryLabelNames(category).ToList();
                    
                    sprites.Add(spriteLib.GetSprite(category, labels[spriteId]));
                }
            } 
            else if (equipSlot == EquipmentSlot.Pants)
            {
                foreach (SpriteResolver resolver in CharacterManager.Instance.activeCharacter.pantResolvers) 
                {
                    string category = resolver.GetCategory();
                    List<string> labels = spriteLib.spriteLibraryAsset.GetCategoryLabelNames(category).ToList();
                    
                    sprites.Add(spriteLib.GetSprite(category, labels[spriteId]));
                }
            } else {
                string category = equipSlot switch
                {
                    EquipmentSlot.Helmet => CharacterManager.Instance.activeCharacter.helmetResolver.GetCategory(),
                    EquipmentSlot.Weapon => CharacterManager.Instance.activeCharacter.weaponResolver.GetCategory(),
                    _ => ""
                };

                List<string> labels = spriteLib.spriteLibraryAsset.GetCategoryLabelNames(category).ToList();
                sprites.Add(spriteLib.GetSprite(category, labels[spriteId]));
            }

            return sprites.Count != 0 ? sprites : null;
        }
    }
}
