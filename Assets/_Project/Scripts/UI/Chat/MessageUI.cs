using TMPro;
using UnityEngine;

namespace LGamesDev.UI
{
    public class MessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI author;
        [SerializeField] private TextMeshProUGUI content;

        private string _message;

        public void Setup(string author, string content)
        {
            this.author.text = author;
            this.content.text = content;
        }
    }
}