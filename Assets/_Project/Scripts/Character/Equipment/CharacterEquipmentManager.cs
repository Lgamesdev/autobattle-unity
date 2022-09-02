using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev.Core.Character
{
    public class CharacterEquipmentManager : MonoBehaviour
    {
        public delegate void OnEquipmentChangedEvent(Equipment newItem, Equipment oldItem);
        public OnEquipmentChangedEvent OnEquipmentChanged;

        public static CharacterEquipmentManager Instance;

        public CharacterEquipment[] currentEquipment;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one instance of CharacterEquipment found ! ");
                return;
            }

            Instance = this;
        }

        public void Equip(Equipment newItem)
        {
            var slotIndex = (int)newItem.equipmentType;

            Equipment oldItem = null;

            if (currentEquipment[slotIndex].equipment != null)
            {
                oldItem = currentEquipment[slotIndex].equipment;
                PlayerInventoryManager.Instance.Inventory.AddItem(oldItem);
            }

            currentEquipment[slotIndex].equipment = newItem;

            OnEquipmentChanged?.Invoke(newItem, oldItem);
        }

        public void Unequip(int slotIndex)
        {
            if (currentEquipment[slotIndex].equipment != null)
            {
                var oldItem = currentEquipment[slotIndex].equipment;
                PlayerInventoryManager.Instance.Inventory.AddItem(oldItem);

                currentEquipment[slotIndex] = null;

                OnEquipmentChanged?.Invoke(null, oldItem);
            }
        }

        public void UnequipAll()
        {
            for (var i = 0; i < currentEquipment.Length; i++) Unequip(i);
        }
    }
}