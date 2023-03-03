using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LGamesDev.Helper
{
    public static class AbbreviationUtility
    {
        public static string AbbreviateNumber(float number)
        {
            return number switch
            {
                > 1000000000 =>
                    (number / 10000).ToString("F") + "B",
                > 1000000 =>
                    (number / 10000).ToString("F") + "M",
                > 1000 =>
                    (number / 1000).ToString("F") + "K",
                _ => 
                    number.ToString()
            };;
        }
    }
}