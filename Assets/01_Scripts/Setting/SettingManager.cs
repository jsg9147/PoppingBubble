using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using DarkTonic.MasterAudio;

public class SettingManager : MonoBehaviour
{
    private const string soundKey = "IsSoundOn";
    private const string bgmKey = "IsBgmOn";

    public GameObject settingPopup;

    public Button settingPopupOnButton;
    public Button reStartButton;
    public Button continueButton;

    [HideInInspector] public bool isBgmOn = false;
    [HideInInspector] public bool isSoundOn = false;

    private void Start()
    {
        settingPopup.SetActive(false);

        reStartButton.onClick.AddListener(SceneReload);
        continueButton.onClick.AddListener(PopupOff);
        settingPopupOnButton.onClick.AddListener(PopupOn);

        LoadBeforeSetting();
    }

    void SceneReload()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    void PopupOff()
    {
        settingPopup.SetActive(false);
        Time.timeScale = 1;
    }

    void PopupOn()
    {
        settingPopup.SetActive(true);
        Time.timeScale = 0;
    }

    void LoadBeforeSetting()
    {
        isSoundOn = PlayerPrefs.GetInt(soundKey, 1) == 1 ? true : false;
        isBgmOn = PlayerPrefs.GetInt(bgmKey, 1) == 1 ? true : false;

        // MasterAudio.MixerMuted = !isSoundOn;
        // MasterAudio.PlaylistsMuted = !isBgmOn;
    }

    public bool BGMToggle()
    {
        isBgmOn = !isBgmOn;

        int valueToSave = isBgmOn ? 1 : 0;
        PlayerPrefs.SetInt(bgmKey, valueToSave);
        PlayerPrefs.Save();

        return isBgmOn;
    }

    public bool SoundToggle()
    {
        isSoundOn = !isSoundOn;

        int valueToSave = isSoundOn ? 1 : 0;
        PlayerPrefs.SetInt(soundKey, valueToSave);
        PlayerPrefs.Save();

        return isSoundOn;
    }
}
