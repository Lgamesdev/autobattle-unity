using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LGamesDev
{
    public class SpriteLibSwitchComponent : MonoBehaviour
    {
        public SpriteLib spriteLib;

        public void SwitchLib()
        {
            SpriteLibManager.Instance.SwitchLibrary(spriteLib);
        }
    }


    public enum SpriteLib
    {
        Male,
        Female
    }
}
