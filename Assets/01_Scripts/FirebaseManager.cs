using System;
using Firebase.Auth;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseApp app;

    void Start()
    {
        InitializePlayGames();
        PlayGamesVersionCheck();
    }

    void PlayGamesVersionCheck()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    void InitializePlayGames()
    {
        // PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        //     .RequestServerAuthCode(false) // 서버 인증 코드를 요청하지 않음
        //     .Build();

        // PlayGamesPlatform.InitializeInstance(config);
        // PlayGamesPlatform.DebugLogEnabled = true;
        // PlayGamesPlatform.Activate();

        // auth = FirebaseAuth.DefaultInstance;
        // SignInWithPlayGames();
    }

    void SignInWithPlayGames()
    {
        // Social.localUser.Authenticate((bool success) =>
        // {
        //     if (success)
        //     {
        //         Debug.Log("PlayGames 로그인 성공");
        //         string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
        //         SignInWithFirebase(authCode);
        //     }
        //     else
        //     {
        //         Debug.LogError("PlayGames 로그인 실패");
        //     }
        // });
    }

    void SignInWithFirebase(string authCode)
    {
        Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Firebase 로그인이 취소됨");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Firebase 로그인 실패: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase 사용자 로그인 성공: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });
    }
}
