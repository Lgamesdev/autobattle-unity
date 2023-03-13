using System;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev.UI
{
    public class CharacterEquipmentUI : MonoBehaviour
    {
        private CharacterEquipmentManager _equipmentManager;

        [SerializeField] private EquipmentSlotUI helmetSlot;
        [SerializeField] private EquipmentSlotUI armorSlot;
        [SerializeField] private EquipmentSlotUI pantsSlot;
        [SerializeField] private EquipmentSlotUI weaponSlot;
        [SerializeField] private EquipmentSlotUI offHandSlot;
        
        //[SerializeField] private Transform pfUI_ItemEquipment;

        //private Transform itemContainer;
        
        /*private void Awake()
        {
            //itemContainer = transform.Find("itemContainer");
            _helmetSlot = transform.Find("helmetSlot").GetComponent<EquipmentSlotUI>();
            _armorSlot = transform.Find("armorSlot").GetComponent<EquipmentSlotUI>();
            _pantsSlot = transform.Find("pantsSlot").GetComponent<EquipmentSlotUI>();
            _weaponSlot = transform.Find("weaponSlot").GetComponent<EquipmentSlotUI>();
            _offHandSlot = transform.Find("offHandSlot").GetComponent<EquipmentSlotUI>();
        }*/

        public void Start()
        {
            _equipmentManager = CharacterManager.Instance.equipmentManager;//CharacterEquipmentManager.Instance;
            _equipmentManager.EquipmentChanged += CharacterEquipment_OnEquipmentChanged;
            
            SetupVisual();
        }

        private void SetupVisualEquipment(EquipmentSlot equipmentSlot, CharacterEquipment equipment = null)
        {
            if (equipment != null && equipment.item.spriteId != 0) {
                switch (equipment.item.equipmentSlot)
                {
                    case EquipmentSlot.Helmet:
                        helmetSlot.SetCharacterEquipment(equipment);
                        helmetSlot.transform.Find("emptyImage").gameObject.SetActive(false);
                        break;
                    case EquipmentSlot.Chest:
                        armorSlot.SetCharacterEquipment(equipment);
                        armorSlot.transform.Find("emptyImage").gameObject.SetActive(false);
                        break;
                    case EquipmentSlot.Pants:
                        pantsSlot.SetCharacterEquipment(equipment);
                        pantsSlot.transform.Find("emptyImage").gameObject.SetActive(false);
                        break;
                    case EquipmentSlot.Weapon:
                        weaponSlot.SetCharacterEquipment(equipment);
                        weaponSlot.transform.Find("emptyImage").gameObject.SetActive(false);
                        break;
                }
            } else {
                switch (equipmentSlot)
                {
                    case EquipmentSlot.Helmet:
                        helmetSlot.ClearSlot();
                        helmetSlot.transform.Find("emptyImage").gameObject.SetActive(true);
                        break;
                    case EquipmentSlot.Chest:
                        armorSlot.ClearSlot();
                        armorSlot.transform.Find("emptyImage").gameObject.SetActive(true);
                        break;
                    case EquipmentSlot.Pants:
                        pantsSlot.ClearSlot();
                        pantsSlot.transform.Find("emptyImage").gameObject.SetActive(true);
                        break;
                    case EquipmentSlot.Weapon:
                        weaponSlot.ClearSlot();
                        weaponSlot.transform.Find("emptyImage").gameObject.SetActive(true);
                        break;
                }
            }
        }

        private void CharacterEquipment_OnEquipmentChanged(CharacterEquipment newEquipment, CharacterEquipment oldEquipment)
        {
            if (newEquipment != null)
            {
                SetupVisualEquipment((newEquipment.item).equipmentSlot, newEquipment);
            } 
            else if (oldEquipment != null)
            {
                SetupVisualEquipment((oldEquipment.item).equipmentSlot);
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
                if (_equipmentManager.currentGear.equipments[(int)equipmentSlot] != null)
                {
                    SetupVisualEquipment(equipmentSlot, _equipmentManager.currentGear.equipments[(int)equipmentSlot]);
                }
                else
                {
                    SetupVisualEquipment(equipmentSlot);
                }
            }
        }
    }
}