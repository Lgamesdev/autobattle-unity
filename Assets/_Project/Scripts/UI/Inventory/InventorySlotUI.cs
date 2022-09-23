using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        public Image icon;

        public Item item;

        public void AddItem(Item newItem)
        {
            item = newItem;

            icon.sprite = item.icon;
            icon.enabled = true;

            if (newItem.amount > 1) transform.Find("ItemButton").Find("amount").gameObject.SetActive(true);
        }

        public void ClearSlot()
        {
            item = null;

            icon.sprite = null;
            icon.enabled = false;

            transform.Find("ItemButton").Find("amount").gameObject.SetActive(false);
        }

        public void Clicked()
        {
            if (item != null) ItemStatsUI.Instance.Show(item);
        }
    }
}