using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallController : MonoBehaviour
{
    public Ball ballPrefab;

    public BallPreview ballPreview;
    public WaitingBall waitingBall;

    public List<float> ballScales;
    public List<Sprite> ballSprites;
    public List<Sprite> emojiSprites;

    public float dropInterval;
    float dropTimer;

    BallInfo currentInfo;
    BallInfo nextInfo;

    int count = 0;

    void Start()
    {
        Application.targetFrameRate = 60;
        SetBallInfo();
        SetCurrentBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (dropTimer < dropInterval)
        {
            dropTimer += Time.deltaTime;
        }

        waitingBall.SetVisible(CanDropBall());

        if (CanDropBall() && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonUp(0))
            {
                GameManager.instance.ComboReset();
                DropBall();
                SetCurrentBall();
                SetBallInfo();
            }
        }

    }

    void SetBallInfo()
    {
        nextInfo = new BallInfo();
        int colorNum = Random.Range(0, ballSprites.Count);
        nextInfo.ballSprite = ballSprites[colorNum];
        nextInfo.emojiSprite = emojiSprites[Random.Range(0, emojiSprites.Count)];
        nextInfo.colorNum = colorNum;
        nextInfo.ballScale = ballScales[Random.Range(0, ballScales.Count)];

        ballPreview.SetImage(nextInfo);
    }

    void SetCurrentBall()
    {
        currentInfo = nextInfo;
        waitingBall.SetSprite(currentInfo);
        SetBallInfo();
    }

    void DropBall()
    {
        Ball ball = Instantiate(ballPrefab);
        ball.SetBallInfo(currentInfo);
        ball.ballController = this;

        ball.transform.position = waitingBall.transform.position;

        ball.transform.name = count.ToString();
        count++;

        dropTimer = 0;
    }

    bool CanDropBall()
    {
        return dropTimer > dropInterval;
    }

    public void RemoveConnectedBalls(List<Ball> connectedBalls)
    {
        int removeCount = 0;
        int colorNum = connectedBalls[0].colorNum;
        foreach (Ball ball in connectedBalls)
        {
            if (ball.isRemoved == false)
            {
                ball.BallRemove();
                removeCount++;
            }
        }
        GameManager.instance.Combo(colorNum);
        GameManager.instance.AddScore(removeCount);

        dropTimer = -1f;
    }
}

public class BallInfo
{
    public Sprite ballSprite;
    public Sprite emojiSprite;
    public int colorNum;
    public float ballScale;
}