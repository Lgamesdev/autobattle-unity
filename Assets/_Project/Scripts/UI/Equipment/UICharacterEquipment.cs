using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev.UI
{
    public class CharacterEquipmentUI : MonoBehaviour
    {
        private EquipmentSlotUI _armorSlot;

        private CharacterEquipmentManager _equipmentManager;

        private EquipmentSlotUI _helmetSlot;
        //[SerializeField] private Transform pfUI_ItemEquipment;

        //private Transform itemContainer;

        private EquipmentSlotUI _weaponSlot;

        private void Awake()
        {
            //itemContainer = transform.Find("itemContainer");
            _helmetSlot = transform.Find("helmetSlot").GetComponent<EquipmentSlotUI>();
            _armorSlot = transform.Find("armorSlot").GetComponent<EquipmentSlotUI>();
            _weaponSlot = transform.Find("weaponSlot").GetComponent<EquipmentSlotUI>();
        }

        public void Start()
        {
            _equipmentManager = CharacterEquipmentManager.Instance;
            UpdateVisual();

            _equipmentManager.OnEquipmentChanged += CharacterEquipment_OnEquipmentChanged;
        }

        private void CharacterEquipment_OnEquipmentChanged(Equipment newItem, Equipment oldItem)
        {
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            var helmetItem = _equipmentManager.currentEquipment[(int)EquipmentSlot.Head].equipment;
            if (helmetItem != null)
            {
                _helmetSlot.SetEquipment(helmetItem);
                _helmetSlot.transform.Find("emptyImage").gameObject.SetActive(false);
            }
            else
            {
                _helmetSlot.ClearSlot();
                _helmetSlot.transform.Find("emptyImage").gameObject.SetActive(true);
            }

            var armorItem = _equipmentManager.currentEquipment[(int)EquipmentSlot.Chest].equipment;
            if (armorItem != null)
            {
                _armorSlot.SetEquipment(armorItem);
                _armorSlot.transform.Find("emptyImage").gameObject.SetActive(false);
            }
            else
            {
                _armorSlot.ClearSlot();
                _armorSlot.transform.Find("emptyImage").gameObject.SetActive(true);
            }

            var weaponItem = _equipmentManager.currentEquipment[(int)EquipmentSlot.Weapon].equipment;
            if (weaponItem != null)
            {
                _weaponSlot.SetEquipment(weaponItem);
                _weaponSlot.transform.Find("emptyImage").gameObject.SetActive(false);
            }
            else
            {
                _weaponSlot.ClearSlot();
                _weaponSlot.transform.Find("emptyImage").gameObject.SetActive(true);
            }
        }
    }
}