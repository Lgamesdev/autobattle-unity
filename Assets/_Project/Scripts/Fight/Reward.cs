using System;
using System.Collections.Generic;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;

namespace LGamesDev.Fighting
{
    public class Reward
    {
        public int Experience;
        public int Ranking;
        public Currency[] Currencies = new Currency[Enum.GetNames(typeof(CurrencyType)).Length];
        public List<IBaseCharacterItem> Items = new(); 
        
        public override string ToString()
        {
            string result = "[ \n" +
                            "experience : " + Experience + "\n" +
                            "rating : " + Ranking + "\n" +
                            "currencies : [ \n";
            foreach (Currency currency in Currencies) result += currency.ToString();
            result += " ] \n" +
                      "items : [";
            foreach (Item item in Items) result += item.ToString();
            result += " ] \n";
            return result;
        }
    }
}