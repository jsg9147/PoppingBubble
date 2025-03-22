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
            Screen.SetResolution(500, 900, false); // SetResolution �Լ� ����� ����ϱ�
            //SetResolution(); // �ʱ⿡ ���� �ػ� ����
        }

        SetWallPosition();
        TopWallPos();
        BottomWallPosition();
    }

    /* �ػ� �����ϴ� �Լ� */
    public void SetResolution()
    {
        int setWidth = 500; // ����� ���� �ʺ�
        int setHeight = 900; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
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
        // UI ������Ʈ�� ��ǥ�� ũ�⸦ ������
        Vector3[] corners = new Vector3[4];
        topPoint.GetWorldCorners(corners);

        // y ��ǥ�� �ּҰ��� �ʱ� �ּҰ����� ����
        float minY = corners[0].y;

        // ��� �𼭸��� y ��ǥ �� �ּҰ� ã��
        for (int i = 1; i < corners.Length; i++)
        {
            if (corners[i].y < minY)
            {
                minY = corners[i].y;
            }
        }

        // ���� ��ǥ�� ��ȯ�Ͽ� ���
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, minY, 0));

        topWallTransform.position = new(0, worldPosition.y + (thickness * 2f));
    }

    void BottomWallPosition()
    {
        // UI ������Ʈ�� ��ǥ�� ũ�⸦ ������
        Vector3[] corners = new Vector3[4];
        bottomPoint.GetWorldCorners(corners);

        // y ��ǥ�� �ּҰ��� �ʱ� �ּҰ����� ����
        float maxY = corners[0].y;

        // ��� �𼭸��� y ��ǥ �� �ּҰ� ã��
        for (int i = 1; i < corners.Length; i++)
        {
            if (corners[i].y > maxY)
            {
                maxY = corners[i].y;
            }
        }

        // ���� ��ǥ�� ��ȯ�Ͽ� ���
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, maxY, 0));

        bottomWallTransform.position = new(0, worldPosition.y - thickness / 2f);
    }
}
