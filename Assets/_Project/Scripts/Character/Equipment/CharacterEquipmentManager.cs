using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using UnityEngine;

namespace LGamesDev.Core.Character
{
    public class CharacterEquipmentManager : MonoBehaviour
    {
        public delegate void EquipmentChangedEvent(CharacterEquipment newEquipment, CharacterEquipment oldEquipment);
        public EquipmentChangedEvent EquipmentChanged;
        
        public Gear currentGear;

        public void SetupManager(Gear gear)
        {
            currentGear = gear;
        }

        public void TryEquip(CharacterEquipment newCharacterEquipment) 
        {
            GameManager.Instance.networkManager.TryEquip(newCharacterEquipment);
        }

        public void Equip(int id)
        {
            CharacterEquipment newCharacterEquipment = (CharacterEquipment)CharacterManager.Instance.inventoryManager.GetItemById(id);
            
            int slotIndex = (int)newCharacterEquipment.item.equipmentSlot;

            CharacterEquipment oldCharacterEquipment = null;

            if (currentGear.equipments[slotIndex] != null && currentGear.equipments[slotIndex].item.isDefaultItem == false) {
                oldCharacterEquipment = currentGear.equipments[slotIndex];
            }
            
            //Debug.Log("Equipment successfully equipped : " + response);
                    
            PlayerInventoryManager.Instance.RemoveItem(newCharacterEquipment);
            if (oldCharacterEquipment != null) {
                PlayerInventoryManager.Instance.AddItem(oldCharacterEquipment);
            }

            currentGear.equipments[slotIndex] = newCharacterEquipment;
            EquipmentChanged?.Invoke(newCharacterEquipment, oldCharacterEquipment);
            
            /*StartCoroutine(GearHandler.TryEquip(
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
                    EquipmentChanged?.Invoke(newCharacterEquipment, oldCharacterEquipment);
                }
            ));*/
        }
        
        public void TryUnEquip(int slotIndex) 
        {
            GameManager.Instance.networkManager.TryUnEquip(currentGear.equipments[slotIndex]);
        }

        public void UnEquip(int slotIndex)
        {
            if (currentGear.equipments[slotIndex] == null) return;
            
            CharacterEquipment oldCharacterEquipment = currentGear.equipments[slotIndex];
            
            PlayerInventoryManager.Instance.AddItem(oldCharacterEquipment);

            currentGear.equipments[slotIndex] = null;
            EquipmentChanged?.Invoke(null, oldCharacterEquipment);
            
            /*StartCoroutine(GearHandler.TryUnEquip(
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
                    EquipmentChanged?.Invoke(null, oldCharacterEquipment);
                }
            ));*/
        }

        public bool GotWeapon()
        {
            return !currentGear.equipments[(int)EquipmentSlot.Weapon].item.isDefaultItem;
        }
    }
}