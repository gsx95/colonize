using UnityEngine;
using UnityEngine.UI;

public class MenuBtn : MonoBehaviour {
    public Text text;
    public GameObject subUI;

    private bool sasAction = true;
    public string actionText = "Build";
    public string closeText = "Close";

    public void Start() {
        text.text = actionText;
    }
    public void Clicked() {
        if (sasAction) {
            sasAction = false;
            text.text = closeText;
            subUI.SetActive(true);
        } else {
            sasAction = true;
            text.text = actionText;
            subUI.SetActive(false);
        }
    }
}
