using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class FirebaseRankingSystem : MonoBehaviour
{
    public static FirebaseRankingSystem Instance;
    FirebaseAuth auth;

    FirebaseApp firebaseApp;
    DatabaseReference reference;

    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string HighScore = "HighScore";
    private const string RankingKey = "Ranking";

    string Token;
    string Error;
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

    private void Start()
    {
        firebaseApp = FirebaseApp.GetInstance("LineBall");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;

        // LoadRankingData();
    }

    private void LoadRankingData()
    {
        int currentHighScore = PlayerPrefs.GetInt(HighScore, 0);

        reference.Child(RankingKey).OrderByChild("timestamp").GetValueAsync().ContinueWith(task =>
        {
            try
            {
                if (task == null || task.IsFaulted || !task.IsCompleted || task.IsCanceled)
                {
                    Debug.LogError("Failed to read data: Task is null or not completed.");
                    return;
                }

                DataSnapshot snapshot = task.Result;

                Dictionary<string, int> sortedRanking = new Dictionary<string, int>();

                if (snapshot != null && snapshot.Exists)
                {
                    foreach (DataSnapshot userSnapshot in snapshot.Children)
                    {
                        string uid = userSnapshot.Key;

                        string username = userSnapshot.Child("name").Value.ToString();
                        int score = int.Parse(userSnapshot.Child("score").Value.ToString());

                        sortedRanking.Add(username, score);

                        if (auth.CurrentUser != null && uid == auth.CurrentUser.UserId)
                        {

                            // uIManager.SetMyRank(username, score);
                            if (currentHighScore < score)
                            {
                                // uIManager.highestScore = score;
                                //uIManager.highestScoretext.text = score.ToString();
                            }
                        }
                    }
                }

                SortRankingByScore(sortedRanking);
            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred: " + ex.Message);
            }
        });
    }


    private void SortRankingByScore(Dictionary<string, int> rankingData)
    {
        List<KeyValuePair<string, int>> sortedRanking = new List<KeyValuePair<string, int>>(rankingData);
        sortedRanking.Sort((x, y) => y.Value.CompareTo(x.Value));
        // uIManager.sortedRanking = sortedRanking;
    }

    public void RegisterRanking(string playerName, int score)
    {
        if (playerName == "Guest")
            return;

        if (auth.CurrentUser != null)
        {
            string uid = auth.CurrentUser.UserId;

            if (Application.platform == RuntimePlatform.Android)
            {
                uid = Token;
            }

            Debug.Log("User UID: " + uid);

            Rank newRank = new Rank(playerName, score);
            string json = JsonUtility.ToJson(newRank);

            reference.Child(RankingKey).Child(uid).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to register ranking data: " + task.Exception);
                    return;
                }

                if (task.IsCompleted)
                {
                    Debug.Log("Ranking data registered successfully.");
                    LoadRankingData();
                }
            });
        }
        else
        {
            Debug.Log("User is not logged in.");
        }
    }
}

