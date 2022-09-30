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

        public float movementSpeed = 10f;

        private HealthBar _healthBar;
        private LevelSystem _levelSystem;
        private Action _onSlideComplete;
        private Action _onWalkComplete;
        private CharacterStatsManager _characterStatsManager;
        private GameObject _selectionCircleGameObject;
        private Vector3 _slideTargetPosition;

        private State _state = State.Idle;
        private CharacterHandler _characterHandler;

        private Vector3 _walkTargetPosition;

        private void Awake()
        {
            _characterHandler = GetComponent<CharacterHandler>();

            _characterStatsManager = GetComponent<CharacterStatsManager>();
            _characterStatsManager.OnHealthChanged += CharacterStatsManagerOnHealthChanged;

            //selectionCircleGameObject = transform.Find("SelectionCircle").gameObject;
            //HideSelectionCircle();

            _state = State.Idle;
        }

        public IEnumerator SetupCharacterFight(Character character)
        {
            yield return StartCoroutine(_characterHandler.SetupCharacter(character));
            
            _levelSystem = new LevelSystem(character.Level, character.Experience);
            
            if (isPlayerTeam)
            {
                _healthBar = FindObjectOfType<HealthPannelUI>().playerHealthBar;
                /*_characterHandler.SetAnimsSwordTwoHandedBack();

                _characterHandler.SetGuestSpriteSheetData(GuestSpritesheetData.Load_Static());
                _characterHandler.SetGuestSpriteSheetTexture();

                _characterHandler.SetCharacterEquipment(CharacterEquipmentManager.Instance);*/
                
                //unitHandler.GetMaterial().mainTexture = BattleHandler.GetInstance().playerSpritesheet;
                
                RewardUI.Instance.SetLevelSystem(_levelSystem);
                
                /*string log = "character stats : [ \n";
                foreach (Stat stat in _characterHandler.statsManager.stats) log += stat.ToString() + "\n";
                Debug.Log(log + "\n ]");*/
            }
            else
            {
                /*string log = "opponent stats : [ \n";
                foreach (Stat stat in _characterHandler.statsManager.stats) log += stat.ToString() + "\n";
                Debug.Log(log + "\n ]");*/
                
                _healthBar = FindObjectOfType<HealthPannelUI>().opponentHealthBar;
                
                //TODO
                /*_characterHandler.SetAnimsSwordShield();
                _characterHandler.GetMaterial().mainTexture = FightManager.Instance.enemySpritesheet;*/
            }
            
            _healthBar.SetMaxHealth(_characterStatsManager.GetMaxHealth());
            _healthBar.SetCurrentHealth(_characterStatsManager.GetCurrentHealth());

            //PlayAnimIdle();
        }
        
        public void Intro(CharacterFight target, Action onIntroComplete)
        {
            var position = transform.position;
            
            var walkTargetPosition = new Vector3(position.x, position.y);
            var targetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 12f;
            position -= new Vector3(position.x + targetPosition.x, 0);
            transform.position = position;

            _walkTargetPosition = walkTargetPosition;
            _onWalkComplete = onIntroComplete;
            _state = State.Walking;
            _characterHandler.PlayAnimMove(walkTargetPosition);
        }

        private void Update()
        {
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Busy:
                    break;
                case State.Sliding:
                    transform.position += (_slideTargetPosition - GetPosition()) * (Time.deltaTime * movementSpeed);

                    var reachedDistance = 0.1f;
                    if (Vector3.Distance(GetPosition(), _slideTargetPosition) < reachedDistance)
                    {
                        //Arrived at slide target position
                        transform.position = _slideTargetPosition;
                        _onSlideComplete();
                    }

                    break;
                case State.Walking:
                    //TODO : fix intro duration
                    transform.position =
                        new Vector3(Mathf.Lerp(transform.position.x, _walkTargetPosition.x, introDuration * Time.deltaTime),
                            _walkTargetPosition.y);

                    reachedDistance = 0.1f;
                    if (Vector3.Distance(GetPosition(), _walkTargetPosition) < reachedDistance)
                    {
                        //Arrived at initial fight position
                        transform.position = _walkTargetPosition;
                        _onWalkComplete();
                    }

                    break;
            }
        }

        private void CharacterStatsManagerOnHealthChanged(object sender, EventArgs e)
        {
            _healthBar.SetSize(_characterStatsManager.GetHealthPercent());
            _healthBar.SetCurrentHealth(_characterStatsManager.GetCurrentHealth());
        }

        private void PlayAnimIdle()
        {
            if (isPlayerTeam)
            {
                //_characterHandler.PlayAnimIdle(new Vector3(1, 0));
            } else {
                //_characterHandler.PlayAnimIdle(new Vector3(-1, 0));
            }
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
            var slideTargetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 18f;

            var startingPosition = GetPosition();

            //Slide to Target
            SlideToPosition(slideTargetPosition, () =>
            {
                // Arrived at target, attack him
                _state = State.Busy;
                var attackDir = (target.GetPosition() - GetPosition()).normalized;
                _characterHandler.PlayAnimAttack(attackDir, () =>
                {
                    //Target hit
                    target.Damage(damage, isCritical);
                }, () =>
                {
                    // Slide back to starting position
                    SlideToPosition(startingPosition, () =>
                    {
                        _state = State.Idle;
                        PlayAnimIdle();
                        onAttackComplete();
                    });
                });
            });
        }

        private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
        {
            this._slideTargetPosition = slideTargetPosition;
            this._onSlideComplete = onSlideComplete;
            _state = State.Sliding;
            if (slideTargetPosition.x - transform.position.x > 0)
            {
               // _characterHandler.PlayAnimSlideRight();
            }
            else
            {
                //_characterHandler.PlayAnimSlideLeft();
            }
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

        private enum State
        {
            Idle,
            Sliding,
            Walking,
            Busy
        }
    }
}