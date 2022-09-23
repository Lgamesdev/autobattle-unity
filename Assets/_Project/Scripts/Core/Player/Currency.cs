using System;

namespace LGamesDev.Core.Player
{
    [Serializable]
    public class Currency
    {
        public CurrencyType currencyType;

        public int amount;

        public override string ToString()
        {
            return "currency : { \n" +
                   " type : " + currencyType + "\n" +
                   " amount : " + amount + "\n" +
                   "}";

        }
    }

    public enum CurrencyType
    {
        Gold,
        Crystal
    }
}