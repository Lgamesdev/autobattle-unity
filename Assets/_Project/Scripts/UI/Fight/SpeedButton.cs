using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev;
using LGamesDev.Fighting;
using TMPro;
using UnityEngine;

public class SpeedButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonLabel;

    private void Start()
    {
        FightManager.Instance.FightSpeedChanged += SetButtonSpeed;
    }

    public void OnButtonClick()
    {
        switch (FightManager.Instance.fightSpeed)
        {
            case 1:
                FightManager.Instance.SetFightSpeed(2);
                break;
            case 2:
                FightManager.Instance.SetFightSpeed(4);
                break;
            case 4:
                FightManager.Instance.SetFightSpeed(1);
                break;
        }
    }

    private void SetButtonSpeed(int speed)
    {
        buttonLabel.text = "x" + speed;
    }
}
