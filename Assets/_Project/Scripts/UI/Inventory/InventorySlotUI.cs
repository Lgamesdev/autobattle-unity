using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        public Image icon;

        private Item _item;

        public void AddItem(Item newItem)
        {
            _item = newItem;

            icon.sprite = _item.icon;
            icon.enabled = true;

            if (newItem.amount > 1) transform.Find("ItemButton").Find("amount").gameObject.SetActive(true);
        }

        public void ClearSlot()
        {
            _item = null;

            icon.sprite = null;
            icon.enabled = false;

            transform.Find("ItemButton").Find("amount").gameObject.SetActive(false);
        }

        public Item GetItem()
        {
            return _item != null ? _item : null;
        }

        public void Clicked()
        {
            if (_item != null) ItemStatsUI.Show_Static(_item);
        }
    }
}