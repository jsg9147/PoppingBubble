using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBackward : MonoBehaviour
{
    void Start()
    {
        // Canvas�� Sorting Layer�� "Background"�� �����մϴ�.
        GetComponent<Canvas>().sortingLayerName = "Background";

        // Canvas�� Order in Layer ���� ����� ���� �����մϴ�.
        GetComponent<Canvas>().sortingOrder = -100;
    }
}
