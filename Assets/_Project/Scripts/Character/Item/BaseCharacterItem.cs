using System;

namespace LGamesDev.Core.Character
{
    [Serializable]
    public abstract class BaseCharacterItem<T> : IBaseCharacterItem where T : Item, new()
    {
        public int id;
        public int amount = 1;
        public T item;
        
        Item IBaseCharacterItem.Item
        {
            get => item;
            set => item = value as T;
        }

        int IBaseCharacterItem.Amount
        {
            get => amount;
            set => amount = value;
        }

        public abstract void Use();
        public abstract void Sell();
    }
}