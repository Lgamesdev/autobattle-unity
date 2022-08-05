using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class Scroller : MonoBehaviour
    {
        [SerializeField] private RawImage img;
        [SerializeField] private float x;
        [SerializeField] private float y;

        private void Update()
        {
            img.uvRect = new Rect(img.uvRect.position + new Vector2(x, y) * Time.deltaTime, img.uvRect.size);
        }
    }
}