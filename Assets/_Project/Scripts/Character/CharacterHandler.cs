using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace LGamesDev
{
    public class CharacterHandler : MonoBehaviour
    {
        [Header("Hair")]
        public SpriteResolver HairResolver;
        [Header("Moustache")]
        public SpriteResolver MoustacheResolver;
        [Header("Beard")]
        public SpriteResolver BeardResolver;
        [Header("Body")]
        public List<SpriteResolver> BodyResolvers;
        [Header("Chest")]
        public SpriteResolver ChestResolver;
        [Header("Belt")]
        public SpriteResolver BeltResolver;
        [Header("Short")]
        public SpriteResolver ShortResolver;
        
        [Header("Weapon")]
        public SpriteResolver WeaponResolver;
        [Header("OffHand")]
        public SpriteResolver OffHandResolver;

        private CharacterEquipmentManager _equipmentManager;

        private Body _body;

        public static CharacterHandler Instance;

        private void Awake()
        {
            Instance = this;
        }

        public IEnumerator SetupCharacter()
        {
            _body = GameManager.Instance.GetPlayerBody();
            _equipmentManager = CharacterEquipmentManager.Instance;
            _equipmentManager.OnEquipmentChanged += UpdateEquipmentTexture;

            SpriteLibManager.Instance.SwitchLibrary(_body.isMaleGender ? SpriteLib.Male : SpriteLib.Female);
            
            List<string> labels = HairResolver.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(HairResolver.GetCategory()).ToList();
            HairResolver.SetCategoryAndLabel(HairResolver.GetCategory(), labels[_body.hairIndex]);
            ColorUtility.TryParseHtmlString(_body.hairColor, out Color hairColor);
            HairResolver.GetComponent<SpriteRenderer>().color = hairColor;

            labels = MoustacheResolver.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(MoustacheResolver.GetCategory()).ToList();
            MoustacheResolver.SetCategoryAndLabel(MoustacheResolver.GetCategory(), labels[_body.isMaleGender ? _body.moustacheIndex : 0]);
            MoustacheResolver.GetComponent<SpriteRenderer>().color = hairColor;

            labels = BeardResolver.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(BeardResolver.GetCategory()).ToList();
            BeardResolver.SetCategoryAndLabel(BeardResolver.GetCategory(), labels[_body.isMaleGender ? _body.beardIndex : 0]);
            MoustacheResolver.GetComponent<SpriteRenderer>().color = hairColor;

            ColorUtility.TryParseHtmlString(_body.skinColor, out Color skinColor);
            foreach (SpriteResolver bodyResolver in BodyResolvers)
            {
                bodyResolver.GetComponent<SpriteRenderer>().color = skinColor;
            }
            
            ColorUtility.TryParseHtmlString(_body.chestColor, out Color chestColor);
            ChestResolver.GetComponent<SpriteRenderer>().color = chestColor;
            
            ColorUtility.TryParseHtmlString(_body.beltColor, out Color beltColor);
            BeltResolver.GetComponent<SpriteRenderer>().color = beltColor;
            
            ColorUtility.TryParseHtmlString(_body.shortColor, out Color shortColor);
            ShortResolver.GetComponent<SpriteRenderer>().color = shortColor;

            foreach (CharacterEquipment characterEquipment in _equipmentManager.currentEquipment) {
                if (characterEquipment.equipment != null)
                    UpdateEquipmentTexture(characterEquipment.equipment, null);
            }

            yield return new WaitForEndOfFrame();
        }

        public void UpdateEquipmentTexture(Equipment newItem, Equipment oldItem)
        {
            if (newItem != null)
                // Equip Item
                switch (newItem.equipmentType)
                {
                    case EquipmentSlot.Weapon:
                        //_swordTexture = newItem.sprite.texture;
                        break;
                    case EquipmentSlot.Chest:
                        //_chestTexture = newItem.sprite.texture;
                        break;
                    case EquipmentSlot.Head:
                        //_helmetTexture = newItem.sprite.texture;
                        break;
                    /*case EquipmentSlot.Shield:
                    break;*/
                }
            else if (oldItem != null)
                // Unequip Item
                switch (oldItem.equipmentType)
                {
                    case EquipmentSlot.Weapon:
                        //_swordTexture = null;
                        break;
                    case EquipmentSlot.Chest:
                        //_chestTexture = null;
                        break;
                    case EquipmentSlot.Head:
                        //_helmetTexture = null;
                        break;
                    /*case EquipmentSlot.Shield:
                    break;*/
                }
        }
    }
}