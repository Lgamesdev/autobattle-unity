using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LGamesDev
{
    public class ColorButton : MonoBehaviour, IPointerUpHandler
    {
        private Color _color;

        public void SetColor(Color color)
        {
            _color = color;

            GetComponent<Image>().color = color;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SpriteColorPicker.Instance.currentColorPicker.SetSpritesColor(_color);
        }
    }
}