using UnityEngine;

public class CloseBtn : MonoBehaviour {
    public GameObject toShow;

    public void Clicked() {
        toShow.SetActive(true);
        gameObject.SetActive(false);
        PopupWindow.HidePopup();

        //disable all actions
        Placer.DisablePlacing();
    }
}
