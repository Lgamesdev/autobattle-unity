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
        [Header("Belt")]
        public SpriteResolver beltResolver;
        [Header("Pant")]
        public SpriteResolver pantResolver;
        
        [Header("Weapon")]
        public SpriteResolver weaponResolver;
        [Header("OffHand")]
        public SpriteResolver offHandResolver;

        private Animator _animator;
        private int _walkHash = Animator.StringToHash("Walk");
        private int _runHash = Animator.StringToHash("Run");
        private int _attackHash = Animator.StringToHash("Attack");
        
        public CharacterEquipmentManager equipmentManager;
        public CharacterStatsManager statsManager;
        
        public readonly Character Character = new();

        public static CharacterHandler Instance;

        private void Awake()
        {
            Instance = this;

            _animator = GetComponent<Animator>();
        }

        public IEnumerator SetupCharacter(Character character)
        {
            //Setup Equipments
            foreach (EquipmentType equipmentSlot in (EquipmentType[]) Enum.GetValues(typeof(EquipmentType)))
            {
                Equipment defaultEquipment = new Equipment() { equipmentType = equipmentSlot, isDefaultItem = true };
                        
                Character.Equipments[(int)equipmentSlot] = new CharacterEquipment()
                {
                    equipment = defaultEquipment
                };
            }
            
            foreach (CharacterEquipment characterEquipment in character.Equipments)
            {
                Character.Equipments[(int)characterEquipment.equipment.equipmentType] = characterEquipment;
            }
            
            //Setup Stats
            foreach (StatType statType in (StatType[])Enum.GetValues(typeof(StatType)))
            {
                Character.Stats[(int)statType] = new Stat() { statType = statType };
            }
            
            foreach (Stat stat in character.Stats)
            {
                Character.Stats[(int)stat.statType] = stat;
            }
            
            //Setup Equipment Manager
            equipmentManager = GetComponent<CharacterEquipmentManager>();
            equipmentManager.SetupManager(Character.Equipments);
            equipmentManager.OnEquipmentChanged += UpdateEquipmentTexture;

            //Setup Stat Manager
            statsManager = GetComponent<CharacterStatsManager>();
            statsManager.SetupManager(Character.Stats);

            //Setup Wallet/Wallet Manager
            Character.Wallet = character.Wallet;
            GetComponent<PlayerWalletManager>()?.SetupManager(Character.Wallet);
            
            //Setup Inventory/Inventory Manager
            Character.Inventory = character.Inventory;
            GetComponent<PlayerInventoryManager>()?.SetupManager(Character.Inventory);
            
            //Setup body
            Character.Body = character.Body;
            Body body = Character.Body;

            GetComponent<SpriteLibManager>().SwitchLibrary(body.isMaleGender ? SpriteLib.Male : SpriteLib.Female);
            
            List<string> labels = hairResolver.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(hairResolver.GetCategory()).ToList();
            hairResolver.SetCategoryAndLabel(hairResolver.GetCategory(), labels[body.hairIndex]);
            ColorUtility.TryParseHtmlString(body.hairColor, out Color hairColor);
            hairResolver.GetComponent<SpriteRenderer>().color = hairColor;

            eyeBrowsRenderer.color = hairColor;

            labels = moustacheResolver.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(moustacheResolver.GetCategory()).ToList();
            moustacheResolver.SetCategoryAndLabel(moustacheResolver.GetCategory(), labels[body.isMaleGender ? body.moustacheIndex : 0]);
            moustacheResolver.GetComponent<SpriteRenderer>().color = hairColor;

            labels = beardResolver.spriteLibrary.spriteLibraryAsset.GetCategoryLabelNames(beardResolver.GetCategory()).ToList();
            beardResolver.SetCategoryAndLabel(beardResolver.GetCategory(), labels[body.isMaleGender ? body.beardIndex : 0]);
            beardResolver.GetComponent<SpriteRenderer>().color = hairColor;

            ColorUtility.TryParseHtmlString(body.skinColor, out Color skinColor);
            foreach (SpriteResolver bodyResolver in bodyResolvers)
            {
                bodyResolver.GetComponent<SpriteRenderer>().color = skinColor;
            }
            
            if (Character.Equipments[(int)EquipmentType.Chest].equipment.spriteId == 0)
            {
                ColorUtility.TryParseHtmlString(body.chestColor, out Color chestColor);
                foreach (SpriteResolver chestResolver in chestResolvers) {
                    chestResolver.GetComponent<SpriteRenderer>().color = chestColor;
                }
            }

            ColorUtility.TryParseHtmlString(body.beltColor, out Color beltColor);
            beltResolver.GetComponent<SpriteRenderer>().color = beltColor;

            if (Character.Equipments[(int)EquipmentType.Pants].equipment.spriteId == 0)
            {
                ColorUtility.TryParseHtmlString(body.shortColor, out Color shortColor);
                pantResolver.GetComponent<SpriteRenderer>().color = shortColor;
            }

            foreach (CharacterEquipment characterEquipment in Character.Equipments) {
                if (characterEquipment.equipment != null)
                {
                    //Debug.Log("equipment : " + characterEquipment.ToString());
                    UpdateEquipmentTexture(characterEquipment.equipment, null);
                }
            }

            yield return new WaitForEndOfFrame();
        }

        private void UpdateEquipmentTexture(Equipment newEquipment, Equipment oldEquipment)
        {
            List<string> labels;
            if (newEquipment != null)
            {
                // Equip Item
                switch (newEquipment.equipmentType)
                {
                    case EquipmentType.Helmet:
                        if (Character.Body.isMaleGender)
                        {
                            labels = hairResolver.spriteLibrary.spriteLibraryAsset
                                .GetCategoryLabelNames(hairResolver.GetCategory()).ToList();
                            hairResolver.SetCategoryAndLabel(hairResolver.GetCategory(), labels[0]);
                        }

                        labels = helmetResolver.spriteLibrary.spriteLibraryAsset
                            .GetCategoryLabelNames(helmetResolver.GetCategory()).ToList();
                        helmetResolver.SetCategoryAndLabel(helmetResolver.GetCategory(), labels[newEquipment.spriteId]);
                        break;
                    
                    case EquipmentType.Chest:
                        foreach (SpriteResolver resolver in chestResolvers) {
                            resolver.GetComponent<SpriteRenderer>().color = Color.white;
                            
                            labels = resolver.spriteLibrary.spriteLibraryAsset
                                .GetCategoryLabelNames(resolver.GetCategory()).ToList();
                            resolver.SetCategoryAndLabel(resolver.GetCategory(), labels[newEquipment.spriteId]);
                        }
                        break;
                    
                    case EquipmentType.Pants:
                        pantResolver.GetComponent<SpriteRenderer>().color = Color.white;
                        
                        labels = pantResolver.spriteLibrary.spriteLibraryAsset
                            .GetCategoryLabelNames(pantResolver.GetCategory()).ToList();
                        pantResolver.SetCategoryAndLabel(pantResolver.GetCategory(), labels[newEquipment.spriteId]);
                        break;
                    
                    case EquipmentType.Weapon:
                        labels = weaponResolver.spriteLibrary.spriteLibraryAsset
                            .GetCategoryLabelNames(weaponResolver.GetCategory()).ToList();
                        weaponResolver.SetCategoryAndLabel(weaponResolver.GetCategory(), labels[newEquipment.spriteId]);
                        break;
                    
                    /*case EquipmentSlot.Shield:
                    break;*/
                }
            } 
            else if (oldEquipment != null) 
            {
                // Unequip Item
                switch (oldEquipment.equipmentType)
                {
                    case EquipmentType.Helmet:
                        labels = helmetResolver.spriteLibrary.spriteLibraryAsset
                            .GetCategoryLabelNames(helmetResolver.GetCategory()).ToList();
                        helmetResolver.SetCategoryAndLabel(helmetResolver.GetCategory(), labels[0]);
                        break;

                    case EquipmentType.Chest:
                        ColorUtility.TryParseHtmlString(Character.Body.chestColor, out Color chestColor);
                        
                        foreach (SpriteResolver resolver in chestResolvers) {
                            labels = resolver.spriteLibrary.spriteLibraryAsset
                                .GetCategoryLabelNames(resolver.GetCategory()).ToList();
                            resolver.SetCategoryAndLabel(resolver.GetCategory(), labels[0]);
                            
                            resolver.GetComponent<SpriteRenderer>().color = chestColor;
                        }
                        break;
                    
                    case EquipmentType.Pants:
                        labels = pantResolver.spriteLibrary.spriteLibraryAsset
                            .GetCategoryLabelNames(pantResolver.GetCategory()).ToList();
                        pantResolver.SetCategoryAndLabel(pantResolver.GetCategory(), labels[0]);
                        
                        ColorUtility.TryParseHtmlString(Character.Body.shortColor, out Color shortColor);
                        pantResolver.GetComponent<SpriteRenderer>().color = shortColor;
                        break;
                    
                    case EquipmentType.Weapon:
                        labels = weaponResolver.spriteLibrary.spriteLibraryAsset
                            .GetCategoryLabelNames(weaponResolver.GetCategory()).ToList();
                        weaponResolver.SetCategoryAndLabel(weaponResolver.GetCategory(), labels[0]);
                        break;
                    
                    /*case EquipmentSlot.Shield:
                    break;*/
                }
            }
        }

        public void PlayAnimMove(Vector3 moveDir) {
            /*if (moveVector == Vector3.zero) {
                // Idle
                unitAnimation.PlayAnim(idleAnimType, lastMoveDir, idleFrameRate, null, null, null);
            } else {
                // Moving
                lastMoveDir = moveDir;
                unitAnimation.PlayAnim(walkAnimType, lastMoveDir, walkFrameRate, null, null, null);
            }*/
        }
        
        public void PlayAnimAttack(Vector3 attackDir, Action onHit, Action onComplete) {
            _animator.SetTrigger(_attackHash);
            onHit?.Invoke();
            onComplete?.Invoke();

            /*unitAnimation.PlayAnimForced(attackUnitAnim, attackDir, 1f, (UnitAnim unitAnim) => {
                if (onComplete != null) onComplete();
            }, (string trigger) => {
                if (onHit != null) onHit();
            }, null);*/
        }

        private IEnumerator PlayAnim(int animHash, Vector3 attackDir, Action onHit, Action onComplete)
        {
            _animator.SetTrigger(animHash);

            float animLength = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            yield return new WaitForSeconds(animLength);

            onComplete?.Invoke();
        }

        /*public virtual void PlayAnimIdle() {
            animatedWalker.SetMoveVector(Vector3.zero);
        }*/

        /*public virtual void PlayAnimIdle(Vector3 animDir) {
            animatedWalker.PlayIdleAnim(animDir);
        }*/

        /*public virtual void PlayAnimSlideRight() {
            unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("dBareHands_SlideRight"), 1f, null);
        }*/

        /*public virtual void PlayAnimSlideLeft() {
            unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("dBareHands_SlideLeft"), 1f, null);
        }*/

        /*public virtual void PlayAnimLyingUp() {
            unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("LyingUp"), 1f, null);
        }*/

        /*public virtual void PlayAnimJump(Vector3 moveDir)
        {
            if (moveDir.x >= 0)
            {
                unitAnimation.PlayAnim(UnitAnim.GetUnitAnim("dBareHands_JumpRight"));
            }
            else
            {
                unitAnimation.PlayAnim(UnitAnim.GetUnitAnim("dBareHands_JumpLeft"));
            }
        }*/

        /*public virtual void SetAnimsBareHands() {
            animatedWalker.SetAnimations(UnitAnimType.GetUnitAnimType("dBareHands_Idle"), UnitAnimType.GetUnitAnimType("dBareHands_Walk"), 1f, 1f);
            attackUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_PunchQuickAttack");
        }*/

        /*public virtual void SetAnimsSwordTwoHandedBack() {
            animatedWalker.SetAnimations(UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Idle"), UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Walk"), 1f, 1f);
            attackUnitAnim = UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Sword");
        }*/

        /*public virtual void SetAnimsSwordShield() {
            animatedWalker.SetAnimations(UnitAnimType.GetUnitAnimType("dSwordShield_Idle"), UnitAnimType.GetUnitAnimType("dSwordShield_Walk"), 1f, 1f);
            attackUnitAnim = UnitAnimType.GetUnitAnimType("dSwordShield_Attack");
        }*/

        /*public virtual Vector3 GetHandLPosition() {
            return unitSkeleton.GetBodyPartPosition("HandL");
        }*/

        /*public virtual Vector3 GetHandRPosition() {
            return unitSkeleton.GetBodyPartPosition("HandR");
        }*/
    }
}