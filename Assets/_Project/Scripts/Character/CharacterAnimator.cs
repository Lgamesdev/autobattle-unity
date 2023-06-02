using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace LGamesDev
{
    public class CharacterAnimator : MonoBehaviour
    {
        PlayerStates CurrentState
        {
            set
            {
                if (!_stateLock)
                {
                    _currentState = value;

                    switch (_currentState)
                    {
                        case PlayerStates.IDLE:
                            _animator.Play("Idle");
                            _canMove = true;
                            break;
                        case PlayerStates.RUN:
                            _animator.Play("Run");
                            _canMove = true;
                            break;
                        case PlayerStates.ATTACK:
                            _animator.Play("Attack");
                            _stateLock = true;
                            _canMove = false;
                            break;
                        case PlayerStates.STRUCK:
                            _animator.Play("Struck");
                            _stateLock = true;
                            _canMove = false;
                            break;
                        case PlayerStates.DODGE:
                            _animator.Play("Dodge");
                            _stateLock = true;
                            _canMove = false;
                            break;
                        case PlayerStates.HAPPY:
                            _animator.Play("Happy");
                            _canMove = false;
                            break;
                        case PlayerStates.SAD:
                            _animator.Play("Sad");
                            _canMove = false;
                            break;
                    }
                }
            }
        }

        private bool _stateLock;
        private bool _canMove;
        private PlayerStates _currentState;

        [SerializeField] private AudioSource audioSource;

        private Animator _animator;

        [SerializeField] private AudioClip[] stepAudioClips;
        [SerializeField] private AudioClip[] punchAudioClips;
        [SerializeField] private AudioClip[] swordAudioClips;
        private int _stepClipIndex;
        private int _punchClipIndex;
        private int _swordClipIndex;

        //private static readonly int OnWalk = Animator.StringToHash("isWalking");
        //private static readonly int OnRun = Animator.StringToHash("isRunning");
        //private static readonly int HandAttack = Animator.StringToHash("Hand_Attack");
        //private static readonly int SwordAttack = Animator.StringToHash("Sword_Attack");
        private static readonly int IsParrying = Animator.StringToHash("isParrying");
        private static readonly int WeaponType = Animator.StringToHash("WeaponType");
        //private static readonly int OnHit = Animator.StringToHash("Attack_Hit");
        //private static readonly int OnDodge = Animator.StringToHash("Attack_Dodge");
        //private static readonly int OnWin = Animator.StringToHash("Win");
        //private static readonly int OnLose = Animator.StringToHash("Lose");

        [SerializeField] private VisualEffect handHitImpactFX;
        [SerializeField] private VisualEffect swordSlashFX;

        [SerializeField] private VisualEffect groundSlashFX;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            _stepClipIndex = Random.Range(0, stepAudioClips.Length - 1);
            _punchClipIndex = Random.Range(0, punchAudioClips.Length - 1);
            _swordClipIndex = Random.Range(0, swordAudioClips.Length - 1);
        }

        public void OnMove(Vector3 move)
        {
            Vector3 pos = transform.position;
            
            if (move != Vector3.zero) {
                CurrentState = PlayerStates.RUN;
                
                transform.right = new Vector3(move.x, pos.y, pos.z) - pos;
            } else {
                CurrentState = PlayerStates.IDLE;
            }
            //Debug.Log("Run animation");
            //_animator.SetBool(OnRun, true);
        }

        public void LookAt(Vector3 lookAt)
        {
            Transform tr = transform;
            Vector3 pos = tr.position;
            
            tr.right = new Vector3(lookAt.x, pos.y, pos.z) - pos;
        }
        
        /*public void StopRun()
        {
            _animator.SetBool(OnRun, false);
        }*/
        
        public void PlayStepAudio()
        {
            audioSource.PlayOneShot(stepAudioClips[_stepClipIndex]);

            if (_stepClipIndex < stepAudioClips.Length - 1)
            {
                _stepClipIndex++;
            }
            else
            {
                _stepClipIndex = 0;
            }
        }

        public void OnAttack(WeaponType weaponType)
        {
            _animator.SetFloat(WeaponType, (int)weaponType);
            CurrentState = PlayerStates.ATTACK;
            //Debug.Log("Hand attack animation");
            //_animator.SetTrigger(HandAttack);
        }
        
        public void OnAttackComplete()
        {
            _stateLock = false;
            CurrentState = PlayerStates.IDLE;
        }

        public void PlayHandHitAudio()
        {
            audioSource.PlayOneShot(punchAudioClips[_punchClipIndex]);

            if (_punchClipIndex < punchAudioClips.Length - 1)
            {
                _punchClipIndex++;
            }
            else
            {
                _punchClipIndex = 0;
            }
        }

        /*public void PlaySwordAttack()
        {
            //Debug.Log("Sword attack animation");
            _animator.SetTrigger(SwordAttack);
        }*/

        public void PlaySwordAudio()
        {
            audioSource.PlayOneShot(swordAudioClips[_swordClipIndex]);

            if (_swordClipIndex < swordAudioClips.Length - 1)
            {
                _swordClipIndex++;
            }
            else
            {
                _swordClipIndex = 0;
            }
        }

        public void OnParry(Action onStartParry)
        {
            CurrentState = PlayerStates.IDLE;
            LeanTween.value(0f, 1f, .45f).setOnUpdate(val =>
            {
                _animator.SetFloat(IsParrying, val);
            });

            onStartParry?.Invoke();
        }

        public void StopParrying()
        {
            CurrentState = PlayerStates.IDLE;
            LeanTween.value(1f, 0f, .45f).setOnUpdate(val =>
            {
                _animator.SetFloat(IsParrying, val);
            });
        }

        public void OnStruck()
        {
            CurrentState = PlayerStates.STRUCK;
            //_animator.SetTrigger(OnHit);
        }

        public void OnStruckFinished()
        {
            _stateLock = false;
            CurrentState = PlayerStates.IDLE;
        }

        public void OnDodge()
        {
            CurrentState = PlayerStates.DODGE;
            //_animator.SetTrigger(OnDodge);
        }

        public void OnDodgeFinished()
        {
            _stateLock = false;
            CurrentState = PlayerStates.IDLE;
        }

        public void OnWin()
        {
            CurrentState = PlayerStates.HAPPY;
            //_animator.SetBool(OnWin, true);
        }

        public void OnLose()
        {
            CurrentState = PlayerStates.SAD;
            //_animator.SetBool(OnLose, true);
        }

        public void CreateHandHitImpact(Vector3 position)
        {
            Instantiate(handHitImpactFX, new Vector3(position.x, position.y, position.z), Quaternion.identity);
        }

        public void CreateSwordSlash(Vector3 position)
        {
            Instantiate(swordSlashFX, new Vector3(position.x, position.y, position.z), Quaternion.identity);
        }

        public void CreateProjectile(Vector3 initialPosition, Transform target, Action onHit, Action onComplete)
        {
            VisualEffect projectile = Instantiate(groundSlashFX, initialPosition, Quaternion.identity);

            GroundSlash groundSlash = projectile.GetComponent<GroundSlash>();
            groundSlash.Setup(target, onHit, onComplete);
            
            RotateToDestination(projectile.gameObject, target.transform.position, true);
            projectile.GetComponent<Rigidbody>().velocity = transform.right * groundSlash.speed;
        }

        private void RotateToDestination(GameObject obj, Vector3 destination, bool onlyY)
        {
            Vector3 direction = destination - obj.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            if (!onlyY) return;
            
            rotation.x = 0;
            rotation.z = 0;

            //obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
        }

        /*public void SetMoveVector(Vector3 position)
        {
            transform.right = new Vector3(position.x, transform.position.y, transform.position.z) - transform.position;
        }*/
    }
    
    public enum PlayerStates
    {
        IDLE,
        RUN,
        ATTACK,
        STRUCK,
        DODGE,
        HAPPY,
        SAD
    }
}
