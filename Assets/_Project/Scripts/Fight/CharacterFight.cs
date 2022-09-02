using System;
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

        private HealthBar healthBar;
        private LevelSystem levelSystem;
        private Action onSlideComplete;
        private Action onWalkComplete;
        private PlayerStats playerStats;
        private GameObject selectionCircleGameObject;
        private Vector3 slideTargetPosition;

        private State state;
        private UnitHandler unitHandler;

        private Vector3 walkTargetPosition;

        private void Awake()
        {
            unitHandler = GetComponent<UnitHandler>();
            healthBar = transform.Find("HealthBar").gameObject.GetComponent<HealthBar>();

            playerStats = GetComponent<PlayerStats>();
            playerStats.OnHealthChanged += PlayerStats_OnHealthChanged;

            selectionCircleGameObject = transform.Find("SelectionCircle").gameObject;
            HideSelectionCircle();

            state = State.Idle;
        }

        private void Start()
        {
            var level = PlayerPrefs.GetInt("level", 0);

            if (isPlayerTeam)
            {
                var experience = PlayerPrefs.GetFloat("experience", 0);

                unitHandler.SetAnimsSwordTwoHandedBack();

                unitHandler.SetGuestSpriteSheetData(GuestSpritesheetData.Load_Static());
                unitHandler.SetGuestSpriteSheetTexture();

                unitHandler.SetCharacterEquipment(CharacterEquipmentManager.Instance);
                unitHandler.SetupTexture();
                //unitHandler.GetMaterial().mainTexture = BattleHandler.GetInstance().playerSpritesheet;

                levelSystem = new LevelSystem(level, experience);
                //Debug.Log("trying to setlevelsystem by " + transform.name);

                EarningWindow.Instance.SetLevelSystem(levelSystem);
            }
            else
            {
                //TODO
                unitHandler.SetAnimsSwordShield();
                unitHandler.GetMaterial().mainTexture = FightManager.Instance.enemySpritesheet;

                levelSystem = new LevelSystem(level, 0);
            }

            PlayAnimIdle();
        }

        private void Update()
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Busy:
                    break;
                case State.Sliding:
                    transform.position += (slideTargetPosition - GetPosition()) * Time.deltaTime * movementSpeed;

                    var reachedDistance = 0.1f;
                    if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance)
                    {
                        //Arrived at slide target position
                        transform.position = slideTargetPosition;
                        onSlideComplete();
                    }

                    break;
                case State.Walking:
                    //TODO : fix intro duration
                    transform.position =
                        new Vector3(Mathf.Lerp(transform.position.x, walkTargetPosition.x, introDuration * Time.deltaTime),
                            walkTargetPosition.y);

                    reachedDistance = 0.1f;
                    if (Vector3.Distance(GetPosition(), walkTargetPosition) < reachedDistance)
                    {
                        //Arrived at inital fight position
                        transform.position = walkTargetPosition;
                        onWalkComplete();
                    }

                    break;
            }
        }

        private void PlayerStats_OnHealthChanged(object sender, EventArgs e)
        {
            healthBar.SetSize(playerStats.GetHealthPercent());
        }

        private void PlayAnimIdle()
        {
            if (isPlayerTeam)
                unitHandler.PlayAnimIdle(new Vector3(1, 0));
            else
                unitHandler.PlayAnimIdle(new Vector3(-1, 0));
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void Damage(int damageAmount, bool isCritical)
        {
            playerStats.TakeDamage(damageAmount);

            DamagePopup.Create(GetPosition(), damageAmount, isCritical);
            //TODO:  Impact effect

            //CodeMonkey.Utils.UtilsClass.ShakeCamera(.7f, .07f);

            if (IsDead())
                //Died
                unitHandler.PlayAnimLyingUp();
        }

        public bool IsDead()
        {
            return playerStats.IsDead();
        }

        public void Attack(CharacterFight target, Action onAttackComplete)
        {
            var slideTargetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 12f;

            var startingPosition = GetPosition();

            //Slide to Target
            SlideToPosition(slideTargetPosition, () =>
            {
                // Arrived at target, attack him
                state = State.Busy;
                var attackDir = (target.GetPosition() - GetPosition()).normalized;
                unitHandler.PlayAnimAttack(attackDir, () =>
                {
                    //Target hit
                    var damageAmount = Mathf.RoundToInt(Random.Range(playerStats.GetStat(StatType.Damage) * 0.8f,
                        playerStats.GetStat(StatType.Damage) * 1.2f));
                    if (!isPlayerTeam)
                        damageAmount = Mathf.RoundToInt(Random.Range(playerStats.GetStat(StatType.Damage) * 0.7f,
                            playerStats.GetStat(StatType.Damage) * 1.1f));
                    if (Random.value < (float)playerStats.GetStat(StatType.Critical) / 100)
                        target.Damage(damageAmount * 2, true);
                    else
                        target.Damage(damageAmount, false);
                }, () =>
                {
                    // Slide back to starting position
                    SlideToPosition(startingPosition, () =>
                    {
                        state = State.Idle;
                        PlayAnimIdle();
                        onAttackComplete();
                    });
                });
            });
        }

        public void Intro(CharacterFight target, Action onIntroComplete)
        {
            var walkTargetPosition = new Vector3(transform.position.x, transform.position.y);
            var targetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 12f;
            transform.position -= new Vector3(transform.position.x + targetPosition.x, 0);

            this.walkTargetPosition = walkTargetPosition;
            onWalkComplete = onIntroComplete;
            state = State.Walking;
            unitHandler.PlayAnimMove(walkTargetPosition);
        }

        private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
        {
            this.slideTargetPosition = slideTargetPosition;
            this.onSlideComplete = onSlideComplete;
            state = State.Sliding;
            if (slideTargetPosition.x - transform.position.x > 0)
                unitHandler.PlayAnimSlideRight();
            else
                unitHandler.PlayAnimSlideLeft();
        }

        public void HideSelectionCircle()
        {
            selectionCircleGameObject.SetActive(false);
        }

        public void ShowSelectionCircle()
        {
            selectionCircleGameObject.SetActive(true);
        }

        public LevelSystem GetLevelSystem()
        {
            return levelSystem;
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