using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;

namespace LGamesDev
{
    public class CharacterHandler : MonoBehaviour
    {
        [Header("Hair")]
        public SpriteResolver hairResolver;
        [Header("EyeBrows")]
        public SpriteRenderer eyeBrowsRenderer;
        [Header("Moustache")]
        public SpriteResolver moustacheResolver;
        [Header("Beard")]
        public SpriteResolver beardResolver;
        [Header("Body")]
        public List<SpriteResolver> bodyResolvers;
        
        [Header("Helmet")]
        public SpriteResolver helmetResolver;
        [Header("Chest")]
        public List<SpriteResolver> chestResolvers;
        [Header("Pant")]
        public List<SpriteResolver> pantResolvers;
        
        [Header("Weapon")]
        public SpriteResolver weaponResolver;
        [Header("OffHand")]
        public SpriteResolver offHandResolver;

        private Body _body;

        private CharacterManager _characterManager;

        private CharacterAnimator _characterAnimator;
        //private State _state = State.Idle;
        public float movementSpeed = 10f;

        private Action _onAttackHit;
        private Action _onAttackComplete;

        private void OnEnable()
        {
            _characterManager = GetComponentInParent<CharacterManager>();
            _characterAnimator = GetComponent<CharacterAnimator>();
        }

        public IEnumerator SetupCharacter(Character character)
        {
            //Setup body
            _body = character.Body;

            List<string> labels = hairResolver.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(hairResolver.GetCategory()).ToList();
            hairResolver.SetCategoryAndLabel(hairResolver.GetCategory(), labels[_body.hairIndex]);
            ColorUtility.TryParseHtmlString(_body.hairColor, out Color hairColor);
            hairResolver.GetComponent<SpriteRenderer>().color = hairColor;

            eyeBrowsRenderer.color = hairColor;

            if (_body.isMaleGender)
            {
                labels = moustacheResolver.spriteLibrary.spriteLibraryAsset
                    .GetCategoryLabelNames(moustacheResolver.GetCategory()).ToList();
                
                moustacheResolver.SetCategoryAndLabel(moustacheResolver.GetCategory(),
                    labels[_body.moustacheIndex]);
                
                moustacheResolver.GetComponent<SpriteRenderer>().color = hairColor;
                
                labels = beardResolver.spriteLibrary.spriteLibraryAsset
                    .GetCategoryLabelNames(beardResolver.GetCategory()).ToList();
                
                beardResolver.SetCategoryAndLabel(beardResolver.GetCategory(),
                    labels[_body.beardIndex]);
                
                beardResolver.GetComponent<SpriteRenderer>().color = hairColor;
            }

            ColorUtility.TryParseHtmlString(_body.skinColor, out Color skinColor);
            foreach (SpriteResolver bodyResolver in bodyResolvers)
            {
                bodyResolver.GetComponent<SpriteRenderer>().color = skinColor;
                yield return new WaitForEndOfFrame();
            }
            
            if (character.Gear.equipments[(int)EquipmentSlot.Chest].item.spriteId == 0)
            {
                ColorUtility.TryParseHtmlString(_body.chestColor, out Color chestColor);
                foreach (SpriteResolver chestResolver in chestResolvers) {
                    chestResolver.GetComponent<SpriteRenderer>().color = chestColor;
                }
            }

            if (character.Gear.equipments[(int)EquipmentSlot.Pants].item.spriteId == 0)
            {
                ColorUtility.TryParseHtmlString(_body.shortColor, out Color pantColor);
                foreach (SpriteResolver pantResolver in pantResolvers) {
                    pantResolver.GetComponent<SpriteRenderer>().color = pantColor;
                }
            }

            foreach (CharacterEquipment characterEquipment in character.Gear.equipments) {
                if (characterEquipment.item != null)
                {
                    //Debug.Log("equipment : " + characterEquipment.ToString());
                    if (!characterEquipment.item.isDefaultItem)
                    {
                        UpdateEquipmentTexture(characterEquipment, null);
                    }
                    else
                    {
                        UpdateEquipmentTexture(null, characterEquipment);
                    }
                }
                
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
        }

        public void UpdateEquipmentTexture(CharacterEquipment newEquipment, CharacterEquipment oldEquipment)
        {
            List<string> labels;
            if (newEquipment != null)
            {
                // Equip Item
                switch (newEquipment.item.equipmentSlot)
                {
                    case EquipmentSlot.Helmet:
                        if (_body.isMaleGender)
                        {
                            labels = hairResolver.spriteLibrary.spriteLibraryAsset
                                .GetCategoryLabelNames(hairResolver.GetCategory()).ToList();
                            hairResolver.SetCategoryAndLabel(hairResolver.GetCategory(), labels[0]);
                        }

                        labels = helmetResolver.spriteLibrary.spriteLibraryAsset
                            .GetCategoryLabelNames(helmetResolver.GetCategory()).ToList();
                        helmetResolver.SetCategoryAndLabel(helmetResolver.GetCategory(), labels[newEquipment.item.spriteId]);
                        break;
                    
                    case EquipmentSlot.Chest:
                        foreach (SpriteResolver resolver in chestResolvers) {
                            resolver.GetComponent<SpriteRenderer>().color = Color.white;
                            
                            labels = resolver.spriteLibrary.spriteLibraryAsset
                                .GetCategoryLabelNames(resolver.GetCategory()).ToList();
                            resolver.SetCategoryAndLabel(resolver.GetCategory(), labels[newEquipment.item.spriteId]);
                        }
                        break;
                    
                    case EquipmentSlot.Pants:
                        foreach (SpriteResolver resolver in pantResolvers)
                        {
                            resolver.GetComponent<SpriteRenderer>().color = Color.white;
                        
                            labels = resolver.spriteLibrary.spriteLibraryAsset
                                .GetCategoryLabelNames(resolver.GetCategory()).ToList();
                            resolver.SetCategoryAndLabel(resolver.GetCategory(), labels[newEquipment.item.spriteId]);
                        }
                        break;
                    
                    case EquipmentSlot.Weapon:
                        labels = weaponResolver.spriteLibrary.spriteLibraryAsset
                            .GetCategoryLabelNames(weaponResolver.GetCategory()).ToList();
                        weaponResolver.SetCategoryAndLabel(weaponResolver.GetCategory(), labels[newEquipment.item.spriteId]);
                        break;
                    
                    /*case EquipmentSlot.Shield:
                    break;*/
                }
            } 
            else if (oldEquipment != null) 
            {
                // Unequip Item
                switch (oldEquipment.item.equipmentSlot)
                {
                    case EquipmentSlot.Helmet:
                        labels = helmetResolver.spriteLibrary.spriteLibraryAsset
                            .GetCategoryLabelNames(helmetResolver.GetCategory()).ToList();
                        helmetResolver.SetCategoryAndLabel(helmetResolver.GetCategory(), labels[0]);
                        break;

                    case EquipmentSlot.Chest:
                        ColorUtility.TryParseHtmlString(_body.chestColor, out Color chestColor);
                        
                        foreach (SpriteResolver resolver in chestResolvers) {
                            labels = resolver.spriteLibrary.spriteLibraryAsset
                                .GetCategoryLabelNames(resolver.GetCategory()).ToList();
                            resolver.SetCategoryAndLabel(resolver.GetCategory(), labels[0]);
                            
                            resolver.GetComponent<SpriteRenderer>().color = chestColor;
                        }
                        break;
                    
                    case EquipmentSlot.Pants:
                        ColorUtility.TryParseHtmlString(_body.shortColor, out Color pantColor);

                        foreach (SpriteResolver resolver in pantResolvers)
                        {
                            labels = resolver.spriteLibrary.spriteLibraryAsset
                                .GetCategoryLabelNames(resolver.GetCategory()).ToList();
                            resolver.SetCategoryAndLabel(resolver.GetCategory(), labels[0]);
                        
                            resolver.GetComponent<SpriteRenderer>().color = pantColor;
                        }
                        break;
                    
                    case EquipmentSlot.Weapon:
                        labels = weaponResolver.spriteLibrary.spriteLibraryAsset
                            .GetCategoryLabelNames(weaponResolver.GetCategory()).ToList();
                        weaponResolver.SetCategoryAndLabel(weaponResolver.GetCategory(), labels[0]);
                        break;
                    
                    /*case EquipmentSlot.Shield:
                    break;*/
                }
            }
        }

        public void RunToPosition(Vector3 runTargetPosition, Action onRunComplete)
        {
            _characterAnimator.PlayRun();
            _characterAnimator.SetMoveVector(runTargetPosition);

            LTDescr runAnim = LeanTween.move(_characterManager.gameObject, runTargetPosition, .8f);

            if (onRunComplete != null)
            {
                runAnim.setOnComplete(() =>
                {
                    //_state = State.Idle;
                    _characterAnimator.StopRun();
                    onRunComplete?.Invoke();
                });
            }
        }
        
        public void Attack(Vector3 attackDir, Action onHit, Action onComplete) {
            //_state = State.Busy;
            if (_characterManager.equipmentManager.GotWeapon())
            {
                _characterAnimator.PlaySwordAttack();
            }
            else
            {
                _characterAnimator.PlayHandAttack();
            }

            _onAttackHit = onHit;
            _onAttackComplete = onComplete;
        }

        public void OnAttackHit()
        {
            _onAttackHit?.Invoke();
        }

        public void OnAttackComplete()
        {
            _onAttackComplete?.Invoke();
        }
        
        public void PlayAnimIdle()
        {
            _characterAnimator.SetMoveVector(Vector3.zero);
        }

        /*public void PlayAnimMove(Vector3 moveDir) {
           if (moveVector == Vector3.zero) {
               // Idle
               unitAnimation.PlayAnim(idleAnimType, lastMoveDir, idleFrameRate, null, null, null);
           } else {
               // Moving
               lastMoveDir = moveDir;
               unitAnimation.PlayAnim(walkAnimType, lastMoveDir, walkFrameRate, null, null, null);
           }
       }*/
        
        private enum State
        {
            Idle,
            Running,
            Walking,
            Busy
        }
    }
}