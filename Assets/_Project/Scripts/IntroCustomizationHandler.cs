using LGamesDev.UI;
using UnityEngine;

namespace LGamesDev
{
    public class IntroCustomizationHandler : MonoBehaviour
    {
        public PopupHandler popupHandler;

        // Start is called before the first frame update
        private void Start()
        {
            popupHandler.PopUp("Kalcifer : Hey welcome ! Here you can customize your character :)");
        }
    }
}