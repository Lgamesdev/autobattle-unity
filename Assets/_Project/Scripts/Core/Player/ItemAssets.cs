using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class ItemAssets : MonoBehaviour
    {
        public Equipment sword;
        public Equipment armor;
        public Equipment helmet;
        public Item healthPotion;
        public static ItemAssets Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}