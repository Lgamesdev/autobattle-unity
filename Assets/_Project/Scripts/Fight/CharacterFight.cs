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

        [SerializeField] private LayerMask platformLayerMask;
        private bool _isGrounded;
        
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
                FindObjectOfType<InfosPanelUI>().playerUsername.text = character.Username;
                FindObjectOfType<InfosPanelUI>().playerLevel.text = "lvl. " + character.Level;
                RewardUI.Instance.SetLevelSystem(_levelSystem);
            }
            else
            {
                _healthBar = FindObjectOfType<HealthPannelUI>().opponentHealthBar;
                FindObjectOfType<InfosPanelUI>().opponentUsername.text = character.Username;
                FindObjectOfType<InfosPanelUI>().opponentLevel.text = "lvl. " + character.Level;
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

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        private void Damage(int damageAmount, bool isCritical, bool dodged)
        {
            _characterStatsManager.TakeDamage(damageAmount);

            if (dodged)
            {
                _characterManager.activeCharacter.Dodge();
            }
            else
            {
                _characterManager.activeCharacter.TakeHit();
            }

            DamagePopup.Create(GetPosition(), damageAmount, isCritical, dodged);
            //TODO:  Impact effect

            if (IsDead()) {
                //Lose
                _characterManager.activeCharacter.PlayLose();
            }
        }

        public bool IsDead()
        {
            return _characterStatsManager.IsDead();
        }

        public void Attack(CharacterFight target, int damage, bool isCritical, bool dodged, Action onAttackComplete)
        {
            var runningTargetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 15f;

            var startingPosition = GetPosition();

            //Slide to Target
            _characterManager.activeCharacter.RunToPosition(runningTargetPosition, () =>
            {
                // Arrived at target, attack him
                var attackDir = (target.GetPosition() - GetPosition()).normalized;
                _characterManager.activeCharacter.Attack(attackDir, () =>
                {
                    //Target hit
                    target.Damage(damage, isCritical, dodged);
                }, () =>
                {
                    if (!target.IsDead())
                    {
                        // Slide back to starting position
                        _characterManager.activeCharacter.RunToPosition(startingPosition, () =>
                        {
                            _characterManager.activeCharacter.PlayAnimIdle();
                            onAttackComplete?.Invoke();
                        });
                    }
                    else
                    {
                        _characterManager.activeCharacter.PlayWin();
                        onAttackComplete?.Invoke();
                    }
                });
            });
        }

        public void LookAt(Vector3 position)
        {
            _characterManager.activeCharacter.LookAt(position);
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

        public bool IsGrounded()
        {
            return _isGrounded;
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            _isGrounded = col != null && (((1 << col.gameObject.layer) & platformLayerMask) != 0);
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            _isGrounded = false;
        }
    }
}