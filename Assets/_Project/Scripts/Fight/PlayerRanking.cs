using UnityEngine;
using UnityEngine.UI;

namespace LGamesDev.Fighting
{
    public class PlayerRanking : MonoBehaviour
    {
        private Text losesNumber;
        private int playerLoses;

        private int playerWins;
        private Text winsNumber;

        private void Awake()
        {
            winsNumber = transform.Find("winsNumber").GetComponent<Text>();
            losesNumber = transform.Find("losesNumber").GetComponent<Text>();

            playerWins = PlayerPrefs.GetInt("playerWins");
            playerLoses = PlayerPrefs.GetInt("playerLoses");

            winsNumber.text = playerWins.ToString();
            losesNumber.text = playerLoses.ToString();

            FightManager.Instance.onPlayerWin += BattleHandler_OnPlayerWin;
            FightManager.Instance.onPlayerLose += BattleHandler_OnPlayerLose;
        }

        private void BattleHandler_OnPlayerLose()
        {
            losesNumber.text = (playerLoses + 1).ToString();
            PlayerPrefs.SetInt("playerLoses", PlayerPrefs.GetInt("playerLoses") + 1);
            PlayerPrefs.Save();
        }

        private void BattleHandler_OnPlayerWin()
        {
            winsNumber.text = (playerWins + 1).ToString();
            PlayerPrefs.SetInt("playerWins", PlayerPrefs.GetInt("playerWins") + 1);
            PlayerPrefs.Save();
        }
    }
}