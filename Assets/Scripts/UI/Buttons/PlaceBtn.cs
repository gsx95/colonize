using UnityEngine;

public class PlaceBtn : MonoBehaviour {

    public GameObject toShow;
    public void Clicked() {
        toShow.SetActive(true);
        gameObject.SetActive(false);
        Placer.PlaceCurrent();
        PopupWindow.HidePopup();
    }
}
