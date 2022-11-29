using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LGamesDev
{
    public class SpriteLibSwitchComponent : MonoBehaviour
    {
        public SpriteLib spriteLib;

        public void SwitchCharacter()
        {
            CharacterManager.Instance.CreateCharacter(spriteLib);
        }
    }
}
