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
            _fight = fight;

            yield return StartCoroutine(playerCharacterFight.SetupCharacterFight(fight.Character));
            

            _enemyCharacterFight = SpawnCharacter();

            yield return StartCoroutine(_enemyCharacterFight.SetupCharacterFight(fight.Opponent));
            
            var cam = Camera.main;

            var camHeight = cam.orthographicSize * 2f;
            var camWidth = camHeight * cam.aspect;

            playerCharacterFight.transform.position = new Vector3(camWidth * -0.28f, -30);
            _enemyCharacterFight.transform.position = new Vector3(camWidth * 0.28f, -30);
            
            playerCharacterFight.LookAt(_enemyCharacterFight.GetPosition());
            _enemyCharacterFight.LookAt(playerCharacterFight.GetPosition());
        }

        public void StartFight()
        {
            //playerCharacterFight.Intro(_enemyCharacterFight, ChooseNextActiveCharacter);

            ChooseNextActiveCharacter();
            
            _gameManager.PlayFightMusic();
        }

        private CharacterFight SpawnCharacter()
        {
            CharacterFight characterFight = Instantiate(pfCharacterBattle, transform.position, Quaternion.identity);

            return characterFight;
        }

        private void ChooseNextActiveCharacter()
        {
            if (TestBattleOver()) return;

            FightAction fightAction = _fight.Actions[currentAction];

            if (fightAction.playerTeam)
            {
                playerCharacterFight.Attack(
                    _enemyCharacterFight, 
                    _fight.Actions[currentAction].damage, 
                    fightAction.criticalHit, 
                    fightAction.dodged, 
                    ChooseNextActiveCharacter
                );
            }
            else
            {
                _enemyCharacterFight.Attack(
                    playerCharacterFight, 
                    _fight.Actions[currentAction].damage,
                    fightAction.criticalHit,
                    fightAction.dodged, 
                    ChooseNextActiveCharacter
                );
            }

            currentAction++;
        }
        

        private bool TestBattleOver()
        {
            if (currentAction < _fight.Actions.Count) return false;

            PlayerOptions playerOptions = _gameManager.GetPlayerOptions();
            playerOptions.FightSpeed = _fightSpeed;
            _gameManager.SetPlayerOptions(playerOptions);
            // reset fight speed on battle over window
            SetFightSpeed(1);
            
            OnFightOver?.Invoke(_fight.Reward, _fight.PlayerWin);
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
            
            playerCharacterFight.GetLevelSystem().AddExperience(_fight.Reward.Experience);

            foreach (Currency currency in _fight.Reward.Currencies)
            {
                PlayerWalletManager.Instance.AddCurrency(
                    currency.currencyType, currency.amount
                );
            }
        }

        public void SetFightSpeed(int fightSpeed)
        {
            _fightSpeed = fightSpeed;
            Time.timeScale = _fightSpeed;
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