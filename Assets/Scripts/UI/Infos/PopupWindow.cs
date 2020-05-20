using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{
    public GameObject panel;
    public Text text;

    private static PopupWindow Instance;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        text.text = "";
        panel.SetActive(false);
    }

    public static void ShowPopup(string message)
    {
        Instance.text.text = message;
        Instance.panel.SetActive(true);
    }

    public static void HidePopup()
    {
        Instance.text.text = "";
        Instance.panel.SetActive(false);
    }
}
