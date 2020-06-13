using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingClicker : MonoBehaviour
{

    private Updating hitter = null;
    private static BuildingClicker Instance;

    void Awake()
    {
        Instance = this;
    }

    class Hitter<BuildingType> : Updating
    {
        private Action<BuildingType> buildingClicked;
        private Ray ray;
        private RaycastHit hit;

        public Hitter(Action<BuildingType> buildingClicked)
        {
            this.buildingClicked = buildingClicked;
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                int layer_mask = LayerMask.GetMask("Building");
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask))
                {
                    if (hit.collider.tag == "Building")
                    {
                        BuildingType building;
                        if (hit.collider.gameObject.TryGetComponent<BuildingType>(out building))
                        {
                            buildingClicked(building);
                        }
                    }
                }
            }
        }
    }

    public static void DisableBuildingClicker()
    {
        Instance.DisableClicking();
    }

    public static void EnableBuildingClicker<T>(Action<T> buildingClicked)
    {
        Instance.EnableClicking<T>(buildingClicked);
    }


    private void DisableClicking()
    {
        hitter = null;
    }

    private void EnableClicking<T>(Action<T> buildingClicked)
    {
        hitter = new Hitter<T>(buildingClicked);
    }

    void Update()
    {
        if(hitter == null)
            return;
        hitter.Update();
    }
}
