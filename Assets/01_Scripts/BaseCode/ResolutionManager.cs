using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    public bool isPC;

    public float thickness;

    public RectTransform topPoint;
    public RectTransform bottomPoint;

    public Transform leftWallTransform;
    public Transform rightWallTransform;
    public Transform bottomWallTransform;
    public Transform topWallTransform;

    private void Start()
    {
        if (isPC)
        {
            Screen.SetResolution(500, 900, false); // SetResolution 함수 제대로 사용하기
            //SetResolution(); // 초기에 게임 해상도 고정
        }

        SetWallPosition();
        TopWallPos();
        BottomWallPosition();
    }

    /* 해상도 설정하는 함수 */
    public void SetResolution()
    {
        int setWidth = 500; // 사용자 설정 너비
        int setHeight = 900; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }

    void SetWallPosition()
    {
        Camera mainCamera = Camera.main;
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        leftWallTransform.position = new(bottomLeft.x - thickness, 0);
        rightWallTransform.position = new(topRight.x + thickness, 0);
        //bottomWallTransform.position = new(0, bottomLeft.y);
    }

    void TopWallPos()
    {
        // UI 오브젝트의 좌표와 크기를 가져옴
        Vector3[] corners = new Vector3[4];
        topPoint.GetWorldCorners(corners);

        // y 좌표의 최소값을 초기 최소값으로 설정
        float minY = corners[0].y;

        // 모든 모서리의 y 좌표 중 최소값 찾기
        for (int i = 1; i < corners.Length; i++)
        {
            if (corners[i].y < minY)
            {
                minY = corners[i].y;
            }
        }

        // 게임 좌표로 변환하여 출력
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, minY, 0));

        topWallTransform.position = new(0, worldPosition.y + (thickness * 2f));
    }

    void BottomWallPosition()
    {
        // UI 오브젝트의 좌표와 크기를 가져옴
        Vector3[] corners = new Vector3[4];
        bottomPoint.GetWorldCorners(corners);

        // y 좌표의 최소값을 초기 최소값으로 설정
        float maxY = corners[0].y;

        // 모든 모서리의 y 좌표 중 최소값 찾기
        for (int i = 1; i < corners.Length; i++)
        {
            if (corners[i].y > maxY)
            {
                maxY = corners[i].y;
            }
        }

        // 게임 좌표로 변환하여 출력
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, maxY, 0));

        bottomWallTransform.position = new(0, worldPosition.y - thickness / 2f);
    }
}
