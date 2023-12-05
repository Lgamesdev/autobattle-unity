using System;
using System.Collections.Generic;
using LGamesDev.Core.Player;

namespace LGamesDev.Core.Character
{
    [Serializable]
    public class Gear
    {
        
        public CharacterEquipment[] equipments = new CharacterEquipment[Enum.GetNames(typeof(EquipmentSlot)).Length];

        public override string ToString()
        {
            var result = "[ \n ";
            foreach (CharacterEquipment characterEquipment in equipments) result += "\t " + characterEquipment.ToString() + "\n";
            result += "]";

            return result;
        }
    }
}