using LGamesDev.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private SceneIndexes sceneToLoad;
        [SerializeField] private Image img;
        [SerializeField] private Sprite _default, _pressed;
        [SerializeField] private AudioClip _compressClip, _unCompressClip;
        [SerializeField] private AudioSource _source;

        public void OnPointerDown(PointerEventData eventData)
        {
            img.sprite = _pressed;
            _source.PlayOneShot(_compressClip);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            img.sprite = _default;
            _source.PlayOneShot(_unCompressClip);
        }

        public void LoadScene()
        {
            LevelLoader.LoadNextLevel(sceneToLoad);
        }
    }
}