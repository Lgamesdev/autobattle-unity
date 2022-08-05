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
            popupHandler.PopUp("Kalcifer : Hey bienvenue ! :) Ici du va pouvoir creer ton personnage comme tu le veux !");
        }
    }
}