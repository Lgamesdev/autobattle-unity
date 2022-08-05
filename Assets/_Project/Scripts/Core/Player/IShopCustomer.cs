namespace LGamesDev.Core.Player
{
    public interface IShopCustomer
    {
        void BoughtItem(Item item);

        void SellItem(Item item);

        bool TrySpendGoldAmount(int goldAmount);

        int GetGoldAmount();
    }
}