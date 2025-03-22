using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallPreview : MonoBehaviour
{
    public Image ballEmoji;
    public Image ballColor;

    public void SetImage(BallInfo ballInfo)
    {
        ballEmoji.sprite = ballInfo.emojiSprite;
        ballColor.sprite = ballInfo.ballSprite;
    }
}
