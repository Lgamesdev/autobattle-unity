using System;
using System.Collections;
using LGamesDev.Core;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace LGamesDev.Fighting
{
    public class CharacterFight : MonoBehaviour
    {
        public float introDuration = 5;

        [SerializeField] private bool isPlayerTeam;

        private ResourceBar _healthBar;
        private ResourceBar _energyBar;
        private int _energy;
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
            transform.localScale = new Vector3(.85f, .85f, 1);
            
            yield return StartCoroutine(_characterManager.SetupCharacter(character));
            
            _levelSystem = new LevelSystem(character.Level, character.Experience);
            
            if (isPlayerTeam)
            {
                _healthBar = FindObjectOfType<ResourcePanelUI>().playerHealthBar;
                _energyBar = FindObjectOfType<ResourcePanelUI>().playerEnergyBar;
                FindObjectOfType<InfosPanelUI>().playerUsername.text = character.Username;
                FindObjectOfType<InfosPanelUI>().playerLevel.text = "lvl. " + character.Level;
                RewardUI.Instance.SetLevelSystem(_levelSystem);
            }
            else
            {
                _healthBar = FindObjectOfType<ResourcePanelUI>().opponentHealthBar;
                _energyBar = FindObjectOfType<ResourcePanelUI>().opponentEnergyBar;
                FindObjectOfType<InfosPanelUI>().opponentUsername.text = character.Username;
                FindObjectOfType<InfosPanelUI>().opponentLevel.text = "lvl. " + character.Level;
            }
            
            _healthBar.SetMaxResource(_characterStatsManager.GetMaxHealth());
            _healthBar.SetCurrentResource(_characterStatsManager.GetCurrentHealth());
            
            _energyBar.SetMaxResource(100);
            SetEnergy(0);

            //PlayIdle();
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
            _healthBar.SetCurrentResource(_characterStatsManager.GetCurrentHealth());
        }

        private void SetEnergy(int energy)
        {
            _energy = energy;
            _energyBar.SetSize((float)energy / 100);
            _energyBar.SetCurrentResource(energy);
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

        public void Attack(CharacterFight target, int damage, bool isCritical, bool dodged, int energyGained, Action onAttackComplete)
        {
            _characterManager.activeCharacter.SetSpriteSorting(CharacterSorting.Top);
            target._characterManager.activeCharacter.SetSpriteSorting(CharacterSorting.Default);
            var runningTargetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 6f;

            var startingPosition = GetPosition();
            

            //Slide to Target
            _characterManager.activeCharacter.RunToPosition(runningTargetPosition, () =>
            {
                // Arrived at target, attack him
                var attackDir = (target.GetPosition() - GetPosition()).normalized;
                _characterManager.activeCharacter.Attack(() =>
                {
                    //Target hit
                    target.Damage(damage, isCritical, dodged);
                    SetEnergy(_energy + energyGained);
                }, () =>
                {
                    if (!target.IsDead())
                    {
                        // Slide back to starting position
                        _characterManager.activeCharacter.RunToPosition(startingPosition, () =>
                        {
                            _characterManager.activeCharacter.PlayIdle();
                            _characterManager.activeCharacter.LookAt(target.GetPosition());
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
        
        public void Parry(CharacterFight target, int damage, bool isCritical, int energyGained, Action onAttackComplete)
        {
            var runningTargetPosition =  GetPosition() + (target.GetPosition() - GetPosition()).normalized * 6f;

            var startingPosition = target.GetPosition();
            
            var parryDir = (target.GetPosition() - GetPosition()).normalized;
            _characterManager.activeCharacter.Parry(() =>
            {
                //Slide to Target
                target._characterManager.activeCharacter.RunToPosition(runningTargetPosition, null);
            }, () =>
            {
                // Arrived at target, attack him
                var attackDir = (GetPosition() - target.GetPosition()).normalized;
                target._characterManager.activeCharacter.Attack(() =>
                {
                    //Target hit
                    Damage(damage, isCritical, false);
                    SetEnergy(_energy + energyGained);
                    _characterManager.activeCharacter.StopParry();
                }, () =>
                {
                    if (!IsDead())
                    {
                        // Slide back to starting position
                        target._characterManager.activeCharacter.RunToPosition(startingPosition, () =>
                        {
                            target._characterManager.activeCharacter.PlayIdle();
                            target._characterManager.activeCharacter.LookAt(GetPosition());
                            onAttackComplete?.Invoke();
                        });
                    }
                    else
                    {
                        _characterManager.activeCharacter.PlayLose();
                        onAttackComplete?.Invoke();
                    }
                });
            });
        }

        public void SpecialAttack(CharacterFight target, int damage, bool isCritical, bool dodged, Action onAttackComplete)
        {
            _characterManager.activeCharacter.SetSpriteSorting(CharacterSorting.Top);
            target._characterManager.activeCharacter.SetSpriteSorting(CharacterSorting.Default);
            
            SetEnergy(0);
            _characterManager.activeCharacter.SpecialAttack(target.transform, () =>
            {
                //Target hit
                target.Damage(damage, isCritical, dodged);
            }, () =>
            {
                if (!target.IsDead())
                {
                    _characterManager.activeCharacter.PlayIdle();
                    onAttackComplete?.Invoke();
                }
                else
                {
                    _characterManager.activeCharacter.PlayWin();
                    onAttackComplete?.Invoke();
                }
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

        public int GetEnergy()
        {
            return _energy;
        }
        
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