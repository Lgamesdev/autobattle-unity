using System.Collections;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LGamesDev.Fighting
{
    public class FightManager : MonoBehaviour
    {
        public delegate void OnFightOverEvent(Reward reward, bool playerWin);
        public OnFightOverEvent OnFightOver;

        public static FightManager Instance;

        [SerializeField] private CharacterFight pfCharacterBattle;

        public CharacterFight playerCharacterFight;
        private CharacterFight _enemyCharacterFight;
        private CharacterFight _activeCharacterFight;

        private Fight _fight;
        [SerializeField] private int currentAction = 0;

        //[SerializeField] private State state = State.Busy;

        private GameManager _gameManager; 

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _gameManager = GameManager.Instance;
            
            if (_gameManager == null)
            {
                SceneManager.LoadScene((int)SceneIndexes.PersistentScene);
            }
        }

        public IEnumerator SetupFight(Fight fight)
        {
            _fight = fight;
            
            yield return StartCoroutine(playerCharacterFight.SetupCharacterFight(fight.Character));

            _enemyCharacterFight = SpawnCharacter();

            yield return StartCoroutine(_enemyCharacterFight.SetupCharacterFight(fight.Opponent));
        }

        public void StartFight()
        {
            //playerCharacterFight.Intro(_enemyCharacterFight, ChooseNextActiveCharacter);

            ChooseNextActiveCharacter();
            
            _gameManager.PlayFightMusic();
        }

        private CharacterFight SpawnCharacter()
        {
            var cam = Camera.main;

            var camHeight = cam.orthographicSize * 2f;
            var camWidth = camHeight * cam.aspect;

            Vector3 position = new Vector3(camWidth * 0.28f, -30);

            CharacterFight characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);

            return characterTransform;
        }

        private void ChooseNextActiveCharacter()
        {
            if (TestBattleOver()) return;

            FightAction fightAction = _fight.Actions[currentAction];

            if (fightAction.playerTeam)
            {
                playerCharacterFight.Attack(_enemyCharacterFight, _fight.Actions[currentAction].damage, fightAction.critialHit, ChooseNextActiveCharacter);
            }
            else
            {
                _enemyCharacterFight.Attack(playerCharacterFight, _fight.Actions[currentAction].damage, fightAction.critialHit, ChooseNextActiveCharacter);
            }

            currentAction++;
        }
        

        private bool TestBattleOver()
        {
            if (currentAction < _fight.Actions.Count) return false;
            
            Debug.Log("reward : " + _fight.Reward);
            OnFightOver?.Invoke(_fight.Reward, _fight.PlayerWin);
            HandlePlayerReward();

            return true;
        }

        private void HandlePlayerReward()
        {
            playerCharacterFight.GetLevelSystem().AddExperience(_fight.Reward.Experience);

            foreach (Currency currency in _fight.Reward.Currencies)
            {
                PlayerWalletManager.Instance.AddCurrency(
                    currency.currencyType, currency.amount
                );
            }
        }

        public void BackToMainMenu()
        {
            _gameManager.LoadMainMenu();
        }

        public void FightAgain()
        {
            StartCoroutine(FightHandler.Load(
                this,
                result =>
                {
                    _gameManager.LoadFight(result);
                }
            ));
        }

        private enum State
        {
            WaitingForPlayer,
            Busy
        }
    }
}