using System;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace LGamesDev
{
    public class SpriteLibManager : MonoBehaviour
    {
        public static SpriteLibManager Instance;

        [SerializeField] private SpriteLibraryAsset maleLibrary;
        [SerializeField] private SpriteLibraryAsset femaleLibrary;

        public SpriteLibrary currentLibrary;

        public delegate void OnSpriteLibChangeEvent(SpriteLib spriteLib);

        public OnSpriteLibChangeEvent OnSpriteLibChange;

        private void Awake()
        {
            Instance = this;

            currentLibrary = transform.GetComponent<SpriteLibrary>();
        }

        public void SwitchLibrary(SpriteLib library)
        {
            OnSpriteLibChange?.Invoke(library);

            currentLibrary.spriteLibraryAsset = library switch
            {
                SpriteLib.Male => maleLibrary,
                SpriteLib.Female => femaleLibrary,
                _ => throw new ArgumentOutOfRangeException(nameof(library), library, null)
            };
        }
    }
}
