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
        public Inventory Inventory = new();
        public Stat[] Stats = new Stat[Enum.GetNames(typeof(StatType)).Length];
        public CharacterEquipment[] Equipments = new CharacterEquipment[Enum.GetNames(typeof(EquipmentType)).Length];

        public override string ToString()
        {
            string result = "Character : [ \n" +
                            "username : " + Username + "\n" +
                            "level : " + Level + "\n" +
                            "xp : " + Experience + "\n" +
                            "ranking : " + Ranking + "\n" +
                            "body : " + Body.ToString() + "\n" +
                            "equipments : [ \n";
            foreach (CharacterEquipment equipment in Equipments) { result += equipment.ToString(); }
            result +=  " ] \n" +
                       "stats : [ \n";
            foreach (Stat stat in Stats) { result += stat.ToString(); }
            result += " ] \n";

            return result;
        }
    }
}