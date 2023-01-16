using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

namespace LGamesDev.UI
{
    public class ChatWindow : MonoBehaviour
    {
        [SerializeField] private Transform pfUIMessageBox;

        [SerializeField] private Transform itemsParent;

        private InventorySlotUI[] _slots;

        private WebSocket _ws;

        private void Awake()
        {
            itemsParent = transform.Find("DialogBox").Find("itemsParent");
            
            Hide();
        }

        private void Start()
        {
            Debug.Log("connecting to websocket server ...");
            
            _ws = new WebSocket("ws://autobattle.hopto.org:35080/echo");
            _ws.Connect();
            _ws.OnMessage += (sender, e) =>
            {
                Debug.Log("Message Received from "+((WebSocket)sender).Url+", Data : "+e.Data);
            };
            _ws.OnOpen += (sender, e) =>
            {
                _ws.Send("Hello Me !");
            };
            
            Debug.Log("connected...");
        }

        /*private void Inventory_OnItemChanged(List<IBaseCharacterItem> items)
        {
            UpdateInventoryUI();
        }*/

        private void UpdateInventoryUI()
        {
            /*for (var i = 0; i < _slots.Length; i++)
                if (i < _inventoryManager.items.Count)
                {
                    _slots[i].AddItem(_inventoryManager.items[i]);
                }
                else
                {
                    _slots[i].ClearSlot();
                }*/
        }

        private void SetupInventoryUI()
        {
            //foreach (Transform child in itemsParent) Destroy(child.gameObject);
            
            /*for (var i = 0; i < _slots.Length; i++)
            {
                var itemSlotRectTransform = Instantiate(pfUIInventorySlot, _itemsParent).GetComponent<RectTransform>();

                var slot = itemSlotRectTransform.GetComponent<InventorySlotUI>();

                _slots[i] = slot;
            }

            UpdateInventoryUI();*/
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SendHello()
        {
            if(_ws == null)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
            }  
        }
    }
}
