using System;
using System.Collections.Generic;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev
{
    public class Character
    {
        public string Username;
        public int Level;
        public int Experience;
        public int Ranking;
        public Body Body;
        public Wallet Wallet = new();
        public Stat[] Stats = new Stat[Enum.GetNames(typeof(StatType)).Length];
        public Gear Gear = new();
        public Inventory Inventory = new();

        public override string ToString()
        {
            string result = "Character : [ \n" +
                            "\t username : " + Username + "\n" +
                            "\t level : " + Level + "\n" +
                            "\t xp : " + Experience + "\n" +
                            "\t ranking : " + Ranking + "\n" +
                            "\t body : " + Body.ToString() + "\n" +
                            "\t gear : " + Gear.ToString() + "\n" +
                            "\t stats : [ \n";
            foreach (Stat stat in Stats) { result += "\t " + stat.ToString(); }
            result += " ] \n";

            return result;
        }
    }
}