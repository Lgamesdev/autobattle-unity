using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LGamesDev.Fighting
{
    public class FightManager : MonoBehaviour
    {
        public delegate void FightOverEvent(Reward reward, bool playerWin);
        public FightOverEvent FightOver;

        public static FightManager Instance;

        [SerializeField] private CharacterFight pfCharacterBattle;

        public CharacterFight playerCharacterFight;
        private CharacterFight _enemyCharacterFight;
        private CharacterFight _activeCharacterFight;

        public Fight Fight;
        private int _currentAction = 0;
        public Action ActionsComplete;

        private int _fightSpeed = 1;

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
            Fight = fight;

            yield return StartCoroutine(playerCharacterFight.SetupCharacterFight(fight.Character));
            

            _enemyCharacterFight = SpawnCharacter();

            yield return StartCoroutine(_enemyCharacterFight.SetupCharacterFight(fight.Opponent));
            
            var cam = Camera.main;

            var camHeight = cam.orthographicSize * 2f;
            var camWidth = camHeight * cam.aspect;

            playerCharacterFight.transform.position = new Vector3(camWidth * -0.30f, -20);
            _enemyCharacterFight.transform.position = new Vector3(camWidth * 0.30f, -20);
            
            playerCharacterFight.LookAt(_enemyCharacterFight.GetPosition());
            _enemyCharacterFight.LookAt(playerCharacterFight.GetPosition());
        }

        public void StartFight()
        {
            //playerCharacterFight.Intro(_enemyCharacterFight, ChooseNextActiveCharacter);
            
            _gameManager.PlayFightMusic();
            
            ChooseNextActiveCharacter();
        }

        private CharacterFight SpawnCharacter()
        {
            CharacterFight characterFight = Instantiate(pfCharacterBattle, transform.position, Quaternion.identity);

            return characterFight;
        }

        private void ChooseNextActiveCharacter()
        {
            if (TestBattleOver()) return;

            if (_currentAction < Fight.Actions.Count)
            {
                FightAction fightAction = Fight.Actions[_currentAction];

                if (fightAction.PlayerTeam)
                {
                    playerCharacterFight.Attack(
                        _enemyCharacterFight,
                        Fight.Actions[_currentAction].Damage,
                        fightAction.CriticalHit,
                        fightAction.Dodged,
                        ChooseNextActiveCharacter
                    );
                }
                else
                {
                    _enemyCharacterFight.Attack(
                        playerCharacterFight,
                        Fight.Actions[_currentAction].Damage,
                        fightAction.CriticalHit,
                        fightAction.Dodged,
                        ChooseNextActiveCharacter
                    );
                }

                _currentAction++;
            }
            else
            {
                ActionsComplete?.Invoke();
            }
        }
        

        private bool TestBattleOver()
        {
            if (!playerCharacterFight.IsDead() && !_enemyCharacterFight.IsDead()) return false;
            
            // reset fight speed on battle over window
            SetFightSpeed(1);

            FightOver?.Invoke(Fight.Reward, Fight.PlayerWin);
            HandlePlayerReward();

            return true;
        }

        private void HandlePlayerReward()
        {
            if (Fight.PlayerWin)
            {
                _gameManager.audioManager.PlayWinMusic();
            }
            else
            {
                _gameManager.audioManager.PlayLoseMusic();
            }
            
            playerCharacterFight.GetLevelSystem().AddExperience(Fight.Reward.Experience);

            foreach (Currency currency in Fight.Reward.Currencies)
            {
                PlayerWalletManager.Instance.AddCurrency(
                    currency.currencyType, currency.amount
                );
            }
        }

        public void Attack(IEnumerable<FightAction> fightActions)
        {
            Fight.Actions.AddRange(fightActions);
            ChooseNextActiveCharacter();
        }

        public void SetFightSpeed(int fightSpeed)
        {
            _fightSpeed = fightSpeed;
            Time.timeScale = _fightSpeed;
            
            PlayerOptions playerOptions = _gameManager.GetPlayerOptions();
            playerOptions.FightSpeed = _fightSpeed;
            _gameManager.SetPlayerOptions(playerOptions);
        }

        public void BackToMainMenu()
        {
            _gameManager.LoadMainMenu();
        }

        public void FightAgain()
        {
            _gameManager.networkManager.SearchFight();
            /*StartCoroutine(FightHandler.Load(
                this,
                result =>
                {
                    _gameManager.LoadFight(result);
                }
            ));*/
        }

        private enum State
        {
            WaitingForPlayer,
            Busy
        }
    }
}