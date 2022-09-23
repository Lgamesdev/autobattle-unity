using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev.Core.Character
{
    public class CharacterEquipmentManager : MonoBehaviour
    {
        public delegate void OnEquipmentChangedEvent(Equipment newEquipment, Equipment oldEquipment);
        public OnEquipmentChangedEvent OnEquipmentChanged;

        //public static CharacterEquipmentManager Instance;

        public CharacterEquipment[] currentEquipment;

        private void Awake()
        {
            /*if (Instance != null)
            {
                Debug.LogWarning("More than one instance of CharacterEquipment found ! ");
                return;
            }

            Instance = this;*/
        }

        public void SetupManager(CharacterEquipment[] characterEquipments)
        {
            currentEquipment = characterEquipments;
        }

        public void Equip(Equipment newEquipment)
        {
            var slotIndex = (int)newEquipment.equipmentType;

            Equipment oldEquipment = null;

            if (currentEquipment[slotIndex].equipment is { isDefaultItem: false })
            {
                oldEquipment = currentEquipment[slotIndex].equipment;
                PlayerInventoryManager.Instance.inventory.AddItem(oldEquipment);
            }

            currentEquipment[slotIndex].equipment = newEquipment;

            OnEquipmentChanged?.Invoke(newEquipment, oldEquipment);
        }

        public void UnEquip(int slotIndex)
        {
            if (currentEquipment[slotIndex].equipment == null) return;
            
            Equipment oldItem = currentEquipment[slotIndex].equipment;
            PlayerInventoryManager.Instance.AddItem(oldItem);

            currentEquipment[slotIndex].equipment = null;

            OnEquipmentChanged?.Invoke(null, oldItem);
        }

        public void UnequipAll()
        {
            for (var i = 0; i < currentEquipment.Length; i++) UnEquip(i);
        }
    }
}