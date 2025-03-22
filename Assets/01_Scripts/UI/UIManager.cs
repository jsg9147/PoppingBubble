using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public ScoreUI scoreUI;
    public GameoverUI gameoverUI;
    public CameraCapture cameraCapture;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }

    public void UpdateScore(int score)
    {
        if (scoreUI != null)
        {
            scoreUI.SetScore(score);
        }
    }

    public void ShowGameoverUI(int score)
    {
        if (gameoverUI != null)
        {
            gameoverUI.ShowGameoverUI(score);
            CaptureAndApply();
        }
    }

    public void CaptureAndApply()
    {
        if (cameraCapture != null)
        {
            cameraCapture.CaptureAndApply();
        }
    }

    // private const string HighestScoreKey = "HighestScore";

    // public CameraCapture cameraCapture;

    // public Button menuListBtn;
    // public Button menuListOffBtn;
    // public Button leaderBoardBtn;

    // public Button gameOverRestartBtn;
    // public Button gameOverLeaderBoardBtn;

    // public Transform menuListTransform;

    // public TMP_Text scoreText;
    // public TMP_Text endScoreText;

    // public HighScoreTrophy highScore;
    // public TMP_Text highestScoretext;

    // public int highestScore;

    // public GameObject eraseUI;
    // public GameObject rankingUI;

    // public BallPreview ballPreview;

    // public RankingItem rankingItemPrefab;
    // public RankingItem myRankingItem;

    // public Transform rankingContent;

    // public LastChance lastChancePopup;

    // bool menuListOn = false;

    // public List<KeyValuePair<string, int>> sortedRanking;

    // List<RankingItem> rankingItems;

    // public TMP_InputField nameField;

    // string myName;
    // int myScore;

    // private void Start()
    // {
    //     menuListBtn.onClick.AddListener(MenuListOnOff);
    //     menuListOffBtn.onClick.AddListener(MenuListOnOff);

    //     menuListTransform.localScale = new(1, 0, 1);
    //     leaderBoardBtn.onClick.AddListener(() =>
    //     {
    //         rankingUI.SetActive(true);
    //         SetRankingItem();
    //     });

    //     gameOverRestartBtn.onClick.AddListener(SceneReload);
    //     gameOverLeaderBoardBtn.onClick.AddListener(() =>
    //     {
    //         rankingUI.SetActive(true);
    //         SetRankingItem();
    //     });

    //     rankingItems = new();

    //     for (int i = 0; i < 100; i++)
    //     {
    //         RankingItem rankingItem = Instantiate(rankingItemPrefab, rankingContent);
    //         rankingItems.Add(rankingItem);
    //         rankingItem.gameObject.SetActive(false);
    //     }
    //     nameField.onValueChanged.AddListener(SaveInputFieldValue);

    //     SetHighestScoretext();

    // }

    // private void SaveInputFieldValue(string newValue)
    // {
    //     PlayerPrefs.SetString("PlayerName", newValue);
    // }

    // void MenuListOnOff()
    // {
    //     float size = menuListOn ? 0f : 1f;
    //     menuListTransform.DOScaleY(size, 0.1f);

    //     menuListOn = size == 1f;
    // }

    // public void ScoreUpdate(int score)
    // {
    //     scoreText.text = score.ToString();
    //     if (score >= PlayerPrefs.GetInt(HighestScoreKey, 0))
    //     {
    //         HighestScoreUpdate(score);
    //     }
    // }

    // public void EraseMode(bool isActive)
    // {
    //     eraseUI.SetActive(isActive);
    // }

    // public void SetHighestScoretext()
    // {
    //     highScore.SetScore(PlayerPrefs.GetInt(HighestScoreKey, 0));
    //     highestScoretext.text = PlayerPrefs.GetInt(HighestScoreKey, 0).ToString();

    //     if (highestScore < PlayerPrefs.GetInt(HighestScoreKey, 0))
    //         highestScore = PlayerPrefs.GetInt(HighestScoreKey, 0);
    //     else
    //         PlayerPrefs.SetInt(HighestScoreKey, highestScore);
    // }

    // void HighestScoreUpdate(int score)
    // {
    //     highestScoretext.text = score.ToString();
    //     highestScore = score;
    //     PlayerPrefs.SetInt(HighestScoreKey, score);
    // }

    // public void SetBallPreview(Ball nextBall)
    // {
    //     // ballPreview.SetImage(nextBall);
    // }

    // public void SetRankingItem()
    // {
    //     RankItemOff();
    //     int rank = 0;
    //     bool isRanker = false;
    //     try
    //     {
    //         myRankingItem.nameText.text = myName;
    //         myRankingItem.SetScore(myScore);

    //         foreach (var rankData in sortedRanking)
    //         {
    //             if (sortedRanking.Count > rank)
    //             {
    //                 rankingItems[rank].SetRankingText(rank);
    //                 rankingItems[rank].nameText.text = rankData.Key;
    //                 rankingItems[rank].SetScore(rankData.Value);
    //                 rankingItems[rank].gameObject.SetActive(true);
    //                 rank++;

    //                 if (myRankingItem != null)
    //                 {

    //                     if (myRankingItem.score == rankingItems[rank].score)
    //                     {
    //                         isRanker = true;
    //                         myRankingItem.SetRankingText(rank);
    //                     }
    //                 }

    //             }
    //         }

    //         if (isRanker == false)
    //             myRankingItem.SetRankingText(100);
    //     }
    //     catch (NullReferenceException ex)
    //     {
    //         print(ex);
    //     }
    // }

    // public void SetMyRank(string name, int score)
    // {
    //     myName = name;
    //     myScore = (score);
    // }

    // void RankItemOff()
    // {
    //     for (int i = 0; i < rankingItems.Count; i++)
    //     {
    //         rankingItems[i].gameObject.SetActive(false);
    //     }
    // }

    // public void RankingPopupOff()
    // {
    //     rankingUI.SetActive(false);
    // }

    // public void LastChancePopupON()
    // {
    //     Time.timeScale = 0;
    //     lastChancePopup.gameObject.SetActive(true);
    // }

    // public void GameOverUI()
    // {
    //     if (highestScore > PlayerPrefs.GetInt(HighestScoreKey, 0))
    //         PlayerPrefs.SetInt(HighestScoreKey, highestScore);

    //     cameraCapture.CaptureAndApply();
    //     cameraCapture.gameOverUIPanel.SetActive(true);
    // }
    // void SceneReload()
    // {
    //     Time.timeScale = 1;
    //     SceneManager.LoadScene(0);
    // }

}
