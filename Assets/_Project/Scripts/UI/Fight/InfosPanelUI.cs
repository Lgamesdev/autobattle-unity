using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev.Fighting;
using TMPro;
using UnityEngine;

public class InfosPanelUI : MonoBehaviour
{
    public TextMeshProUGUI playerUsername;
    public TextMeshProUGUI playerLevel;

    public TextMeshProUGUI opponentUsername;
    public TextMeshProUGUI opponentLevel;

    private void Awake()
    {
        playerUsername = transform.Find("Player Username").GetComponent<TextMeshProUGUI>();
        playerLevel = transform.Find("Player Level").GetComponent<TextMeshProUGUI>();
        opponentUsername = transform.Find("Opponent Username").GetComponent<TextMeshProUGUI>();
        opponentLevel = transform.Find("Opponent Level").GetComponent<TextMeshProUGUI>();
    }
}
