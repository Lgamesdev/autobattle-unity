using UnityEngine;

namespace LGamesDev.Core
{
    public class GameAssets : MonoBehaviour
    {
        private static GameAssets _i;

        public Transform pfDamagePopup;
        public Transform levelLoader;

        public static GameAssets i
        {
            get
            {
                if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
                return _i;
            }
        }
    }
}