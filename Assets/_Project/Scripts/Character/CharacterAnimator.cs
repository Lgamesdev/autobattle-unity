using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    private Animator _animator;

    [SerializeField] private AudioClip[] stepAudioClips;
    [SerializeField] private AudioClip[] punchAudioClips;
    [SerializeField] private AudioClip[] swordAudioClips;
    private int _stepClipIndex;
    private int _punchClipIndex;
    private int _swordClipIndex;
    
    private static readonly int OnWalk = Animator.StringToHash("isWalking");
    private static readonly int OnRun = Animator.StringToHash("isRunning");
    private static readonly int OnHandHit = Animator.StringToHash("Hand_Attack");
    private static readonly int OnSwordHit = Animator.StringToHash("Sword_Attack");
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        _stepClipIndex = Random.Range(0, stepAudioClips.Length - 1);
        _punchClipIndex = Random.Range(0, punchAudioClips.Length - 1);
        _swordClipIndex = Random.Range(0, swordAudioClips.Length - 1);
    }

    /*public void PlayIdle()
    {
        Debug.Log("Idle animation");
        _animator.SetTrigger(OnIdle);
    }*/

    public void PlayWalk()
    {
        //Debug.Log("Walk animation");
        _animator.SetBool(OnWalk, true);
    }

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

    public void PlayRun()
    {
        //Debug.Log("Run animation");
        _animator.SetBool(OnRun, true);
    }
    
    public void StopRun()
    {
        _animator.SetBool(OnRun, false);
    }

    public void PlayHandAttack()
    {
        //Debug.Log("Hand attack animation");
        _animator.SetTrigger(OnHandHit);
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
    
    public void PlaySwordAttack()
    {
        //Debug.Log("Sword attack animation");
        _animator.SetTrigger(OnSwordHit);
    }

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

    public void SetMoveVector(Vector3 position)
    {
        transform.right = new Vector3(position.x, transform.position.y, position.z) - transform.position;
    }
}
