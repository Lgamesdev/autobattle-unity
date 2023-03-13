using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using LGamesDev.Fighting;
using TMPro;
using UnityEngine;

public class AutoButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textSwitch;
    private bool _isAutoOn;

    public void OnButtonClick()
    {
        _isAutoOn = !_isAutoOn;
        if (_isAutoOn)
        {
            FightManager.Instance.ActionsComplete += Attack;
            textSwitch.text = "On";
        }
        else
        {
            FightManager.Instance.ActionsComplete -= Attack;
            textSwitch.text = "Off";
        }
    }

    private void Attack()
    {
        GameManager.Instance.networkManager.Attack();
    }
}
