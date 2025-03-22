using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayLog : MonoBehaviour
{
    private List<string> logMessages = new List<string>();
    private Vector2 scrollPosition;
    private GUIStyle logStyle;

    void Start()
    {
        if (Application.isEditor == true)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;

        // 글자 스타일 초기화
        logStyle = new GUIStyle();
        logStyle.fontSize = 50; // 글자 크기를 원하는 크기로 설정
        logStyle.normal.textColor = Color.black; // 텍스트 색상을 흰색으로 설정
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logMessages.Add(logString);
        if (logMessages.Count > 100)
        {
            logMessages.RemoveAt(0); // 오래된 로그를 삭제하여 메모리 절약
        }

    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(50, 50, Screen.width - 100, Screen.height - 100));
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        foreach (string log in logMessages)
        {
            GUILayout.Label(log, logStyle); // logStyle을 사용하여 글자 크기 적용
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}
