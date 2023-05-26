using System;
using System.Collections;
using Core.Player;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance;

        public delegate void CharacterSwitchChangeEvent();
        public CharacterSwitchChangeEvent CharacterSwitchChange;

        [SerializeField] private CharacterHandler maleCharacter;
        [SerializeField] private CharacterHandler femaleCharacter;
        public CharacterHandler activeCharacter { get; private set; }
        
        public SpriteLib activeSpriteLib { get; private set; }

        [Header("Managers")] 
        public CharacterEquipmentManager equipmentManager;
        public CharacterStatsManager statsManager;
        public PlayerInventoryManager inventoryManager;
        public PlayerWalletManager walletManager;

        public readonly Character Character = new();

        public Action<Character> PlayerInfosUpdate;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public IEnumerator SetupCharacter(IFighter fighter)
        {
            Character.username = GameManager.Instance.GetAuthentication().username;
            Character.level = fighter.Level;
            
            //Setup Equipments
            foreach (EquipmentSlot equipmentSlot in (EquipmentSlot[])Enum.GetValues(typeof(EquipmentSlot)))
            {
                Equipment defaultEquipment = new Equipment() { equipmentSlot = equipmentSlot, isDefaultItem = true };

                Character.Gear.equipments[(int)equipmentSlot] = new CharacterEquipment()
                {
                    item = defaultEquipment
                };
                yield return new WaitForEndOfFrame();
            }
            
            //Setup stats
            foreach (StatType statType in (StatType[])Enum.GetValues(typeof(StatType)))
            {
                Character.stats[(int)statType] = new Stat() { statType = statType };
                yield return new WaitForEndOfFrame();
            }

            foreach (Stat stat in fighter.Stats)
            {
                Character.stats[(int)stat.statType] = stat;
                yield return new WaitForEndOfFrame();
            }
            
            //Setup Equipment Manager
            equipmentManager = GetComponent<CharacterEquipmentManager>();
            equipmentManager.SetupManager(Character.Gear);

            //Setup Stat Manager
            statsManager = GetComponent<CharacterStatsManager>();
            statsManager.SetupManager(Character.stats);
            
            Character.Body = fighter.Body;

            if (fighter is Character character)
            {
                Character.Experience = character.Experience;
                Character.MaxExperience = character.MaxExperience;
                Character.StatPoint = character.StatPoint;
                Character.Ranking = character.Ranking;
                
                //Setup Equipments
                foreach (CharacterEquipment characterEquipment in character.Gear.equipments)
                {
                    Character.Gear.equipments[(int)characterEquipment.item.equipmentSlot] = characterEquipment;
                    yield return new WaitForEndOfFrame();
                }
                
                //Setup Wallet/Wallet Manager
                Character.Wallet = character.Wallet;
                walletManager = GetComponent<PlayerWalletManager>();
                if (walletManager != null)
                {
                    walletManager.SetupManager(Character.Wallet);
                }
                
                //Setup Inventory/Inventory Manager
                Character.Inventory = character.Inventory;
                inventoryManager = GetComponent<PlayerInventoryManager>();
                if (inventoryManager != null)
                {
                    inventoryManager.SetupManager(Character.Inventory);
                }
            }
            
            PlayerInfosUpdate?.Invoke(Character);

            CreateCharacter(Character.Body.isMaleGender ? SpriteLib.Male : SpriteLib.Female);

            equipmentManager.EquipmentChanged += activeCharacter.UpdateEquipmentTexture;
            
            yield return activeCharacter.SetupCharacter(Character);
        }

        public void CreateCharacter(SpriteLib spriteLib)
        {
            activeSpriteLib = spriteLib;
            
            if (activeCharacter != null)
            {
                Destroy(activeCharacter.gameObject);
            }

            activeCharacter = Instantiate(spriteLib == SpriteLib.Male ? maleCharacter : femaleCharacter, transform);
            CharacterSwitchChange?.Invoke();
        }
    }
    
    public enum SpriteLib
    {
        Male,
        Female
    }
}
