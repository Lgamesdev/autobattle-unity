using System;
using System.Collections;
using LGamesDev.Core;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LGamesDev.Fighting
{
    public class CharacterFight : MonoBehaviour
    {
        public float introDuration = 5;

        [SerializeField] private bool isPlayerTeam;

        private HealthBar _healthBar;
        private LevelSystem _levelSystem;
        
        private CharacterStatsManager _characterStatsManager;
        private GameObject _selectionCircleGameObject;

        private CharacterManager _characterManager;

        private void OnEnable()
        {
            _characterManager = GetComponent<CharacterManager>();

            _characterStatsManager = _characterManager.statsManager;
            _characterStatsManager.OnHealthChanged += CharacterStatsManagerOnHealthChanged;
        }

        public IEnumerator SetupCharacterFight(Character character)
        {
            transform.localScale = new Vector3(1.4f, 1.4f, 1);
            
            yield return StartCoroutine(_characterManager.SetupCharacter(character));
            
            _levelSystem = new LevelSystem(character.Level, character.Experience);
            
            if (isPlayerTeam)
            {
                _healthBar = FindObjectOfType<HealthPannelUI>().playerHealthBar;
                FindObjectOfType<UsernamePanelUI>().playerUsername.text = character.Username;
                RewardUI.Instance.SetLevelSystem(_levelSystem);
            }
            else
            {
                _healthBar = FindObjectOfType<HealthPannelUI>().opponentHealthBar;
                FindObjectOfType<UsernamePanelUI>().opponentUsername.text = character.Username;
            }
            
            _healthBar.SetMaxHealth(_characterStatsManager.GetMaxHealth());
            _healthBar.SetCurrentHealth(_characterStatsManager.GetCurrentHealth());

            //PlayAnimIdle();
        }
        
        /*public void Intro(CharacterFight target, Action onIntroComplete)
        {
            var position = transform.position;
            
            var walkTargetPosition = new Vector3(position.x, position.y);
            var targetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 12f;
            position -= new Vector3(position.x + targetPosition.x, 0);
            transform.position = position;

            /*_walkTargetPosition = walkTargetPosition;
            _onWalkComplete = onIntroComplete;
            _characterManager.activeCharacter.PlayAnimMove(walkTargetPosition);#1#
        }*/

        private void CharacterStatsManagerOnHealthChanged(object sender, EventArgs e)
        {
            _healthBar.SetSize(_characterStatsManager.GetHealthPercent());
            _healthBar.SetCurrentHealth(_characterStatsManager.GetCurrentHealth());
        }

        private Vector3 GetPosition()
        {
            return transform.position;
        }

        private void Damage(int damageAmount, bool isCritical)
        {
            _characterStatsManager.TakeDamage(damageAmount);

            DamagePopup.Create(GetPosition(), damageAmount, isCritical);
            
            //TODO:  Impact effect
            //CodeMonkey.Utils.UtilsClass.ShakeCamera(.7f, .07f);

            if (IsDead()) {
                //Died
                //_characterHandler.PlayAnimLyingUp();
            }
        }

        public bool IsDead()
        {
            return _characterStatsManager.IsDead();
        }

        public void Attack(CharacterFight target, int damage, bool isCritical, Action onAttackComplete)
        {
            var runningTargetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 18f;

            var startingPosition = GetPosition();

            //Slide to Target
            _characterManager.activeCharacter.RunToPosition(runningTargetPosition, () =>
            {
                // Arrived at target, attack him
                var attackDir = (target.GetPosition() - GetPosition()).normalized;
                _characterManager.activeCharacter.Attack(attackDir, () =>
                {
                    //Target hit
                    target.Damage(damage, isCritical);
                }, () =>
                {
                    // Slide back to starting position
                    _characterManager.activeCharacter.RunToPosition(startingPosition, () =>
                    {
                        _characterManager.activeCharacter.PlayAnimIdle();
                        onAttackComplete();
                    });
                });
            });
        }

        /*public void HideSelectionCircle()
        {
            _selectionCircleGameObject.SetActive(false);
        }

        public void ShowSelectionCircle()
        {
            _selectionCircleGameObject.SetActive(true);
        }*/

        public LevelSystem GetLevelSystem()
        {
            return _levelSystem;
        }
    }
}