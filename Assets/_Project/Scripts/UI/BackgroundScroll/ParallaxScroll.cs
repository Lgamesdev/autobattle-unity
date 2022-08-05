using UnityEngine;

namespace LGamesDev.UI
{
    public class ParallaxScroll : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _img;
        [SerializeField] private float _x, _y;

        private void Update()
        {
            //TODO : _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
        }
    }
}