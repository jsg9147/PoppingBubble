using System;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System.Linq;


public class GPGSManager : MonoBehaviour
{
    private const string HighScore = "HighScore";
    public static GPGSManager Instance;

    public List<Rank> rankingList = new List<Rank>();

    FirebaseAuth auth;
    DatabaseReference reference;

    public string Token;
    public string Error;

    public bool isLeaderboardLoaded = false;

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

        PlayGamesPlatform.Activate();
    }

    async void Start()
    {
        await InitializeServices();
    }
    private async Task InitializeServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
            auth = FirebaseAuth.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;

            await LoginGooglePlayGames();
            await SignInWithGooglePlayGamesAsync();
            await LoadRankingDataAsync();
            // FirebaseSignInWithGooglePlayGames(Token);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Initialization failed: {ex.Message}");
        }
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            Debug.Log("Login with Google Play games successful.");
        }
        else
        {
            Error = "Failed to retrieve Google play games authorization code";
            Debug.Log("Login Unsuccessful");
        }
    }
    //Fetch the Token / Auth code
    public Task LoginGooglePlayGames()
    {
        var tcs = new TaskCompletionSource<object>();
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    Token = code;
                    // This token serves as an example to be used for SignInWithGooglePlayGames
                    Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(Token);
                    auth.SignInWithCredentialAsync(credential);
                    tcs.SetResult(null);
                });
            }
            else
            {
                Error = "Failed to retrieve Google play games authorization code";
                Debug.Log("Login Unsuccessful");
                tcs.SetException(new Exception("Failed"));
            }
        });
        return tcs.Task;
    }

    async Task SignInWithGooglePlayGamesAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(Token);
            Social.localUser.Authenticate((bool success) =>
           {
               if (success)
               {
                   Debug.Log("Login with Google Play games successful.");
               }
               else
               {
                   Debug.Log("Login Unsuccessful");
               }
           });
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); //Display the Unity Authentication PlayerID
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public async Task LoadRankingDataAsync()
    {
        Debug.Log("Loading ranking data...");
        try
        {
            rankingList = new();
            int currentHighScore = PlayerPrefs.GetInt(HighScore, 0);
            // Debug.Log(PlayGamesPlatform.Instance.localUser.userName);

            var snapshot = await reference.Child("Ranking").OrderByChild("timestamp").GetValueAsync();

            if (snapshot != null && snapshot.Exists)
            {
                Debug.Log(snapshot.ChildrenCount + " users found.");
                var sortedRanking = snapshot.Children
                    .Select(userSnapshot => new
                    {
                        Uid = userSnapshot.Key,
                        Username = userSnapshot.Child("name").Value.ToString(),
                        Score = int.Parse(userSnapshot.Child("score").Value.ToString())
                    })
                    .OrderByDescending(user => user.Score)
                    .ToList();

                foreach (var user in sortedRanking)
                {
                    Debug.Log($"{user.Username} {user.Score} {user.Uid}");
                }

                rankingList = sortedRanking.Select(user => new Rank(user.Username, user.Score, user.Uid)).ToList();
                isLeaderboardLoaded = true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to read data: {ex.Message}");
        }
    }

    public async Task LoadRankingData()
    {
        await LoadRankingDataAsync();
    }
    public async void RegisterRanking(int score)
    {
        if (auth.CurrentUser != null)
        {
            string uid = auth.CurrentUser.UserId;

            if (Application.platform == RuntimePlatform.Android)
            {
                uid = Token;
            }

            Debug.Log("User UID: " + uid);

            Rank newRank = new Rank(auth.CurrentUser.DisplayName, score);
            string json = JsonUtility.ToJson(newRank);

            try
            {
                await reference.Child("Ranking").Child(uid).SetRawJsonValueAsync(json);
                Debug.Log("Ranking data registered successfully.");
                await LoadRankingData();
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to register ranking data: " + ex.Message);
            }
        }
        else
        {
            Debug.Log("User is not logged in.");
        }
    }

    // public void RegisterRanking(int score)
    // {
    //     if (auth.CurrentUser != null)
    //     {
    //         string uid = auth.CurrentUser.UserId;

    //         if (Application.platform == RuntimePlatform.Android)
    //         {
    //             uid = Token;
    //         }

    //         Rank newRank = new Rank(auth.CurrentUser.DisplayName, score);
    //         string json = JsonUtility.ToJson(newRank);

    //         reference.Child("Ranking").Child(uid).SetRawJsonValueAsync(json).ContinueWith(task =>
    //         {
    //             if (task.IsFaulted)
    //             {
    //                 Debug.LogError("Failed to register ranking data: " + task.Exception);
    //                 return;
    //             }

    //             if (task.IsCompleted)
    //             {
    //                 Debug.Log("Ranking data registered successfully.");
    //                 isLeaderboardLoaded = false;
    //             }
    //         });
    //     }
    //     else
    //     {
    //         Debug.Log("User is not logged in.");
    //     }
    // }
}
