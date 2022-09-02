using System.Collections.Generic;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev
{
    public class Character
    {
        public string username;
        public int level;
        public int xp;
        public int rating;
        public Body body;
        public List<CharacterEquipment> equipments;
        public List<Stat> fullStats;
        
        public override string ToString()
        {
            string result = "Character : [ \n" +
                            "username : " + username + "\n" +
                            "level : " + level + "\n" +
                            "xp : " + xp + "\n" +
                            "rating : " + rating + "\n" +
                            "body : " + body.ToString() + "\n" +
                            "equipments : [ \n";
            equipments.ForEach(equipment => result += equipment.ToString());
            result +=  " ] \n" +
                       "fullStats : [ \n";
            fullStats.ForEach(stat => result += stat.ToString());
            result += " ] \n";

            return result;
        }
    }
}