using UnityEngine;

namespace LGamesDev.Helper
{
    public class ConvertNumber
    {
        public static string ToString(int number)
        {
            float numberF = number;
            string result;
            string[] scoreNames = new string[] { "", "k", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", };
            int i;
 
            for (i = 0; i < scoreNames.Length; i++)
                if (numberF < 900)
                    break;
                else numberF = Mathf.Floor(numberF / 100f) / 10f;
 
            if (numberF == Mathf.Floor(numberF))
                result = numberF.ToString() + scoreNames[i];
            else result = numberF.ToString("F1") + scoreNames[i];
            return result;
        }
        
        public static string ToString(float number)
        {
            string result;
            string[] scoreNames = new string[] { "", "k", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", };
            int i;
 
            for (i = 0; i < scoreNames.Length; i++)
                if (number < 900)
                    break;
                else number = Mathf.Floor(number / 100f) / 10f;
 
            if (number == Mathf.Floor(number))
                result = number.ToString() + scoreNames[i];
            else result = number.ToString("F1") + scoreNames[i];
            return result;
        }
        
        public static string ScoreShow(double number)
        {
            string result;
            string[] scoreNames = new string[] {"", "k","M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", };
            int i;
 
            for (i = 0; i < scoreNames.Length; i++)
                if (number < 900)
                    break;
                else number = System.Math.Floor(number / 100f) / 10f;
 
            if (number == System.Math.Floor(number))
                result = number.ToString() + scoreNames[i];
            else result = number.ToString("F1") + scoreNames[i];
            return result;
        }
    }
}