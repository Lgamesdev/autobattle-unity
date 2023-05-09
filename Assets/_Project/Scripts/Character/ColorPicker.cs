using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LGamesDev.UI {
    public class ColorPicker : MonoBehaviour
    {
        public static ColorPicker Instance;

        public SwitchBodyPart switchBodyPart;

        [SerializeField] private Image backgroundScreen;
        [SerializeField] private Transform colorPanel;
        [SerializeField] private Transform colors;
        [SerializeField] private GameObject pfColorButton;

        public ColorPickerButton currentColorPicker;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            colorPanel.localScale = Vector3.zero;
            backgroundScreen.raycastTarget = false;
        }

        public void Show(ColorPickerButton colorPickerButton)
        {
            currentColorPicker = colorPickerButton;

            foreach (Transform child in colors) Destroy(child.gameObject);

            foreach (Color color in currentColorPicker.colorLibrary.colors)
            {
                ColorButton colorButton = Instantiate(pfColorButton, colors).GetComponent<ColorButton>();
                colorButton.SetColor(color);
            }
            
            colorPanel.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
            backgroundScreen.raycastTarget = true;        
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        public void Hide()
        {
            colorPanel.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            backgroundScreen.raycastTarget = false;
        }
    }
}
