using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using DarkTonic.MasterAudio;
public class VolumeToggle : MonoBehaviour
{
    private const string soundKey = "IsSoundOn";
    private const string bgmKey = "IsBgmOn";

    public SettingManager settingManager;
    public Image buttonImage;

    public Sprite onSprite;
    public Sprite offSprite;

    private bool isSoundOn = false;

    public bool isSound = false;

    void Start()
    {
        if (isSound)
            isSoundOn = settingManager.isSoundOn;
        else
            isSoundOn = settingManager.isBgmOn;

        GetComponent<Button>().onClick.AddListener(ToggleVolume);
        UpdateButtonImage();
    }

    void ToggleVolume()
    {
        isSoundOn = !isSoundOn;

        // if (isSound)
        // {
        //     isSoundOn = settingManager.SoundToggle();
        //     MasterAudio.MixerMuted = !isSoundOn;
        // }
        // else
        // {
        //     isSoundOn = settingManager.BGMToggle();
        //     MasterAudio.PlaylistsMuted = !isSoundOn;
        //     if (isSoundOn)
        //     {
        //         MasterAudio.StartPlaylist("BGM");
        //     }
        // }
        UpdateButtonImage();
    }

    void UpdateButtonImage()
    {
        if (isSoundOn)
        {
            buttonImage.sprite = onSprite;
        }
        else
        {
            buttonImage.sprite = offSprite;
        }
    }
}
