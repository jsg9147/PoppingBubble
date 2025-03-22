using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject menuList;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenMenu()
    {
        menuList.SetActive(true);
    }

    public void CloseMenu()
    {
        menuList.SetActive(false);
    }
}
