using System;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine.Serialization;

namespace LGamesDev
{
    [Serializable]
    public abstract class Fighter : IFighter
    {
        public string username;
        public int level;
        public Body Body;
        public Stat[] stats = new Stat[Enum.GetNames(typeof(StatType)).Length];

        string IFighter.Username
        {
            get => username;
            set => username = value;
        }

        int IFighter.Level 
        {
            get => level;
            set => level = value;
        }
        
        Body IFighter.Body 
        {
            get => Body;
            set => Body = value;
        }
        
        Stat[] IFighter.Stats 
        {
            get => stats;
            set => stats = value;
        }

        public override string ToString()
        {
            string result = "Fighter : [ \n" +
                            "\t level : " + level + "\n" +
                            "\t body : " + Body + "\n" +
                            "\t stats : [ \n";
            foreach (Stat stat in stats) { result += "\t " + stat; }
            result += " ] \n";

            return result;
        }
    }
}