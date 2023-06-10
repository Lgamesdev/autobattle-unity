using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D.Animation;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

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
        [Header("Face Resolvers")]
        public List<SpriteResolver> faceResolvers;
        [Header("Body")]
        public List<SpriteResolver> bodyResolvers;
        
        [Header("Helmet")]
        public SpriteResolver helmetResolver;
        [Header("Chest")]
        public List<SpriteResolver> chestResolvers;
        [Header("Pant")]
        public List<SpriteResolver> pantResolvers;
        
        [Header("HandL")]
        public Transform leftHandBone;
        [Header("HandR")]
        public Transform rightHandBone;
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
        
        private bool _isParrying;
        private Action _onParryReady;

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
            
            if (character.Gear.equipments[(int)EquipmentSlot.Chest] == null)
            {
                ColorUtility.TryParseHtmlString(_body.chestColor, out Color chestColor);
                foreach (SpriteResolver chestResolver in chestResolvers) {
                    chestResolver.GetComponent<SpriteRenderer>().color = chestColor;
                }
            }

            if (character.Gear.equipments[(int)EquipmentSlot.Pants] == null)
            {
                ColorUtility.TryParseHtmlString(_body.shortColor, out Color pantColor);
                foreach (SpriteResolver pantResolver in pantResolvers) {
                    pantResolver.GetComponent<SpriteRenderer>().color = pantColor;
                }
            }

            foreach (CharacterEquipment characterEquipment in character.Gear.equipments) {
                if (characterEquipment != null)
                {
                    //Debug.Log("equipment : " + characterEquipment.ToString());
                    if (characterEquipment.item != null)
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
                // TryEquip Item
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
            _characterAnimator.OnMove(runTargetPosition);
            //_characterAnimator.SetMoveVector(runTargetPosition);

            LTDescr runAnim = LeanTween.move(_characterManager.gameObject, runTargetPosition, .8f).setOnComplete(() =>
            {
                onRunComplete?.Invoke();
            });
        }

        public void Attack(/*Vector3 attackDir,*/ Action onHit, Action onComplete) {
            _onAttackHit = onHit;
            _onAttackComplete = onComplete;
            
            _characterAnimator.OnAttack(_characterManager.equipmentManager.GetWeaponType());
        }
        
        public void OnAttackHit()
        {
            _onAttackHit?.Invoke();

            switch (_characterManager.equipmentManager.GetWeaponType())
            {
                case WeaponType.Hand:
                    _characterAnimator.CreateHandHitImpact(leftHandBone.transform.position);
                    break;
                case WeaponType.Sword:
                    _characterAnimator.CreateSwordSlash(rightHandBone.transform.position);
                    break;
            }
        }

        public void OnAttackFinished()
        {
            _characterAnimator.OnAttackComplete();
            _onAttackComplete?.Invoke();
        }

        public void Parry(Action onStart, Action onParryReady)
        {
            _onParryReady = onParryReady;
            
            _characterAnimator.OnParry(onStart);
        }
        
        public void OnParryReady()
        {
            if (_isParrying) return;
            
            _onParryReady?.Invoke();
            _isParrying = true;
        }

        public void StopParry()
        {
            _isParrying = false;
            _characterAnimator.StopParrying();
        }

        public void SpecialAttack(Transform target, Action onHit, Action onComplete)
        {
            //Debug.Log("special attack");
            _characterAnimator.CreateProjectile(transform.parent.position, target, onHit, onComplete);
        }

        public void Dodge()
        {
            _characterAnimator.OnDodge();
        }

        public void TakeHit()
        {
            _characterAnimator.OnStruck();
        }

        public void PlayIdle()
        {
            _characterAnimator.OnMove(Vector3.zero);
        }

        public void LookAt(Vector3 position)
        {
            _characterAnimator.LookAt(position);
        }

        public void PlayWin()
        {
            _characterAnimator.OnWin();
        }
        
        public void PlayLose()
        {
            _characterAnimator.OnLose();
        }

        public void SetSpriteSorting(CharacterSorting characterSorting)
        {
            hairResolver.GetComponent<SpriteRenderer>().sortingLayerName = characterSorting.ToString();
            eyeBrowsRenderer.GetComponent<SpriteRenderer>().sortingLayerName = characterSorting.ToString();
            
            foreach (SpriteResolver spriteResolver in faceResolvers)
            {
                spriteResolver.GetComponent<SpriteRenderer>().sortingLayerName = characterSorting.ToString();
            }

            if (_body.isMaleGender)
            {
                moustacheResolver.GetComponent<SpriteRenderer>().sortingLayerName = characterSorting.ToString();
                beardResolver.GetComponent<SpriteRenderer>().sortingLayerName = characterSorting.ToString();
            }

            foreach (SpriteResolver spriteResolver in bodyResolvers)
            {
                spriteResolver.GetComponent<SpriteRenderer>().sortingLayerName = characterSorting.ToString();
            }

            helmetResolver.GetComponent<SpriteRenderer>().sortingLayerName = characterSorting.ToString();

            foreach (SpriteResolver spriteResolver in chestResolvers)
            {
                spriteResolver.GetComponent<SpriteRenderer>().sortingLayerName = characterSorting.ToString();
            }

            foreach (SpriteResolver spriteResolver in pantResolvers)
            {
                spriteResolver.GetComponent<SpriteRenderer>().sortingLayerName = characterSorting.ToString();
            }

            weaponResolver.GetComponent<SpriteRenderer>().sortingLayerName = characterSorting.ToString();
            //offHandResolver.GetComponent<SpriteRenderer>().sortingLayerName = characterSorting.ToString();
        }

        private enum State
        {
            Idle,
            Running,
            Walking,
            Busy
        }
    }
    
    public enum CharacterSorting
    {
        Top,
        Default
    }
}