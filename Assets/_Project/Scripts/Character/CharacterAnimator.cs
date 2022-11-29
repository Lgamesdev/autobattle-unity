using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private AudioClip walkAudioClip;
    [SerializeField] private AudioClip runAudioClip;
    [SerializeField] private AudioClip handAttackAudioClip;
    [SerializeField] private AudioClip swordAttackAudioClip;
    
    private static readonly int OnWalk = Animator.StringToHash("isWalking");
    private static readonly int OnRun = Animator.StringToHash("isRunning");
    private static readonly int OnHandHit = Animator.StringToHash("Hand_Attack");
    private static readonly int OnSwordHit = Animator.StringToHash("Sword_Attack");
    
    private Animator _animator;
    private AudioSource _audioSource;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
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

    public void PlayWalkAudio()
    {
        _audioSource.clip = walkAudioClip;
        _audioSource.Play();
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

    public void PlayRunAudio()
    {
        _audioSource.clip = runAudioClip;
        _audioSource.Play();
    }
    
    public void PlayHandAttack()
    {
        //Debug.Log("Hand attack animation");
        _animator.SetTrigger(OnHandHit);
    }
    
    public void PlayHandHitAudio()
    {
        _audioSource.clip = handAttackAudioClip;
        _audioSource.Play();
    }
    
    public void PlaySwordAttack()
    {
        //Debug.Log("Sword attack animation");
        _animator.SetTrigger(OnSwordHit);
    }

    public void PlaySwordAudio()
    {
        _audioSource.clip = swordAttackAudioClip;
        _audioSource.Play();
    }

    public void SetMoveVector(Vector3 position)
    {
        transform.right = new Vector3(position.x, transform.position.y, position.z) - transform.position;
    }
}
