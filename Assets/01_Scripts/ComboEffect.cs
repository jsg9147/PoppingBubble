using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboEffect : MonoBehaviour
{
    public TMPro.TMP_Text comboText;
    private Coroutine deactivateCoroutine;

    void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.comboEffect = this;
        }

        gameObject.SetActive(false);
    }

    public void SetCombo(int combo)
    {
        comboText.text = combo.ToString();
        if (deactivateCoroutine != null)
        {
            StopCoroutine(deactivateCoroutine);
        }
        deactivateCoroutine = StartCoroutine(DeactivateAfterDelay(1f));
    }
    IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    public void Deactivate(float delay)
    {
        StartCoroutine(DeactivateAfterDelay(delay));
    }
}
