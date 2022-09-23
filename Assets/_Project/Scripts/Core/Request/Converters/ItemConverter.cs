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
        private string _rootPath = "Assets/_Project/";

        public override bool CanConvert(Type objectType)
        {
            return typeof(Item).IsAssignableFrom(objectType);
        }
        
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
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
            if (iconPath != null) {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(_rootPath + iconPath);
                if (sprite != null)
                    item.icon = sprite;
            }

            /*if (item.GetType() == typeof(Equipment))
            {
                int? spriteId = jsonObject["spriteId"]?.ToObject<int>();
                if (spriteId != null) {
                    List<Sprite> sprites = GetSpritesInLibraryFromLabelId((int)spriteId, equipment.equipmentType);
                    if (sprites != null)
                        equipment.sprites = sprites;
                }
            }*/

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
        
        private List<Sprite> GetSpritesInLibraryFromLabelId(int spriteId, EquipmentType equipType)
        {
            SpriteLibrary spriteLib = SpriteLibManager.Instance.currentLibrary;
            List<Sprite> sprites = new List<Sprite>();

            if (equipType == EquipmentType.Chest) 
            {
                foreach (SpriteResolver resolver in CharacterHandler.Instance.chestResolvers) 
                {
                    string category = resolver.GetCategory();
                    List<string> labels = spriteLib.spriteLibraryAsset.GetCategoryLabelNames(category).ToList();
                    
                    sprites.Add(spriteLib.GetSprite(category, labels[spriteId]));
                }
            } else {
                string category = equipType switch
                {
                    EquipmentType.Helmet => CharacterHandler.Instance.helmetResolver.GetCategory(),
                    EquipmentType.Pants => CharacterHandler.Instance.pantResolver.GetCategory(),
                    EquipmentType.Weapon => CharacterHandler.Instance.weaponResolver.GetCategory(),
                    _ => ""
                };

                List<string> labels = spriteLib.spriteLibraryAsset.GetCategoryLabelNames(category).ToList();
                sprites.Add(spriteLib.GetSprite(category, labels[spriteId]));
            }

            return sprites.Count != 0 ? sprites : null;
        }
    }
}
