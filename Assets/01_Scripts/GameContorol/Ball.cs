using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ball : MonoBehaviour
{
    private Dictionary<Ball, List<Ball>> cachedConnectedBalls = new Dictionary<Ball, List<Ball>>();
    private bool removeConnectedBallsExecuted = false;
    public int colorNum;
    public Sprite contectWallEmojiSprite;
    public GameObject bombEffect;

    public SpriteRenderer ballSpriteRenderer;
    public SpriteRenderer emojiSpriteRenderer;

    [HideInInspector] public float ballScale;
    [HideInInspector] public GameObject effectPrefab;

    public bool isRemoved = false;
    GameManager gameManager;


    // New Code
    HashSet<GameObject> leftCollisions = new HashSet<GameObject>();
    HashSet<GameObject> rightCollisions = new HashSet<GameObject>();
    public BallController ballController;

    Animator animator;
    List<Ball> connectedBalls;

    Sprite originSprite;


    void Start()
    {
        animator = GetComponent<Animator>();
        connectedBalls = new();
    }

    public void SetBallInfo(BallInfo ballInfo)
    {
        colorNum = ballInfo.colorNum;
        ballScale = ballInfo.ballScale;
        ballSpriteRenderer.sprite = ballInfo.ballSprite;
        emojiSpriteRenderer.sprite = ballInfo.emojiSprite;
        transform.localScale = Vector3.one * ballScale;

        originSprite = ballInfo.emojiSprite;
    }

    public bool GetHasLeftWall() => leftCollisions.Count > 0;

    public bool GetHasRightWall() => rightCollisions.Count > 0;

    void ConnectedBallHandle(Collision2D collision)
    {
        try
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                Ball collidedBall = collision.gameObject.GetComponent<Ball>();
                if (collidedBall.isRemoved)
                    return;

                if (collidedBall.colorNum == colorNum)
                {
                    connectedBalls.Add(collidedBall);
                }
            }
        }
        catch (System.NullReferenceException ex)
        {
            print(ex);
        }
    }

    void DisconnectBallHandle(Collision2D collision)
    {
        try
        {
            if (collision.gameObject.CompareTag("Ball"))
            {
                Ball collidedBall = collision.gameObject.GetComponent<Ball>();

                if (collidedBall.colorNum == colorNum)
                {
                    connectedBalls.Remove(collidedBall);
                }
            }
        }
        catch (System.NullReferenceException ex)
        {
            print(ex);
        }
    }

    bool HasBothWallCheck()
    {
        if (isRemoved || removeConnectedBallsExecuted || (!GetHasRightWall() && !GetHasLeftWall()))
            return false;

        bool rightWallTouch = GetHasRightWall();
        bool leftWallTouch = GetHasLeftWall();

        foreach (Ball targetBall in GetConnectedBallList())
        {
            if (targetBall.GetHasLeftWall())
            {
                leftWallTouch = true;
            }
            if (targetBall.GetHasRightWall())
            {
                rightWallTouch = true;
            }
        }

        return leftWallTouch && rightWallTouch;
    }

    public List<Ball> GetConnectedBallList()
    {
        cachedConnectedBalls.Clear();
        List<Ball> connectedAllBalls = new List<Ball>(connectedBalls);
        List<Ball> visitedBalls = new List<Ball>();
        RecursiveFindConnectedBalls(this, connectedAllBalls, visitedBalls);

        cachedConnectedBalls[this] = connectedAllBalls;

        return connectedAllBalls;
    }
    private void RecursiveFindConnectedBalls(Ball currentBall, List<Ball> connectedAllBalls, List<Ball> visitedBalls)
    {
        if (visitedBalls.Contains(currentBall))
        {
            return;
        }

        visitedBalls.Add(currentBall);

        foreach (Ball ball in currentBall.connectedBalls)
        {
            if (!visitedBalls.Contains(ball) && ball.colorNum == currentBall.colorNum)
            {
                connectedAllBalls.Add(ball);
                RecursiveFindConnectedBalls(ball, connectedAllBalls, visitedBalls);
            }
        }
    }

    public void RemoveConnectedBalls()
    {
        if (isRemoved)
            return;

        ballSpriteRenderer.color = Color.red;
        List<Ball> connectedAllBalls = GetConnectedBallList();
        connectedAllBalls.Add(this);
        ballController.RemoveConnectedBalls(connectedAllBalls);
    }

    public void BallRemove()
    {
        if (isRemoved)
            return;

        isRemoved = true;
        animator.SetTrigger("Bomb");
        Instantiate(bombEffect, transform.position, Quaternion.identity);
        StartCoroutine(DestroyBall());
    }


    IEnumerator DestroyBall()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void SetSideWallCollision(Collision2D collision, bool isEnter)
    {
        if (!collision.gameObject.CompareTag("Left") && !collision.gameObject.CompareTag("Right"))
        {
            return;
        }

        if (isEnter)
        {
            if (collision.gameObject.CompareTag("Left"))
            {
                leftCollisions.Add(collision.gameObject);
            }
            if (collision.gameObject.CompareTag("Right"))
            {
                rightCollisions.Add(collision.gameObject);
            }
            emojiSpriteRenderer.sprite = contectWallEmojiSprite;
        }
        else
        {
            if (collision.gameObject.CompareTag("Left"))
            {
                leftCollisions.Remove(collision.gameObject);
            }
            if (collision.gameObject.CompareTag("Right"))
            {
                rightCollisions.Remove(collision.gameObject);
            }
            emojiSpriteRenderer.sprite = originSprite;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        SetSideWallCollision(collision, true);
        ConnectedBallHandle(collision);

        if (HasBothWallCheck())
        {
            connectedBalls.Add(this);
            ballController.RemoveConnectedBalls(connectedBalls);
            removeConnectedBallsExecuted = true; // 플래그 설정
        }

        if (collision.gameObject.CompareTag("EndLine"))
        {
            gameManager.GameOver();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        SetSideWallCollision(collision, true);

        if (HasBothWallCheck() && !removeConnectedBallsExecuted)
        {
            List<Ball> connectedAllBalls = GetConnectedBallList();
            connectedAllBalls.Add(this);
            ballController.RemoveConnectedBalls(connectedAllBalls);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        SetSideWallCollision(collision, false);
        DisconnectBallHandle(collision);
        removeConnectedBallsExecuted = false; // 플래그 재설정
    }
}
