using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        public Image icon;
        public TextMeshProUGUI amount;

        private IBaseCharacterItem _characterItem;

        public void AddItem(IBaseCharacterItem newItem)
        {
            _characterItem = newItem;

            icon.sprite = _characterItem.Item.icon;
            icon.enabled = true;

            if (newItem.Amount <= 1) {
                amount.gameObject.SetActive(false);
                return;
                
            }
            
            amount.gameObject.SetActive(true);
            amount.text = newItem.Amount.ToString();
        }

        public void ClearSlot()
        {
            _characterItem = null;

            icon.sprite = null;
            icon.enabled = false;

            amount.gameObject.SetActive(false);
        }

        public void Clicked()
        {
            //Debug.Log(_characterItem.ToString());
            
            if (_characterItem != null) ItemStatsUI.Instance.Show(_characterItem);
        }
    }
}