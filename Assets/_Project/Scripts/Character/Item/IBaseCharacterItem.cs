namespace LGamesDev.Core.Character
{
    public interface IBaseCharacterItem
    {
        public abstract Item Item { get; set; }
        
        public abstract int Amount { get; set; }

        public abstract void Use();
        public abstract void Sell();
    }
}