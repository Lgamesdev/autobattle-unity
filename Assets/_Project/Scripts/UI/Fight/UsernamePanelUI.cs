using System;
using System.Collections;
using System.Collections.Generic;
using LGamesDev.Fighting;
using TMPro;
using UnityEngine;

public class UsernamePanelUI : MonoBehaviour
{
    public TextMeshProUGUI playerUsername;

    public TextMeshProUGUI opponentUsername;

    private void Awake()
    {
        playerUsername = transform.Find("Player Username").GetComponent<TextMeshProUGUI>();
        opponentUsername = transform.Find("Opponent Username").GetComponent<TextMeshProUGUI>();
    }
}
