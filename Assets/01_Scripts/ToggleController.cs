using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    public Toggle toggle;
    public GameObject onObject;
    public GameObject offObject;

    void Start()
    {
        toggle.onValueChanged.AddListener((isOn) => ToggleObject());
    }

    public void AddListener(UnityEngine.Events.UnityAction<bool> action)
    {
        toggle.onValueChanged.AddListener(action);
    }

    public void SetToggle(bool isOn)
    {
        toggle.isOn = isOn;
        ToggleObject();
    }

    void ToggleObject()
    {
        onObject.SetActive(toggle.isOn);
        offObject.SetActive(!toggle.isOn);
    }
}
