using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuBtn : MonoBehaviour
{
    public Text text;
    public GameObject subUI;

    private bool saysBuild = true;
    private const string buildText = "Build";
    private const string closeText = "Close";

    public void Start()
    {
        text.text = buildText;
    }
    public void Clicked()
    {
        if(saysBuild)
        {
            saysBuild = false;
            text.text = closeText;
            subUI.SetActive(true);
        } else
        {
            saysBuild = true;
            text.text = buildText;
            subUI.SetActive(false);
        }
    }
}
