using System;
using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.Fighting
{
    public class PlayerRanking : MonoBehaviour
    {
        private Text _losesNumber;
        private int _playerLoses;

        private int _playerWins;
        private Text _winsNumber;

        private void Awake()
        {
            _winsNumber = transform.Find("winsNumber").GetComponent<Text>();
            _losesNumber = transform.Find("losesNumber").GetComponent<Text>();

            _playerWins = PlayerPrefs.GetInt("playerWins");
            _playerLoses = PlayerPrefs.GetInt("playerLoses");

            _winsNumber.text = _playerWins.ToString();
            _losesNumber.text = _playerLoses.ToString();
        }

        private void Start()
        {
            FightManager.Instance.FightOver += OnFightOver;
        }

        private void OnFightOver(Reward reward, bool playerWin)
        {
            if (playerWin)
            {
                _winsNumber.text = (_playerWins + 1).ToString();
                PlayerPrefs.SetInt("playerWins", PlayerPrefs.GetInt("playerWins") + 1);
                PlayerPrefs.Save();
            }
            else
            {
                _losesNumber.text = (_playerLoses + 1).ToString();
                PlayerPrefs.SetInt("playerLoses", PlayerPrefs.GetInt("playerLoses") + 1);
                PlayerPrefs.Save();
            }
        }
    }
}