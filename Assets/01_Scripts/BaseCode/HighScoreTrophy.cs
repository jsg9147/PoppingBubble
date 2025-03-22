using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoreTrophy : MonoBehaviour
{
    public List<Color> trophyColors;

    public Image trophyImage;
    public TMP_Text scoreText;

    public void SetScore(int score)
    {
        scoreText.text = score.ToString();

        try
        {
            if (score < 2000)
            {
                trophyImage.color = Color.white;
            }
            else if (2000 < score && score < 4000)
            {
                trophyImage.color = trophyColors[0];
            }
            else if (4000 < score && score < 6000)
            {
                trophyImage.color = trophyColors[1];
            }
            else if (10000 < score)
            {
                trophyImage.color = trophyColors[2];
            }
        }
        catch (System.ArgumentException ex)
        {
            print(ex);
        }
    }
}
