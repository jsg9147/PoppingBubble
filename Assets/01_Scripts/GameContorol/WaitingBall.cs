using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingBall : MonoBehaviour
{
    public SpriteRenderer ballSpriteRenderer;
    public SpriteRenderer emojiSpriteRenderer;
    public SpriteRenderer dotLineSpriteRenderer;

    public float limitXPos;

    CircleCollider2D ballCollider;

    void Start()
    {
        ballCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        FollowMouseX();
    }

    public void SetSprite(BallInfo ballInfo)
    {
        ballSpriteRenderer.sprite = ballInfo.ballSprite;
        emojiSpriteRenderer.sprite = ballInfo.emojiSprite;

        transform.localScale = Vector3.one * ballInfo.ballScale;
    }
    void FollowMouseX()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

        float radius = transform.localScale.x * ballCollider.radius;
        float maxXPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10f)).x;
        float minXPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10f)).x;

        float wallThick = 0.15f;

        float clampedXPos = Mathf.Clamp(mousePosition.x, -limitXPos + radius + wallThick, limitXPos - radius - wallThick);

        transform.position = new Vector3(clampedXPos, transform.position.y, transform.position.z);
    }

    public void SetVisible(bool isTrue)
    {
        ballSpriteRenderer.enabled = isTrue;
        emojiSpriteRenderer.enabled = isTrue;
        dotLineSpriteRenderer.enabled = isTrue;
    }
}
