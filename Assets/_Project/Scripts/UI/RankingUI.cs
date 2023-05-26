using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev.UI
{
    public class RankingUI : MonoBehaviour
    {
        public static RankingUI Instance;
        
        [SerializeField] private Transform container;

        private Transform _goldAmount;
        [SerializeField] private RankCardUI playerCardTemplate;

        private List<Character> _characters = new();
        public List<Character> CharacterList
        {
            set
            {
                _characters = value;
                SetupUI();
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameManager.Instance.networkManager.GetRankList();
            /*StartCoroutine(RequestHandler.Request("api/user/ranking",
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
            );*/
        }

        private void SetupUI()
        {
            foreach (Transform child in container) Destroy(child.gameObject);
            
            Character playerCharacter = CharacterManager.Instance.Character;

            CreateCharacterCard(new Character()
            {
                username = GameManager.Instance.GetAuthentication().username,
                level = playerCharacter.level,
                Ranking = playerCharacter.Ranking,
            }, 0, true);
            
            for (int i = 0; i < _characters.Count; i++) {
                CreateCharacterCard(_characters[i], i + 1);
            }
        }

        private void CreateCharacterCard(Character character, int positionIndex, bool isPlayerCharacter = false)
        {
            RankCardUI playerCard = Instantiate(playerCardTemplate, container);
            playerCard.SetupCard(character, isPlayerCharacter);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}