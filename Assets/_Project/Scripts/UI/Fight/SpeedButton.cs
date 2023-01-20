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

    private int _speed;
    
    private void Start()
    {
        SetButtonSpeed(GameManager.Instance.GetPlayerOptions().FightSpeed);
        
        FightManager.Instance.OnFightOver += (_, _) =>
        {
            transform.gameObject.SetActive(false);
        };
    }

    public void OnButtonClick()
    {
        switch (_speed)
        {
            case 1:
                SetButtonSpeed(2);
                break;
            case 2:
                SetButtonSpeed(4);
                break;
            case 4:
                SetButtonSpeed(1);
                break;
        }
    }

    private void SetButtonSpeed(int speed)
    {
        _speed = speed;
        
        FightManager.Instance.SetFightSpeed(speed);
        buttonLabel.text = "x" + speed;
    }
}
