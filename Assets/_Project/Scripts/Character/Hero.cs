using System;
using System.Collections.Generic;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev
{
    [Serializable]
    public class Hero : Fighter
    {
        //public Gear Gear = new();

        public override string ToString()
        {
            string result = "Hero : [ \n" +
                            "\t " + base.ToString() + "\n" +
                            //"\t gear : " + Gear + "\n" +
                            " ] \n";
            return result;
        }
    }
}