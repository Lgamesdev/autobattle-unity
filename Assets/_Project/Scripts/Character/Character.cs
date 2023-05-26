using System;
using System.Collections.Generic;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev
{
    [Serializable]
    public class Character : Fighter
    {
        public int Experience;
        public int MaxExperience;
        public int StatPoint;
        public int Ranking;
        public Wallet Wallet = new();
        public Gear Gear = new();
        public Inventory Inventory = new();

        public override string ToString()
        {
            string result = "Character : [ \n" +
                            "\t " + base.ToString() + "\n" +
                            "\t xp : " + Experience + "\n" +
                            "\t maxXp : " + MaxExperience + "\n" +
                            "\t ranking : " + Ranking + "\n" +
                            "\t gear : " + Gear + "\n" +
                            " ] \n";
            return result;
        }
    }
}