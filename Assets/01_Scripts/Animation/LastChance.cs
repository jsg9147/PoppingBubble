using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LastChance : MonoBehaviour
{
    public GameManager gameManager;

    public GameObject beforeImage;
    public GameObject afterImage;

    public ScaleAnimation touchHandObj;

    private bool isPlaying = false;

    public Button lastChanceADbutton;
    public Button endButton;


    private void Start()
    {
        lastChanceADbutton.onClick.AddListener(ADButton);
        endButton.onClick.AddListener(EndButton);
    }

    private void OnEnable()
    {
        Play();
    }

    public void Play()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation()
    {
        isPlaying = true;
        while (gameObject.activeSelf)
        {
            beforeImage.SetActive(true);
            afterImage.SetActive(false);
            touchHandObj.gameObject.SetActive(true);
            touchHandObj.Play();
            yield return new WaitForSecondsRealtime(touchHandObj.duration * 2);
            beforeImage.SetActive(false);
            afterImage.SetActive(true);
            touchHandObj.gameObject.SetActive(false);

            yield return new WaitForSecondsRealtime(touchHandObj.duration * 2);
        }

        isPlaying = false;
    }

    void ADButton()
    {
        // gameManager.EraseModeAD();
        // gameObject.SetActive(false);
    }

    void EndButton()
    {
        // gameManager.isLastChance = false;
        // gameManager.EndGame();
    }
}
