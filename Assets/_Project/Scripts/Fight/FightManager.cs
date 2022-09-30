using System;
using System.Collections;
using LGamesDev.Core;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace LGamesDev.Fighting
{
    public class FightManager : MonoBehaviour
    {
        public delegate void OnFightOverEvent(Reward reward, bool playerWin);
        public OnFightOverEvent OnFightOver;

        public static FightManager Instance;

        [SerializeField] private Transform pfCharacterBattle;

        public CharacterFight playerCharacterFight;
        private CharacterFight _activeCharacterFight;
        private CharacterFight _enemyCharacterFight;

        private Fight _fight;
        [SerializeField] private int currentAction = 0;

        //[SerializeField] private State state = State.Busy;

        private GameManager _gameManager; 

        private void Awake()
        {
            Instance = this;
            
            _gameManager = GameManager.Instance;
        }

        private void Start()
        {
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
            playerCharacterFight.Intro(_enemyCharacterFight, ChooseNextActiveCharacter);
        }

        private CharacterFight SpawnCharacter()
        {
            var cam = Camera.main;

            var camHeight = cam.orthographicSize * 2f;
            var camWidth = camHeight * cam.aspect;

            Vector3 position = new Vector3(camWidth * 0.28f, -30);

            var characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);

            return characterTransform.GetComponent<CharacterFight>();
        }

        private void ChooseNextActiveCharacter()
        {
            if (TestBattleOver()) return;

            FightAction fightAction = _fight.Actions[currentAction];

            if (fightAction.playerTeam)
            {
                SetActiveCharacterBattle(playerCharacterFight);

                //state = State.Busy;
                playerCharacterFight.Attack(_enemyCharacterFight, _fight.Actions[currentAction].damage, fightAction.critialHit, ChooseNextActiveCharacter);
            }
            else
            {
                SetActiveCharacterBattle(_enemyCharacterFight);

                //state = State.Busy;
                _enemyCharacterFight.Attack(playerCharacterFight, _fight.Actions[currentAction].damage, fightAction.critialHit, ChooseNextActiveCharacter);
            }

            currentAction++;
        }
        
        private void SetActiveCharacterBattle(CharacterFight characterFight)
        {
            //if (_activeCharacterFight != null) _activeCharacterFight.HideSelectionCircle();

            _activeCharacterFight = characterFight;
            //_activeCharacterFight.ShowSelectionCircle();
        }

        private bool TestBattleOver()
        {
            if (_fight.Actions.Count - 1 != currentAction) return false;
            
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
                    Debug.Log("fight request result : " + result.ToString());

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