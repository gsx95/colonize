using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldClicker : MonoBehaviour
{

    Ray ray;
    RaycastHit hit;


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }
            ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            int layer_mask = LayerMask.GetMask("Building");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask)) {
                if (hit.collider.tag == "Building") {
                    Factory factory;
                    if(hit.collider.gameObject.TryGetComponent<Factory>(out factory)) {
                        factory.ShowInfo();
                    }
                }
            }
        }
    }
}