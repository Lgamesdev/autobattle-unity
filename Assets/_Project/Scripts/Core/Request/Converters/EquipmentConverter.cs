using System;
using System.Collections.Generic;
using CodeMonkey.Utils;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace LGamesDev.Request.Converters
{
    public class EquipmentConverter : JsonConverter<Equipment>
    {
        public override void WriteJson(JsonWriter writer, Equipment value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override Equipment ReadJson(JsonReader reader, Type objectType, Equipment existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) {
                return null;
            }

            
            var jsonObject = JObject.Load(reader);

            Equipment equipment = (existingValue ?? new Equipment());

            string spritePath = jsonObject["spritePath"]?.ToString();
            Debug.Log("spritePath : " + spritePath);
            if (spritePath != null) {
                equipment.sprite = GetSpriteInLibraryFromPath(spritePath);
            }

            using var subReader = jsonObject.CreateReader();
            serializer.Populate(subReader, equipment);

            return equipment;
        }

        public override bool CanWrite => false;

        private Sprite GetSpriteInLibraryFromPath(string path)
        {
            SpriteLibrary spriteLib = SpriteLibManager.Instance.currentLibrary;
            foreach (string categoryName in spriteLib.spriteLibraryAsset.GetCategoryNames()) {
                foreach (string labelName in spriteLib.spriteLibraryAsset.GetCategoryLabelNames(categoryName))
                {
                    Sprite sprite = spriteLib.GetSprite(categoryName, labelName);
                    if (AssetDatabase.GetAssetPath(sprite).Equals(path))
                    {
                        return sprite;
                    }
                }
            }

            return null;
        }
    }
}
