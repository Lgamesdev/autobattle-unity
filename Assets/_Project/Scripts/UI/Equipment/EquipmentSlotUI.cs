using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class EquipmentSlotUI : MonoBehaviour
    {
        public Image icon;

        private CharacterEquipment _characterEquipment;

        public void SetCharacterEquipment(CharacterEquipment characterEquipment)
        {
            this._characterEquipment = characterEquipment;

            icon.sprite = characterEquipment.item.icon;
            icon.enabled = true;
        }

        public void ClearSlot()
        {
            _characterEquipment = null;

            icon.sprite = null;
            icon.enabled = false;
        }

        public CharacterEquipment GetCharacterEquipment()
        {
            return _characterEquipment;
        }

        public void Clicked()
        {
            if (_characterEquipment != null) ItemStatsUI.Instance.ShowEquipped(_characterEquipment);
        }
    }
}