using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using Core.Player;
using LGamesDev.Core.Request;
using LGamesDev.Request.Converters;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace LGamesDev.UI
{
    public class RankingUI : MonoBehaviour
    {
        private Transform _container;

        private Transform _goldAmount;
        private Transform _playerCardTemplate;

        private List<Character> _characters = new();

        private void Awake()
        {
            _container = transform.Find("container");
            _playerCardTemplate = _container.Find("playerCardTemplate");
            _playerCardTemplate.gameObject.SetActive(false);
        }

        private void Start()
        {
            StartCoroutine(RequestHandler.Request("api/user/ranking",
                UnityWebRequest.kHttpVerbGET,
                error => { Debug.Log("Error on /user/ranking : " + error); },
                response =>
                {
                    //Debug.Log("Received /user/ranking : " + response);

                    _characters = JsonConvert.DeserializeObject<List<Character>>(response);

                    SetupUI();
                },
                null,
                GameManager.Instance.GetAuthentication())
            );
        }

        private void SetupUI()
        {
            Character playerCharacter = CharacterManager.Instance.Character;

            CreateCharacterCard(new Character()
            {
                Username = GameManager.Instance.GetAuthentication().username,
                Level = playerCharacter.Level,
                Ranking = playerCharacter.Ranking,
            }, 0, true);
            
            for (int i = 0; i < _characters.Count; i++) {
                CreateCharacterCard(_characters[i], i + 1);
            }
        }

        private void CreateCharacterCard(Character character, int positionIndex, bool isPlayerCharacter = false)
        {
            var playerCardTransform = Instantiate(_playerCardTemplate, _container);
            playerCardTransform.gameObject.SetActive(true);
            RectTransform playerCardRectTransform = playerCardTransform.GetComponent<RectTransform>();

            float playerCardHeight = 120f;
            playerCardRectTransform.anchoredPosition = new Vector2(0, -playerCardHeight * positionIndex);

            playerCardTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = isPlayerCharacter ? character.Username + " (You)" : character.Username;
            playerCardTransform.Find("levelText").GetComponent<TextMeshProUGUI>().text = "level : " + character.Level.ToString();
            playerCardTransform.Find("rankText").GetComponent<TextMeshProUGUI>().text = character.Ranking.ToString();

            //playerCardTransform.Find("playerImage").GetComponent<Image>().sprite = player.body;

            playerCardTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                //Clicked on shop item button
                Debug.Log("popup details playerCard");
            };
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}