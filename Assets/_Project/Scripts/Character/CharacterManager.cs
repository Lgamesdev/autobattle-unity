using System;
using System.Collections;
using LGamesDev.Core.Character;
using LGamesDev.Core.Player;
using UnityEngine;

namespace LGamesDev
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance;

        public delegate void OnCharacterSwitchChangeEvent();

        public OnCharacterSwitchChangeEvent OnCharacterSwitchChange;

        [SerializeField] private CharacterHandler maleCharacter;
        [SerializeField] private CharacterHandler femaleCharacter;
        [field: SerializeField] public CharacterHandler activeCharacter { get; private set; }
        
        [field: SerializeField] public SpriteLib activeSpriteLib { get; private set; }

        [Header("Managers")] 
        public CharacterEquipmentManager equipmentManager;
        public CharacterStatsManager statsManager;
        public PlayerInventoryManager inventoryManager;
        public PlayerWalletManager walletManager;

        public readonly Character Character = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public IEnumerator SetupCharacter(Character character)
        {
            Character.Level = character.Level;
            Character.Experience = character.Experience;
            Character.StatPoint = character.StatPoint;
            Character.Ranking = character.Ranking;
            
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

            foreach (CharacterEquipment characterEquipment in character.Gear.equipments)
            {
                Character.Gear.equipments[(int)characterEquipment.item.equipmentSlot] = characterEquipment;
                yield return new WaitForEndOfFrame();
            }

            //Setup Stats
            foreach (StatType statType in (StatType[])Enum.GetValues(typeof(StatType)))
            {
                Character.Stats[(int)statType] = new Stat() { statType = statType };
                yield return new WaitForEndOfFrame();
            }

            foreach (Stat stat in character.Stats)
            {
                Character.Stats[(int)stat.statType] = stat;
                yield return new WaitForEndOfFrame();
            }

            //Setup Equipment Manager
            equipmentManager = GetComponent<CharacterEquipmentManager>();
            equipmentManager.SetupManager(Character.Gear);

            //Setup Stat Manager
            statsManager = GetComponent<CharacterStatsManager>();
            statsManager.SetupManager(Character.Stats);

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

            Character.Body = character.Body;

            CreateCharacter(Character.Body.isMaleGender ? SpriteLib.Male : SpriteLib.Female);

            equipmentManager.OnEquipmentChanged += activeCharacter.UpdateEquipmentTexture;
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
            OnCharacterSwitchChange?.Invoke();
        }
    }
    
    public enum SpriteLib
    {
        Male,
        Female
    }
}
