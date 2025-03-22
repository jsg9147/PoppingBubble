using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DarkTonic.MasterAudio;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public GameObject settingPanel;

    public ToggleController bgmToggle;
    public ToggleController sfxToggle;

    void Start()
    {
        LoadSoundSetting();
        CloseSetting();
    }

    void LoadSoundSetting()
    {
        bgmToggle.SetToggle(PlayerPrefs.GetInt("BGM", 1) == 1);
        sfxToggle.SetToggle(PlayerPrefs.GetInt("SFX", 1) == 1);

        bgmToggle.AddListener(SetBGM);
        sfxToggle.AddListener(SetSFX);
    }


    public void SetBGM(bool isOn)
    {
        // bool isOn = PlayerPrefs.GetInt("BGM", 1) != 1;
        MasterAudio.PlaylistMasterVolume = isOn ? 1 : 0;
        bgmToggle.SetToggle(isOn);
        PlayerPrefs.SetInt("BGM", isOn ? 1 : 0);
    }

    public void SetSFX(bool isOn)
    {
        // bool isOn = PlayerPrefs.GetInt("SFX", 1) != 1;
        MasterAudio.MixerMuted = !isOn;
        sfxToggle.SetToggle(isOn);
        PlayerPrefs.SetInt("SFX", isOn ? 1 : 0);
    }

    public void OpenSetting()
    {
        settingPanel.SetActive(true);
    }
    public void CloseSetting()
    {
        settingPanel.SetActive(false);
    }

    public void SceneReload()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
