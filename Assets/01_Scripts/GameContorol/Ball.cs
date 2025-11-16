using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ball : MonoBehaviour
{
    private bool removeConnectedBallsExecuted = false;
    public int colorNum;
    public Sprite contectWallEmojiSprite;
    public GameObject bombEffect;

    public SpriteRenderer ballSpriteRenderer;
    public SpriteRenderer emojiSpriteRenderer;

    [HideInInspector] public float ballScale;
    [HideInInspector] public GameObject effectPrefab;

    public bool isRemoved = false;

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

                if (collidedBall.colorNum == colorNum && !connectedBalls.Contains(collidedBall))
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


    public List<Ball> GetConnectedBallList()
    {
        List<Ball> connectedAllBalls = new List<Ball>(connectedBalls);
        List<Ball> visitedBalls = new List<Ball>();
        RecursiveFindConnectedBalls(this, connectedAllBalls, visitedBalls);

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
            return;

        if (isEnter)
        {
            if (collision.gameObject.CompareTag("Left"))
                leftCollisions.Add(collision.gameObject);
            if (collision.gameObject.CompareTag("Right"))
                rightCollisions.Add(collision.gameObject);
        }
        else
        {
            if (collision.gameObject.CompareTag("Left"))
                leftCollisions.Remove(collision.gameObject);
            if (collision.gameObject.CompareTag("Right"))
                rightCollisions.Remove(collision.gameObject);
        }

        // 최종 상태 기준으로 이모지 결정
        if (GetHasLeftWall() || GetHasRightWall())
            emojiSpriteRenderer.sprite = contectWallEmojiSprite;
        else
            emojiSpriteRenderer.sprite = originSprite;
    }


    void TryRemoveIfBothWalls()
    {
        if (isRemoved || removeConnectedBallsExecuted)
            return;

        // 1. 전체 그룹 구하기
        List<Ball> connectedAllBalls = GetConnectedBallList();
        connectedAllBalls.Add(this);

        // 2. 그룹 기준으로 왼/오 벽 닿음 체크
        bool leftWallTouch = false;
        bool rightWallTouch = false;

        foreach (var b in connectedAllBalls)
        {
            if (b.isRemoved) continue;

            if (b.GetHasLeftWall()) leftWallTouch = true;
            if (b.GetHasRightWall()) rightWallTouch = true;
        }

        // 3. 둘 다 있으면 제거
        if (leftWallTouch && rightWallTouch)
        {
            removeConnectedBallsExecuted = true;
            ballController.RemoveConnectedBalls(connectedAllBalls);
        }
    }
    // 같은 색으로 연결된 그룹끼리 이모지 통일
    void SyncGroupEmoji()
    {
        // 1. 이 공을 포함한 같은 색 연결 그룹 가져오기
        List<Ball> group = GetConnectedBallList();
        if (!group.Contains(this))
            group.Add(this);

        // 2. 이 그룹의 기준 이모지 하나 정하기
        //    - 심플하게: "현재 이 공의 originSprite"를 기준으로 사용
        Sprite groupEmoji = originSprite;

        foreach (var b in group)
        {
            if (b == null || b.isRemoved)
                continue;

            // 그룹 공들의 '원래 이모지'를 다 이걸로 통일
            b.originSprite = groupEmoji;

            // 벽에 닿아있지 않은 애들만 실제 표시도 그룹 이모지로 맞추기
            if (!b.GetHasLeftWall() && !b.GetHasRightWall())
            {
                b.emojiSpriteRenderer.sprite = groupEmoji;
            }
        }
    }

void OnCollisionEnter2D(Collision2D collision)
{
    SetSideWallCollision(collision, true);
    ConnectedBallHandle(collision);

    // 같은 색으로 연결된 그룹끼리 이모지 통일
    SyncGroupEmoji();

    TryRemoveIfBothWalls();

    if (collision.gameObject.CompareTag("EndLine"))
    {
        GameManager.instance.GameOver();
    }
}

void OnCollisionStay2D(Collision2D collision)
{
    SetSideWallCollision(collision, true);
    ConnectedBallHandle(collision);   // Stay에서도 관계가 바뀔 수 있으면 유지

    // 그룹 이모지 재동기화
    SyncGroupEmoji();

    TryRemoveIfBothWalls();
}

    void OnCollisionExit2D(Collision2D collision)
    {
        SetSideWallCollision(collision, false);
        DisconnectBallHandle(collision);
    }

}
