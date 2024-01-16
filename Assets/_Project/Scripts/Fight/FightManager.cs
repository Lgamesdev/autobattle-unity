using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LGamesDev.Fighting
{
    public class FightManager : MonoBehaviour
    {
        public static FightManager Instance;

        public FightService fightService;
        
        public Camera cam;
        public Action<int> FightSpeedChanged;

        public delegate void FightOverEvent(Fight fight);
        public FightOverEvent FightOver;

        [SerializeField] private CharacterFight pfCharacterBattle;

        public CharacterFight playerCharacterFight;
        private CharacterFight _enemyCharacterFight;
        private CharacterFight _activeCharacterFight;

        private Fight _fight;
        private int _currentAction = 0;
        public Action OnFightStart;
        public Action ActionsComplete;

        public int fightSpeed;
        public bool isFightOver;

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
                //SceneManager.LoadScene((int)SceneIndexes.PersistentScene);
            }
            
            fightService.OnFightOver += OnFightOver;
        }

        public async Task SetupFight(Fight fight)
        {
            SetFightSpeed(GameManager.Instance.GetPlayerOptions().FightSpeed);
            
            isFightOver = false;
            
            //Debug.Log("setup fight : " + fight);
            _fight = fight;

            await playerCharacterFight.SetupFighter(fight.Character);
            
            _enemyCharacterFight = SpawnCharacter();

            await _enemyCharacterFight.SetupFighter(fight.Opponent);

            Vector3 position = cam.transform.position;
            playerCharacterFight.transform.position = cam.ScreenToWorldPoint(new Vector3(
                cam.pixelWidth * 0.3f, cam.pixelHeight * 0.51f, -14
            ));
            
            _enemyCharacterFight.transform.position = cam.ScreenToWorldPoint(new Vector3(
                cam.pixelWidth * 0.7f, cam.pixelHeight * 0.51f, -14
            ));
            
            playerCharacterFight.LookAt(_enemyCharacterFight.GetPosition());
            _enemyCharacterFight.LookAt(playerCharacterFight.GetPosition());
            
            //TODO
            //yield return new WaitUntil(() => playerCharacterFight.IsGrounded() && _enemyCharacterFight.IsGrounded());
        }

        public void StartFight()
        {
            //playerCharacterFight.Intro(_enemyCharacterFight, PlayNextAction);
            
            _gameManager.PlayFightMusic();
            
            OnFightStart?.Invoke();
            PlayNextAction();
        }

        private CharacterFight SpawnCharacter()
        {
            CharacterFight characterFight = Instantiate(pfCharacterBattle, transform.position, Quaternion.identity);

            return characterFight;
        }

        private void PlayNextAction()
        {
            if (TestBattleOver()) return;

            if (_currentAction < _fight.Actions.Count)
            {
                //Debug.Log("play action " + _currentAction);
                
                FightAction fightAction = _fight.Actions[_currentAction];

                if (fightAction.PlayerTeam)
                {
                    switch (fightAction.ActionType)
                    {
                        case FightActionType.Attack:
                            playerCharacterFight.Attack(
                                _enemyCharacterFight,
                                _fight.Actions[_currentAction].Damage,
                                fightAction.CriticalHit,
                                fightAction.Dodged,
                                fightAction.EnergyGained,
                                PlayNextAction
                            );
                            break;
                        case FightActionType.Parry:
                            playerCharacterFight.Parry(
                                _enemyCharacterFight,
                                _fight.Actions[_currentAction].Damage,
                                fightAction.CriticalHit,
                                fightAction.EnergyGained,
                                PlayNextAction
                            );
                            break;
                        case FightActionType.SpecialAttack:
                            playerCharacterFight.SpecialAttack(
                                _enemyCharacterFight,
                                _fight.Actions[_currentAction].Damage,
                                fightAction.CriticalHit,
                                fightAction.Dodged,
                                PlayNextAction
                            );
                            break;
                    }
                }
                else
                {
                    switch (fightAction.ActionType)
                    {
                        case FightActionType.Attack:
                            _enemyCharacterFight.Attack(
                                playerCharacterFight,
                                _fight.Actions[_currentAction].Damage,
                                fightAction.CriticalHit,
                                fightAction.Dodged,
                                fightAction.EnergyGained,
                                PlayNextAction
                            );
                            break;
                        case FightActionType.Parry:
                            _enemyCharacterFight.Parry(
                                playerCharacterFight,
                                _fight.Actions[_currentAction].Damage,
                                fightAction.CriticalHit,
                                fightAction.EnergyGained,
                                PlayNextAction
                            );
                            break;
                        case FightActionType.SpecialAttack:
                            _enemyCharacterFight.SpecialAttack(
                                playerCharacterFight,
                                _fight.Actions[_currentAction].Damage,
                                fightAction.CriticalHit,
                                fightAction.Dodged,
                                PlayNextAction
                            );
                            break;
                    }
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
            //if (!playerCharacterFight.IsDead() && !_enemyCharacterFight.IsDead()) return false;
            if (!isFightOver) return false;
            
            // reset fight speed on battle over window
            fightSpeed = 1;
            Time.timeScale = fightSpeed;
            
            FightOver?.Invoke(_fight);
            HandlePlayerReward();

            return true;
        }

        private void HandlePlayerReward()
        {
            if (_fight.PlayerWin)
            {
                _gameManager.audioManager.PlayWinMusic();
            }
            else
            {
                _gameManager.audioManager.PlayLoseMusic();
            }
            
            //playerCharacterFight.GetLevelSystem().AddExperience(Fight.Reward.Experience);

            foreach (Currency currency in _fight.Reward.Currencies)
            {
                PlayerWalletManager.Instance.AddCurrency(
                    currency.currencyType, currency.amount
                );
            }
        }

        public void AddActions(IEnumerable<FightAction> fightActions)
        {
            _fight.Actions.AddRange(fightActions);
            PlayNextAction();
        }

        public void SetFightSpeed(int pFightSpeed)
        {
            fightSpeed = pFightSpeed;
            Time.timeScale = fightSpeed;
            
            PlayerOptions playerOptions = _gameManager.GetPlayerOptions();
            playerOptions.FightSpeed = fightSpeed;
            _gameManager.SetPlayerOptions(playerOptions);
            FightSpeedChanged?.Invoke(fightSpeed);
        }

        private void OnFightOver(Fight fight)
        {
            isFightOver = true;
            _fight.PlayerWin = fight.PlayerWin;
            _fight.Reward = fight.Reward;
            TestBattleOver();
        }

        public void BackToMainMenu()
        {
            //_gameManager.LoadMainMenu();
            Loader.Load(Loader.Scene.MenuScene);
        }

        public void FightAgain()
        {
            //_gameManager.networkService.SearchFight(_fight.FightType);
        }
    }

    public enum FightActionType
    {
        Attack, 
        Parry,
        SpecialAttack
    }

    public enum FightType
    {
        Pvp,
        Pve
    }
}