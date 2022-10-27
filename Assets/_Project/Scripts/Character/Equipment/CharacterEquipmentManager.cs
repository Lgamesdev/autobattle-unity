using LGamesDev.Core.Request;
using UnityEngine;

namespace LGamesDev.Core.Character
{
    public class CharacterEquipmentManager : MonoBehaviour
    {
        public delegate void OnEquipmentChangedEvent(CharacterEquipment newEquipment, CharacterEquipment oldEquipment);
        public OnEquipmentChangedEvent OnEquipmentChanged;
        
        public Gear currentGear;

        public void SetupManager(Gear gear)
        {
            currentGear = gear;
        }

        public void Equip(CharacterEquipment newCharacterEquipment)
        {
            int slotIndex = (int)newCharacterEquipment.item.equipmentSlot;

            CharacterEquipment oldCharacterEquipment = null;

            if (currentGear.equipments[slotIndex] != null && currentGear.equipments[slotIndex].item.isDefaultItem == false) {
                oldCharacterEquipment = currentGear.equipments[slotIndex];
            }

            StartCoroutine(GearHandler.Equip(
                this,
                newCharacterEquipment,
                error =>
                {
                    Debug.Log("error on equipment saving : " + error);
                },
                response=>
                {
                    //Debug.Log("Equipment successfully equipped : " + response);
                    
                    PlayerInventoryManager.Instance.RemoveItem(newCharacterEquipment);
                    if (oldCharacterEquipment != null) {
                        PlayerInventoryManager.Instance.AddItem(oldCharacterEquipment);
                    }

                    currentGear.equipments[slotIndex] = newCharacterEquipment;
                    OnEquipmentChanged?.Invoke(newCharacterEquipment, oldCharacterEquipment);
                }
            ));
        }

        public void UnEquip(int slotIndex)
        {
            if (currentGear.equipments[slotIndex] == null) return;
            
            CharacterEquipment oldCharacterEquipment = currentGear.equipments[slotIndex];
            
            StartCoroutine(GearHandler.UnEquip(
                this,
                oldCharacterEquipment,
                error =>
                {
                    Debug.Log("error on equipment saving : " + error);
                },
                response=>
                {
                    //Debug.Log("Equipment successfully unequipped : " + response);
                    
                    PlayerInventoryManager.Instance.AddItem(oldCharacterEquipment);

                    currentGear.equipments[slotIndex] = null;
                    OnEquipmentChanged?.Invoke(null, oldCharacterEquipment);
                }
            ));
            
            
        }
    }
}