using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraCapture : MonoBehaviour
{
    private RenderTexture renderTexture;

    public Camera captureCamera;
    public Image screenShotImg;

    private void Start()
    {
        renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        captureCamera.targetTexture = renderTexture;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.cameraCapture = this;
        }
    }

    public void CaptureAndApply()
    {
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        captureCamera.Render();
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        Sprite capturedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        screenShotImg.sprite = capturedSprite;

        captureCamera.targetTexture = null;
        Destroy(renderTexture);
    }
}
