using System;

namespace LGamesDev.Core.Character
{
    public interface IBaseCharacterItem
    {
        public abstract int Id { get; }
        public abstract Item Item { get; }
        
        public abstract int Amount { get; set; }

        public abstract void Use();
        public abstract void Sell();
    }
}