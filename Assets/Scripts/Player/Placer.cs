using UnityEngine;
using UnityEngine.EventSystems;

public class Placer : MonoBehaviour {
    private static bool isPlacing = false;
    private static GameObject waitingForPlacement = null;


    Ray ray;
    RaycastHit hit;
    private static GameObject pref;
    private static IPlacingListener placedListener;
    private static GameObject lastTry;


    public static void EnablePlacing(GameObject toPlace, IPlacingListener placedListener = null) {
        pref = toPlace;
        isPlacing = true;
        Placer.placedListener = placedListener;
    }

    public static void DisablePlacing() {
        isPlacing = false;
        if (lastTry != null) {
            Destroy(lastTry);
        }
        waitingForPlacement = null;
        lastTry = null;
    }

    public static void PlaceCurrent() {
        if (waitingForPlacement != null) {
            var ph = waitingForPlacement.GetComponent<BuildingPlaceHolder>();
            if (ph != null) {
                ph.Place();
                waitingForPlacement = null;
                lastTry = null;
                placedListener.Placed(waitingForPlacement);
            }
        }
    }

    void Update() {
        if (!isPlacing) {
            return;
        }
        if (waitingForPlacement != null) {
            tryPlacing(waitingForPlacement);
        }

        if (Input.GetMouseButton(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }
            ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            int layer_mask = LayerMask.GetMask("Plane");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask)) {
                if (hit.collider.tag == "Plane") {
                    var yPos = pref.transform.position.y;
                    var newGo = Instantiate(pref, hit.point, Quaternion.identity);
                    var pos = newGo.transform.position;
                    newGo.transform.position = new Vector3(pos.x, yPos, pos.z);
                    waitingForPlacement = newGo;
                    if (lastTry != null) {
                        Destroy(lastTry);
                        lastTry = null;
                    }
                    lastTry = newGo;
                }
            }
        }
    }

    private void tryPlacing(GameObject tryObj) {
        var ph = tryObj.GetComponent<BuildingPlaceHolder>();
        if (!ResourceHolder.CanAfford(ph.costs)) {
            PopupWindow.ShowPopup("You cannot afford this.");
            SetNotPlacable(tryObj);
            return;
        }

        BuildingPlaceHolder.State state = ph.state;
        if (state == BuildingPlaceHolder.State.NONE) {
            return;
        } else if (state == BuildingPlaceHolder.State.PLACING_NOT_POSSIBLE) {
            SetNotPlacable(tryObj);
        } else if (state == BuildingPlaceHolder.State.PLACING) {
            SetPlacable(tryObj);
        }
    }

    private void SetNotPlacable(GameObject tryObj) {
        lastTry = tryObj;
        tryObj.GetComponent<BuildingPlaceHolder>().SetRed();
        placedListener.CannotBePlaced();
    }

    private void SetPlacable(GameObject tryObj) {
        lastTry = tryObj;
        tryObj.GetComponent<BuildingPlaceHolder>().SetGreen();
        placedListener.CanBePlaced();
    }

    public interface IPlacingListener {
        void Placed(GameObject placedGo);
        void CanBePlaced();
        void CannotBePlaced();
    }
}

