using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{

    public Image b1;
    public Image b2;
    public Image b3;

    public Color green;
    public Color orange;
    public Color red;

    public void SetStatus(int status) {
        switch(status) {
            case 0:
                b1.gameObject.SetActive(false);
                b2.gameObject.SetActive(false);
                b3.gameObject.SetActive(false);
                break;
            case 1:
                b1.gameObject.SetActive(true);
                b2.gameObject.SetActive(false);
                b3.gameObject.SetActive(false);
                break;
            case 2:
                b1.gameObject.SetActive(true);
                b2.gameObject.SetActive(true);
                b3.gameObject.SetActive(false);
                break;
            case 3:
                b1.gameObject.SetActive(true);
                b2.gameObject.SetActive(true);
                b3.gameObject.SetActive(true);
                break;
        }
        SetColors(status);
    }

    private void SetColors(int status) {
        switch (status) {
            case 0:
                break;
            case 1:
                b1.color = red;
                b2.color = red;
                b3.color = red;
                break;
            case 2:
                b1.color = orange;
                b2.color = orange;
                b3.color = orange;
                break;
            case 3:
                b1.color = green;
                b2.color = green;
                b3.color = green;
                break;
        }
    }
}
