using UnityEngine;

namespace LGamesDev
{
    public class Body
    {
        public bool isMaleGender;
        public int hairIndex;
        public int moustacheIndex;
        public int beardIndex;
        public string hairColor;
        public string skinColor;
        public string chestColor;
        public string beltColor;
        public string shortColor;

        public override string ToString()
        {
            return "Body : [ \n" +
                       "isMaleGender : " + isMaleGender + "\n" +
                       "hairIndex : " + hairIndex + "\n" +
                       "moustacheIndex : " + moustacheIndex + "\n" +
                       "beardIndex : " + beardIndex + "\n" +
                       "hairColor : " + hairColor + "\n" +
                       "skinColor : " + skinColor + "\n" +
                       "chestColor : " + chestColor + "\n" +
                       "beltColor : " + beltColor + "\n" +
                       "shortColor : " + shortColor + "\n" +
                   "]";
        }
    }
}