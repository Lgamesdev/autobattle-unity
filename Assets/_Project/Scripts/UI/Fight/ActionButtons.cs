using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev.Fighting;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LGamesDev
{
    public class ActionButtons : MonoBehaviour
    {
        public static ActionButtons Instance;
        
        [SerializeField] private Button attackButton;
        [SerializeField] private Button parryButton;
        [SerializeField] private Button specialAttackButton;
        [SerializeField] private TextMeshProUGUI autoTextSwitch;
        private bool _isAutoOn;
        private bool _isAttacking;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            FightManager.Instance.ActionsComplete += ActionsComplete;
            //_attackButton.interactable = false;
            if (FightManager.Instance.playerCharacterFight.GetEnergy() != 100)
            {
                specialAttackButton.interactable = false;
            }
        }

        public void OnAutoClick()
        {
            _isAutoOn = !_isAutoOn;
            if (_isAutoOn)
            {
                attackButton.interactable = false;
                parryButton.interactable = false;
                specialAttackButton.interactable = false;
                autoTextSwitch.text = "On";
                if (!_isAttacking)
                {
                    Attack(FightActionType.Attack);
                }
            }
            else
            {
                autoTextSwitch.text = "Off";
            }
        }

        public void Attack(FightActionType actionType)
        {
            if (_isAttacking) return;
            Debug.Log("click!");
            FightManager.Instance.fightService.Attack(actionType);
            _isAttacking = true;
            attackButton.interactable = false;
            parryButton.interactable = false;
            specialAttackButton.interactable = false;
            
        }

        private void ActionsComplete()
        {
            _isAttacking = false;

            if (_isAutoOn)
            {
                AutoAttack();
            }
            else
            {
                attackButton.interactable = true;
                parryButton.interactable = true;
                specialAttackButton.interactable = FightManager.Instance.playerCharacterFight.GetEnergy() == 100;
            }
        }

        private void AutoAttack()
        {
            Attack(FightManager.Instance.playerCharacterFight.GetEnergy() == 100
                ? FightActionType.SpecialAttack
                : FightActionType.Attack);
        }
    }
}
