using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform pfUIStatSlot;

    private Item _item;
    private Action<Item> _onBuy;
    
    [SerializeField] private Transform header;
    [SerializeField] private Transform content;

    private string _message;

    public void SetupCard(Item item, Action<Item> onBuy)
    {
        _item = item;
        _onBuy = onBuy;
        
        header.Find("Item").Find("nameText").GetComponent<TextMeshProUGUI>().text = item.name;
        header.Find("Item").Find("itemImage").GetComponent<Image>().sprite = item.icon;
        header.Find("Cost").Find("costText").GetComponent<TextMeshProUGUI>().text = item.cost.ToString();

        content.Find("Item Description").GetComponent<TextMeshProUGUI>().text = "Description of " + item.name;
        
        if (item.GetType() == typeof(Equipment))
        {
            Equipment equipment = item as Equipment;

            header.Find("Item").Find("levelText").GetComponent<TextMeshProUGUI>().text =
                "Lvl." + equipment.requiredLevel.ToString();
            
            _message = "Stats \n";

            foreach (Stat stat in equipment.GetStats())
            {
                _message += stat.statType.ToString() + " : " + stat.GetValue().ToString() + "\n";
                
                /*RectTransform statSlotRectTransform =
                    Instantiate(pfUIStatSlot, content.Find("Item Stats")).GetComponent<RectTransform>();

                statSlotRectTransform.Find("label").GetComponent<TextMeshProUGUI>().text =
                    stat.statType.ToString();
                statSlotRectTransform.Find("values").Find("value").GetComponent<TextMeshProUGUI>().text =
                    stat.GetValue().ToString();
                statSlotRectTransform.Find("values").Find("modifier").gameObject.SetActive(false);*/
            }
        }
        else
        {
            header.Find("Item").Find("levelText").gameObject.SetActive(false);
        }
        
        content.gameObject.SetActive(false);
    }

    public void OnBuyButton()
    {
        _onBuy?.Invoke(_item);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.modalWindow.ShowAsPrompt(_item.name, _item.icon, _message);
        //content.gameObject.SetActive(!content.gameObject.activeInHierarchy);
    }
}
