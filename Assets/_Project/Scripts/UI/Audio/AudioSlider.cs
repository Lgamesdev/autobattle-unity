using LGamesDev.Core.Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    [RequireComponent(typeof(Slider))]
    public class AudioSlider : MonoBehaviour, IPointerUpHandler
    {
        private Slider _slider;
        private PlayerOptions _playerOptions;
        
        [SerializeField] private AudioTrack volumeName;
        [SerializeField] private TextMeshProUGUI volumeLabel;
        
        private void Start()
        {
            _playerOptions = GameManager.Instance.GetPlayerOptions();
            _slider = GetComponent<Slider>();

            float value = volumeName switch
            {
                AudioTrack.Music => _playerOptions.MusicVolume,
                AudioTrack.Effects => _playerOptions.EffectsVolume,
                _ => _slider.value
            };
            
            SetupSlider(value);
            
            _slider.onValueChanged.AddListener(delegate
            {
                UpdateValueOnChange(_slider.value);
            });
        }

        private void UpdateValueOnChange(float value)
        {
            GameManager.Instance.audioManager.SetMixerVolume(volumeName, value);
            
            if (volumeLabel != null)
                volumeLabel.text = Mathf.Round(value * 100.0f).ToString() + "%";
        }

        private void SetupSlider(float value)
        {
            _slider.value = value;
            
            if (volumeLabel != null)
                volumeLabel.text = Mathf.Round(value * 100.0f).ToString() + "%";
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            switch (volumeName)
            {
                case AudioTrack.Music:
                    _playerOptions.MusicVolume = _slider.value;
                    break;
                case AudioTrack.Effects:
                    _playerOptions.EffectsVolume = _slider.value;
                    break;
            }
            
            GameManager.Instance.SetPlayerOptions(_playerOptions);
        }
    }
}
