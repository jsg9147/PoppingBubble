using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text highScoreText;
    public TMP_Text scoreText;

    void Start()
    {
        ScoreInit();
    }

    void ScoreInit()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.scoreUI = this;
        }
        else
        {
            Debug.Log("UIManager is null");
        }

        if (scoreText != null)
        {
            scoreText.text = "0";
        }
        if (highScoreText != null)
        {
            highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        }
    }

    public void SetScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }

        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            if (highScoreText != null)
            {
                highScoreText.text = score.ToString();
            }
        }
    }
}
