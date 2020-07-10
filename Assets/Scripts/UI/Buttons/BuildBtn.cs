using UnityEngine;

public class BuildBtn : MonoBehaviour, Placer.IPlacingListener {
    public GameObject buildObj;
    public GameObject uiToHide;
    public GameObject closeBtn;
    public GameObject placeBtn;

    public void Clicked() {
        Placer.EnablePlacing(buildObj, this);
        placeBtn.SetActive(false);
        uiToHide.SetActive(false);
        closeBtn.SetActive(true);
    }

    public void Placed(GameObject placedGo) {
        Placer.DisablePlacing();
        placeBtn.SetActive(false);
        uiToHide.SetActive(true);
        closeBtn.SetActive(false);
        NavMeshController.Bake();

    }

    public void CanBePlaced() {
        placeBtn.SetActive(true);
    }

    public void CannotBePlaced() {
        placeBtn.SetActive(false);
    }
}
