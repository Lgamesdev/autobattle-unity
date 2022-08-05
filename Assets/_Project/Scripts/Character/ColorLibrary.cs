using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev
{
    [CreateAssetMenu(fileName = "New Color Library", menuName = "Color/Library")]
    [Serializable]
    public class ColorLibrary : ScriptableObject
    {
        public List<Color> colors;
    }
}