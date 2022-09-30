using System;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev.UI
{
    public class CharacterEquipmentUI : MonoBehaviour
    {
        private CharacterEquipmentManager _equipmentManager;

        private EquipmentSlotUI _helmetSlot;
        private EquipmentSlotUI _armorSlot;
        private EquipmentSlotUI _pantsSlot;
        private EquipmentSlotUI _weaponSlot;
        private EquipmentSlotUI _offHandSlot;
        
        //[SerializeField] private Transform pfUI_ItemEquipment;

        //private Transform itemContainer;
        
        private void Awake()
        {
            //itemContainer = transform.Find("itemContainer");
            _helmetSlot = transform.Find("helmetSlot").GetComponent<EquipmentSlotUI>();
            _armorSlot = transform.Find("armorSlot").GetComponent<EquipmentSlotUI>();
            _pantsSlot = transform.Find("pantsSlot").GetComponent<EquipmentSlotUI>();
            _weaponSlot = transform.Find("weaponSlot").GetComponent<EquipmentSlotUI>();
            _offHandSlot = transform.Find("offHandSlot").GetComponent<EquipmentSlotUI>();
        }

        public void Start()
        {
            _equipmentManager = CharacterHandler.Instance.equipmentManager;//CharacterEquipmentManager.Instance;
            _equipmentManager.OnEquipmentChanged += CharacterEquipment_OnEquipmentChanged;
            
            SetupVisual();
        }

        private void SetupVisualEquipment(EquipmentSlot equipmentSlot, Equipment equipment = null)
        {
            if (equipment != null && equipment.spriteId != 0) {
                switch (equipment.equipmentSlot)
                {
                    case EquipmentSlot.Helmet:
                        _helmetSlot.SetEquipment(equipment);
                        _helmetSlot.transform.Find("emptyImage").gameObject.SetActive(false);
                        break;
                    case EquipmentSlot.Chest:
                        _armorSlot.SetEquipment(equipment);
                        _armorSlot.transform.Find("emptyImage").gameObject.SetActive(false);
                        break;
                    case EquipmentSlot.Pants:
                        _pantsSlot.SetEquipment(equipment);
                        _pantsSlot.transform.Find("emptyImage").gameObject.SetActive(false);
                        break;
                    case EquipmentSlot.Weapon:
                        _weaponSlot.SetEquipment(equipment);
                        _weaponSlot.transform.Find("emptyImage").gameObject.SetActive(false);
                        break;
                }
            } else {
                switch (equipmentSlot)
                {
                    case EquipmentSlot.Helmet:
                        _helmetSlot.ClearSlot();
                        _helmetSlot.transform.Find("emptyImage").gameObject.SetActive(true);
                        break;
                    case EquipmentSlot.Chest:
                        _armorSlot.ClearSlot();
                        _armorSlot.transform.Find("emptyImage").gameObject.SetActive(true);
                        break;
                    case EquipmentSlot.Pants:
                        _pantsSlot.ClearSlot();
                        _pantsSlot.transform.Find("emptyImage").gameObject.SetActive(true);
                        break;
                    case EquipmentSlot.Weapon:
                        _weaponSlot.ClearSlot();
                        _weaponSlot.transform.Find("emptyImage").gameObject.SetActive(true);
                        break;
                }
            }
        }

        private void CharacterEquipment_OnEquipmentChanged(CharacterEquipment newEquipment, CharacterEquipment oldEquipment)
        {
            if (newEquipment != null)
            {
                SetupVisualEquipment(((Equipment)newEquipment.item).equipmentSlot, (Equipment)newEquipment.item);
            } 
            else if (oldEquipment != null)
            {
                SetupVisualEquipment(((Equipment)oldEquipment.item).equipmentSlot);
            }
            else
            {
                Debug.LogError("error on the character equipment visual loading");
            }
        }

        private void SetupVisual()
        {
            foreach (EquipmentSlot equipmentSlot in (EquipmentSlot[]) Enum.GetValues(typeof(EquipmentSlot)))
            {
                if (_equipmentManager.currentEquipment[(int)equipmentSlot] != null)
                {
                    SetupVisualEquipment(equipmentSlot, (Equipment)_equipmentManager.currentEquipment[(int)equipmentSlot].item);
                }
                else
                {
                    SetupVisualEquipment(equipmentSlot);
                }
            }
        }
    }
}