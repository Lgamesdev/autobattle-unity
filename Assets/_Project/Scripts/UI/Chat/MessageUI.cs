using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class MessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI author;
        [SerializeField] private TextMeshProUGUI content;

        private string _message;

        public void Setup(string username, string message)
        {
            this.author.text = username;
            this.content.text = message;
        }
    }
}