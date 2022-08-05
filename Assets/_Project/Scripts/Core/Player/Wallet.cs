using UnityEngine;

namespace LGamesDev.Core.Player
{
    public class Wallet
    {
        public Currency[] Currencies;

        public Currency GetCurrency(CurrencyType currencyType)
        {
            return Currencies[(int)currencyType];
        }
        
        public int GetAmount(CurrencyType currencyType)
        {
            return Currencies[(int)currencyType].amount;
        }

        public void SpendCurrency(CurrencyType currencyType, int amount)
        {
            Currencies[(int)currencyType].amount -= amount;
        }

        public void AddCurrency(CurrencyType currencyType, int amount)
        {
            Currencies[(int)currencyType].amount += amount;
        }
    }
}