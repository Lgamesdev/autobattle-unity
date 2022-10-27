using System;

namespace LGamesDev.Core.Character
{
    [Serializable]
    public abstract class BaseCharacterItem<T> : IBaseCharacterItem where T : Item, new()
    {
        public int id;
        public int amount = 1;
        public T item;

        int IBaseCharacterItem.Id => id;
        
        Item IBaseCharacterItem.Item => item;

        int IBaseCharacterItem.Amount
        {
            get => amount;
            set => amount = value;
        }

        public abstract void Use();
        public abstract void Sell();
    }
}