using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    public float minScale = 0.4f; // 최소 스케일
    public float maxScale = 0.7f; // 최대 스케일
    public float duration = 1f; // 애니메이션 지속 시간

    public void Play()
    {
        StartCoroutine(ScaleObject());
    }

    IEnumerator ScaleObject()
    {
        for (int i = 0; i < 2; i++)
        {
            yield return ScaleOverTime(transform.localScale.x, minScale, maxScale, duration);
            yield return ScaleOverTime(transform.localScale.x, maxScale, minScale, duration);
        }
    }

    IEnumerator ScaleOverTime(float startScale, float from, float to, float time)
    {
        float currentTime = 0f;

        while (currentTime < time)
        {
            float t = currentTime / time;
            float scale = Mathf.Lerp(from, to, t);
            transform.localScale = new Vector3(scale, scale, scale);
            currentTime += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localScale = new Vector3(to, to, to);
    }
}
