using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameoverUI : MonoBehaviour
{
    public GameObject gameoverUI;
    public TMP_Text scoreText;

    void Start()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.gameoverUI = this;
        }

        scoreText.text = "0";
    }

    public void ShowGameoverUI(int score)
    {
        gameoverUI.SetActive(true);
        SetScore(score);
    }

    public void SetScore(int UpdateScore)
    {
        scoreText.text = UpdateScore.ToString();
    }
}
