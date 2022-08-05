using CodeMonkey.Utils;
using LGamesDev.Core;
using TMPro;
using UnityEngine;

namespace LGamesDev.Battle
{
    public class DamagePopup : MonoBehaviour
    {
        private const float DisappearTimerMax = 1f;

        private static int _sortingOrder;
        private float _disappearTimer;
        private Vector3 _moveVector;
        private Color _textColor;

        private TextMeshPro _textMesh;


        private void Awake()
        {
            _textMesh = transform.GetComponent<TextMeshPro>();
        }

        private void Update()
        {
            transform.position += _moveVector * Time.deltaTime;
            _moveVector -= (8f * Time.deltaTime) * _moveVector;

            if (_disappearTimer > DisappearTimerMax * .5f)
            {
                //First half of the popup lifetime
                var increaseScaleAmount = 1f;
                transform.localScale += (increaseScaleAmount * Time.deltaTime) *Vector3.one;
            }
            else
            {
                //Second half of the popup lifetime
                var decreaseScaleAmount = 1f;
                transform.localScale -= (decreaseScaleAmount * Time.deltaTime) * Vector3.one;
            }

            _disappearTimer -= Time.deltaTime;
            if (_disappearTimer < 0)
            {
                //Start disappearing
                var disapearSpeed = 3f;
                _textColor.a -= disapearSpeed * Time.deltaTime;
                _textMesh.color = _textColor;
                if (_textColor.a < 0) Destroy(gameObject);
            }
        }

        //Create a Damage Popup
        public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit)
        {
            var damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);

            var damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
            damagePopup.Setup(damageAmount, isCriticalHit);

            return damagePopup;
        }

        private void Setup(int damageAmount, bool isCriticalHit)
        {
            _textMesh.text = damageAmount.ToString();
            if (!isCriticalHit)
            {
                //Normal hit
                _textMesh.fontSize = 64;
                _textColor = UtilsClass.GetColorFromString("FFFFFF");
            }
            else
            {
                // Critical hit
                _textMesh.fontSize = 70;
                _textColor = UtilsClass.GetColorFromString("FFEA00");
            }

            _textMesh.color = _textColor;
            _disappearTimer = DisappearTimerMax;

            _sortingOrder++;
            _textMesh.sortingOrder = _sortingOrder;
            _moveVector = new Vector3(1, 2) * 65f;
        }
    }
}