using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev.Fighting;
using UnityEngine;

public class HealthPannelUI : MonoBehaviour
{
    public HealthBar playerHealthBar;

    public HealthBar opponentHealthBar;

    private void Awake()
    {
        playerHealthBar = transform.Find("PlayerTeamHealthBar").GetComponent<HealthBar>();
        opponentHealthBar = transform.Find("OpponentTeamHealthBar").GetComponent<HealthBar>();
    }
}
