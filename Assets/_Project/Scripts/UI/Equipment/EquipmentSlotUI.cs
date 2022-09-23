using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class EquipmentSlotUI : MonoBehaviour
    {
        public Image icon;

        private Core.Player.Equipment _equipment;

        public void SetEquipment(Core.Player.Equipment equipment)
        {
            this._equipment = equipment;

            icon.sprite = equipment.icon;
            icon.enabled = true;
        }

        public void ClearSlot()
        {
            _equipment = null;

            icon.sprite = null;
            icon.enabled = false;
        }

        public Core.Player.Equipment GetEquipment()
        {
            return _equipment;
        }

        public void Clicked()
        {
            if (_equipment != null) ItemStatsUI.Instance.ShowEquipped(_equipment);
        }
    }
}