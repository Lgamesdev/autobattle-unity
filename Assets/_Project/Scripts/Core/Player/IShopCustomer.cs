using LGamesDev.Core.Character;

namespace LGamesDev.Core.Player
{
    public interface IShopCustomer
    {
        void BoughtItem(Item item);

        void SellItem(CharacterItem item);

        bool TrySpendGoldAmount(int goldAmount);

        int GetGoldAmount();
    }
}