using UnityEngine;

namespace LGamesDev.UI
{
    public class ScrollingTexture : MonoBehaviour
    {
        private readonly Vector2 ScrollSpeed = new(1, 1);

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            Debug.Log("in Awake");
        }

        private void OnStart()
        {
            spriteRenderer.material.SetVector("_ScrollSpeed", ScrollSpeed);
            Debug.Log("in Start");
        }
    }
}