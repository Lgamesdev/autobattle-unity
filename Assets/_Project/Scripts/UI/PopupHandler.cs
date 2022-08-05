using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class PopupHandler : MonoBehaviour
    {
        //public GameObject popUpBox;
        public Animator animator;
        public TextMeshProUGUI popupText;

        public void PopUp(string text)
        {
            //popUpBox.SetActive(true);
            popupText.text = text;
            animator.SetTrigger("pop");
        }
    }
}