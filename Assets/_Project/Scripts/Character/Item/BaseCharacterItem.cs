using System;

namespace LGamesDev.Core.Character
{
    [Serializable]
    public abstract class BaseCharacterItem<T> : IBaseCharacterItem where T : Item, new()
    {
        public string id;
        public T item;
        public int amount = 1;

        string IBaseCharacterItem.Id
        {
            get => id;
            set => id = value;
        }

        Item IBaseCharacterItem.Item
        {
            get => item;
            set => item = (T)value;
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