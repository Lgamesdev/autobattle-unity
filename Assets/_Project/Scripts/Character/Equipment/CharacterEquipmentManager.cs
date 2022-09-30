using LGamesDev.Core.Request;
using UnityEngine;

namespace LGamesDev.Core.Character
{
    public class CharacterEquipmentManager : MonoBehaviour
    {
        public delegate void OnEquipmentChangedEvent(CharacterEquipment newEquipment, CharacterEquipment oldEquipment);
        public OnEquipmentChangedEvent OnEquipmentChanged;
        
        public CharacterEquipment[] currentEquipment;

        public void SetupManager(CharacterEquipment[] characterEquipments)
        {
            currentEquipment = characterEquipments;
        }

        public void Equip(CharacterEquipment newCharacterEquipment)
        {
            int slotIndex = (int)newCharacterEquipment.item.equipmentSlot;

            CharacterEquipment oldCharacterEquipment = null;

            if (currentEquipment[slotIndex].item is { isDefaultItem: false }) {
                oldCharacterEquipment = currentEquipment[slotIndex];
            }

            StartCoroutine(CharacterEquipmentHandler.Equip(
                this,
                newCharacterEquipment,
                error =>
                {
                    Debug.Log("error on equipment saving : " + error);
                },
                response=>
                {
                    Debug.Log("Equipment successfully saved : " + response);
                    
                    PlayerInventoryManager.Instance.RemoveItem(newCharacterEquipment);
                    PlayerInventoryManager.Instance.AddItem(oldCharacterEquipment);
                    
                    currentEquipment[slotIndex] = newCharacterEquipment;
                    OnEquipmentChanged?.Invoke(newCharacterEquipment, oldCharacterEquipment);
                }
            ));
        }

        public void UnEquip(int slotIndex)
        {
            if (currentEquipment[slotIndex] == null) return;
            
            CharacterEquipment oldCharacterEquipment = currentEquipment[slotIndex];
            PlayerInventoryManager.Instance.AddItem(oldCharacterEquipment);

            currentEquipment[slotIndex] = null;

            OnEquipmentChanged?.Invoke(null, oldCharacterEquipment);
        }
    }
}