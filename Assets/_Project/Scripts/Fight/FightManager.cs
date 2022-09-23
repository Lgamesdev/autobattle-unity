using System;
using System.Collections;
using LGamesDev.Core.Player;
using LGamesDev.Core.Request;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace LGamesDev.Fighting
{
    public class FightManager : MonoBehaviour
    {
        public delegate void OnPlayerLoseEvent();
        public OnPlayerLoseEvent OnPlayerLose;

        public delegate void OnPlayerWinEvent();
        public OnPlayerWinEvent OnPlayerWin;

        public static FightManager Instance;

        [SerializeField] private Transform pfCharacterBattle;

        public CharacterFight playerCharacterFight;
        private CharacterFight _activeCharacterFight;
        private CharacterFight _enemyCharacterFight;

        private Fight _fight;
        [SerializeField] private int currentAction = 0;

        [SerializeField] private State state = State.Busy;

        private GameManager _gameManager; 

        private void Awake()
        {
            Instance = this;
            
            _gameManager = GameManager.Instance;
        }

        public IEnumerator SetupFight(Fight fight)
        {
            _fight = fight;
            
            yield return StartCoroutine(playerCharacterFight.SetupCharacterFight(fight.character));
            
            _enemyCharacterFight = SpawnCharacter();
            yield return StartCoroutine(_enemyCharacterFight.SetupCharacterFight(fight.opponent));
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

            FightAction fightAction = _fight.actions[currentAction];

            if (fightAction.playerTeam)
            {
                SetActiveCharacterBattle(playerCharacterFight);

                state = State.Busy;
                playerCharacterFight.Attack(_enemyCharacterFight, _fight.actions[currentAction].damage, fightAction.critialHit, ChooseNextActiveCharacter);
            }
            else
            {
                SetActiveCharacterBattle(_enemyCharacterFight);

                state = State.Busy;
                _enemyCharacterFight.Attack(playerCharacterFight, _fight.actions[currentAction].damage, fightAction.critialHit, ChooseNextActiveCharacter);
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
            if (_fight.actions.Count - 1 != currentAction) return false;
            
            //Win
            if (_enemyCharacterFight.IsDead())
            {
                //Enemy dead, player wins
                HandlePlayerEarning(true);

                OnPlayerWin?.Invoke();
            }

            //Lose
            if (playerCharacterFight.IsDead())
            {
                //Player dead, enemy wins
                HandlePlayerEarning(false);

                OnPlayerLose?.Invoke();
            }

            return true;

        }

        public void HandlePlayerEarning(bool playerWin)
        {
            int amount;

            if (playerWin)
                amount = playerCharacterFight.GetLevelSystem().GetLevel() * 16;
            else
                amount = playerCharacterFight.GetLevelSystem().GetLevel() * 9;

            playerCharacterFight.GetLevelSystem()
                .AddExperienceScalable(amount, _enemyCharacterFight.GetLevelSystem().GetLevel());
            PlayerWalletManager.Instance.AddCurrency(CurrencyType.Gold, Mathf.RoundToInt(Random.Range(amount * 0.08f, amount * 0.12f)));

            PlayerPrefs.SetInt("level", playerCharacterFight.GetLevelSystem().GetLevel());
            PlayerPrefs.SetFloat("experience", playerCharacterFight.GetLevelSystem().GetExperience());
        }

        public void BackToMainMenu()
        {
            _gameManager.LoadGame();
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