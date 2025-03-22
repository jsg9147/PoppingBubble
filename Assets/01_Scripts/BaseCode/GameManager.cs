using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using DarkTonic.MasterAudio;

public class GameManager : MonoBehaviour
{
    private float lastGameOverTime = 0;
    private float gameOverCooldown = 1.5f;
    public static GameManager instance;

    public ComboEffect comboEffect;
    public EraseMode eraseMode;
    public bool useLastChance = false;

    int score;
    int combo;

    int currentComboColor;

    float blockTime = 1.5f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Init();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MasterAudio.PlaySound3DAtVector3AndForget("Click", Camera.main.transform.position);
        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Init();
    }

    void Init()
    {
        score = 0;
        combo = 0;
        ComboReset();
        MasterAudio.StartPlaylist("BGM");
    }

    public void AddScore(int value)
    {
        value = value * (int)Math.Pow(10, combo - 1);
        score += value;
        UIManager.Instance.UpdateScore(score);
    }

    public void Combo(int color)
    {
        if (currentComboColor != color)
        {
            combo++;
            currentComboColor = color;
        }
        else
        {
            combo = 1;
        }
        comboEffect.gameObject.SetActive(true);
        comboEffect.SetCombo(combo);
    }

    public void ComboReset()
    {
        currentComboColor = -1;
        combo = 0;
    }

    public void GameOver()
    {
        if (Time.time - lastGameOverTime < gameOverCooldown)
        {
            // 대기 시간이 지나지 않았으므로 함수 실행 중단
            return;
        }

        if (useLastChance)
        {
            UIManager.Instance.ShowGameoverUI(score);

            if (score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", score);
                GPGSManager.Instance.RegisterRanking(score);
            }
        }
        else
        {
            eraseMode.LastChancePopupOn();
            useLastChance = true;
        }

        // 마지막 GameOver 호출 시간 업데이트
        lastGameOverTime = Time.time;
    }

    public void EraseMode()
    {
        combo = 0;
        eraseMode.LastChancePopupOn();
    }
}
