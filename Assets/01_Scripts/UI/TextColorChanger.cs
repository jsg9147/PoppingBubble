using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextColorChanger : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public Color[] colors;
    public float duration = 1.0f;

    void Start()
    {
        StartCoroutine(ChangeColorRoutine());
    }

    private void OnEnable()
    {
        StartCoroutine(ChangeColorRoutine());
    }

    IEnumerator ChangeColorRoutine()
    {
        while (true)
        {
            Color randomColor = colors[Random.Range(0, colors.Length)];
            float elapsedTime = 0.0f;
            Color startColor = textMeshPro.color;

            while (elapsedTime < duration)
            {
                textMeshPro.color = Color.Lerp(startColor, randomColor, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}
