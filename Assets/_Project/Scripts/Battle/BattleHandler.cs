using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev.Battle
{
    public class BattleHandler : MonoBehaviour
    {
        public delegate void OnPlayerLose();

        public delegate void OnPlayerWin();

        public static BattleHandler Instance;

        [SerializeField] private Transform pfCharacterBattle;
        public Texture2D enemySpritesheet;

        public CharacterBattle playerCharacterBattle;
        private CharacterBattle _activeCharacterBattle;
        private CharacterBattle _enemyCharacterBattle;
        public OnPlayerLose onPlayerLose;
        public OnPlayerWin onPlayerWin;

        private State _state;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _enemyCharacterBattle = SpawnCharacter();

            playerCharacterBattle.Intro(_enemyCharacterBattle, () => { _state = State.WaitingForPlayer; });
        }

        private void Update()
        {
            if (_state == State.WaitingForPlayer)
            {
                _state = State.Busy;
                playerCharacterBattle.Attack(_enemyCharacterBattle, ChooseNextActiveCharacter);
            }
        }

        private CharacterBattle SpawnCharacter()
        {
            Vector3 position;

            var cam = Camera.main;

            var camHeight = cam.orthographicSize * 2f;
            var camWidth = camHeight * cam.aspect;

            position = new Vector3(camWidth * 0.28f, -30);

            var characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);

            return characterTransform.GetComponent<CharacterBattle>();
        }

        private void SetActiveCharacterBattle(CharacterBattle characterBattle)
        {
            if (_activeCharacterBattle != null) _activeCharacterBattle.HideSelectionCircle();

            _activeCharacterBattle = characterBattle;
            _activeCharacterBattle.ShowSelectionCircle();
        }

        private void ChooseNextActiveCharacter()
        {
            if (TestBattleOver()) return;

            if (_activeCharacterBattle == playerCharacterBattle)
            {
                SetActiveCharacterBattle(_enemyCharacterBattle);

                _state = State.Busy;
                _enemyCharacterBattle.Attack(playerCharacterBattle, () => { ChooseNextActiveCharacter(); });
            }
            else
            {
                SetActiveCharacterBattle(playerCharacterBattle);
                _state = State.WaitingForPlayer;
            }
        }

        private bool TestBattleOver()
        {
            //Win
            if (_enemyCharacterBattle.IsDead())
            {
                //Enemy dead, player wins
                HandlePlayerEarning(true);

                onPlayerWin?.Invoke();
                return true;
            }

            //Lose
            if (playerCharacterBattle.IsDead())
            {
                //Player dead, enemy wins
                HandlePlayerEarning(false);

                onPlayerLose?.Invoke();
                return true;
            }

            return false;
        }

        public void HandlePlayerEarning(bool playerWin)
        {
            int amount;

            if (playerWin)
                amount = playerCharacterBattle.GetLevelSystem().GetLevel() * 16;
            else
                amount = playerCharacterBattle.GetLevelSystem().GetLevel() * 9;

            playerCharacterBattle.GetLevelSystem()
                .AddExperienceScalable(amount, _enemyCharacterBattle.GetLevelSystem().GetLevel());
            PlayerWalletManager.Instance.AddCurrency(CurrencyType.Gold, Mathf.RoundToInt(Random.Range(amount * 0.08f, amount * 0.12f)));

            PlayerPrefs.SetInt("level", playerCharacterBattle.GetLevelSystem().GetLevel());
            PlayerPrefs.SetFloat("experience", playerCharacterBattle.GetLevelSystem().GetExperience());
        }

        private enum State
        {
            WaitingForPlayer,
            Busy
        }
    }
}