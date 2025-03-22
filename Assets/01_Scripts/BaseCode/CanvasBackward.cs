using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBackward : MonoBehaviour
{
    void Start()
    {
        // Canvas의 Sorting Layer를 "Background"로 변경합니다.
        GetComponent<Canvas>().sortingLayerName = "Background";

        // Canvas의 Order in Layer 값을 충분히 낮게 설정합니다.
        GetComponent<Canvas>().sortingOrder = -100;
    }
}
