using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace LGamesDev.Fighting
{
    public class FightManager : MonoBehaviour
    {
        public delegate void OnPlayerLose();

        public delegate void OnPlayerWin();

        public static FightManager Instance;

        [SerializeField] private Transform pfCharacterBattle;
        public Texture2D enemySpritesheet;

        [FormerlySerializedAs("playerCharacterBattle")] public CharacterFight playerCharacterFight;
        private CharacterFight _activeCharacterFight;
        private CharacterFight _enemyCharacterFight;
        public OnPlayerLose onPlayerLose;
        public OnPlayerWin onPlayerWin;

        private State _state;

        private void Awake()
        {
            Instance = this;
        }

        public void SetupFight(Fight fight)
        {
            //_enemyCharacterFight = SpawnCharacter(fight.opponent);

            playerCharacterFight.Intro(_enemyCharacterFight, () => { _state = State.WaitingForPlayer; });
        }

        private void Update()
        {
            if (_state == State.WaitingForPlayer)
            {
                _state = State.Busy;
                playerCharacterFight.Attack(_enemyCharacterFight, ChooseNextActiveCharacter);
            }
        }

        private CharacterFight SpawnCharacter()
        {
            Vector3 position;

            var cam = Camera.main;

            var camHeight = cam.orthographicSize * 2f;
            var camWidth = camHeight * cam.aspect;

            position = new Vector3(camWidth * 0.28f, -30);

            var characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);

            return characterTransform.GetComponent<CharacterFight>();
        }

        private void SetActiveCharacterBattle(CharacterFight characterFight)
        {
            if (_activeCharacterFight != null) _activeCharacterFight.HideSelectionCircle();

            _activeCharacterFight = characterFight;
            _activeCharacterFight.ShowSelectionCircle();
        }

        private void ChooseNextActiveCharacter()
        {
            if (TestBattleOver()) return;

            if (_activeCharacterFight == playerCharacterFight)
            {
                SetActiveCharacterBattle(_enemyCharacterFight);

                _state = State.Busy;
                _enemyCharacterFight.Attack(playerCharacterFight, () => { ChooseNextActiveCharacter(); });
            }
            else
            {
                SetActiveCharacterBattle(playerCharacterFight);
                _state = State.WaitingForPlayer;
            }
        }

        private bool TestBattleOver()
        {
            //Win
            if (_enemyCharacterFight.IsDead())
            {
                //Enemy dead, player wins
                HandlePlayerEarning(true);

                onPlayerWin?.Invoke();
                return true;
            }

            //Lose
            if (playerCharacterFight.IsDead())
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
                amount = playerCharacterFight.GetLevelSystem().GetLevel() * 16;
            else
                amount = playerCharacterFight.GetLevelSystem().GetLevel() * 9;

            playerCharacterFight.GetLevelSystem()
                .AddExperienceScalable(amount, _enemyCharacterFight.GetLevelSystem().GetLevel());
            PlayerWalletManager.Instance.AddCurrency(CurrencyType.Gold, Mathf.RoundToInt(Random.Range(amount * 0.08f, amount * 0.12f)));

            PlayerPrefs.SetInt("level", playerCharacterFight.GetLevelSystem().GetLevel());
            PlayerPrefs.SetFloat("experience", playerCharacterFight.GetLevelSystem().GetExperience());
        }

        private enum State
        {
            WaitingForPlayer,
            Busy
        }
    }
}