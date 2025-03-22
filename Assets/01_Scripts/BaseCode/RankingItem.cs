using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankingItem : MonoBehaviour
{
    public Sprite[] rankingIconSprites;

    public TMP_Text rankText;
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text scoreText;

    public int score;

    public void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    public void SetRankingText(int rank)
    {
        rankText.text = (rank + 1).ToString();

        if (rank + 1 > 99)
            rankText.text = "99+";

        switch (rank)
        {
            case 0:
            case 1:
            case 2:
                icon.sprite = rankingIconSprites[rank];
                break;
            default:
                break;
        }
    }
}
