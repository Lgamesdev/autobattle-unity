using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        public Image icon;

        private IBaseCharacterItem _characterItem;

        public void AddItem(IBaseCharacterItem newItem)
        {
            _characterItem = newItem;

            icon.sprite = _characterItem.Item.icon;
            icon.enabled = true;

            if (newItem.Amount > 1) transform.Find("ItemButton").Find("amount").gameObject.SetActive(true);
        }

        public void ClearSlot()
        {
            _characterItem = null;

            icon.sprite = null;
            icon.enabled = false;

            transform.Find("ItemButton").Find("amount").gameObject.SetActive(false);
        }

        public void Clicked()
        {
            if (_characterItem != null) ItemStatsUI.Instance.Show(_characterItem);
        }
    }
}