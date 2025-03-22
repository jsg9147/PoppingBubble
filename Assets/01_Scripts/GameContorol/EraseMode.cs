using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraseMode : MonoBehaviour
{

    public GameObject eraseModeUI;

    bool isEraseMode = false;
    void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.eraseMode = this;
        }
    }

    void Update()
    {
        if (isEraseMode)
        {
            TouchDetect();
        }
    }

    void TouchDetect()
    {
        if (Input.GetMouseButtonUp(0) && isEraseMode)
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null)
            {
                Ball ball = hit.collider.GetComponent<Ball>();

                if (ball != null)
                {
                    EraseItemUse(ball);

                }
                else
                {
                    print(hit.collider.gameObject.name);
                }
            }
        }
    }

    public void LastChancePopupOn()
    {
        Time.timeScale = 0;
        eraseModeUI.SetActive(true);
    }

    public void ReviveBtn()
    {
        eraseModeUI.SetActive(false);
        isEraseMode = true;
    }

    public void CloseBtn()
    {
        eraseModeUI.SetActive(false);
        GameManager.instance.GameOver();
        Time.timeScale = 1;
    }

    void EraseItemUse(Ball ball)
    {
        Time.timeScale = 1;
        isEraseMode = false;
        ball.RemoveConnectedBalls();
    }
}
