using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenInfoWindow : MonoBehaviour
{
    private Citizen citizen;

    private static CitizenInfoWindow Instance;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ChangeHomeClicked()
    {
        UIController.HideArrows();
        gameObject.SetActive(false);
        BuildingClicker.EnableBuildingClicker<Housing>((h) => {
            citizen.SetHome(h);
            ShowCitizen(citizen);
        });
    }

    public void ChangeWorkClicked()
    {
        UIController.HideArrows();
        gameObject.SetActive(false);
        BuildingClicker.EnableBuildingClicker<Factory>((f) => {
            citizen.SetWork(f);
            ShowCitizen(citizen);
        });
    }

    public static void ShowCitizen(Citizen citizen)
    {
        Instance.citizen = citizen;
        Instance.gameObject.SetActive(true);
        Func<Vector3> homePosFunc = null;
        Func<Vector3> workPosFunc = null;
        if (citizen.Home())
        {
            homePosFunc = () =>
            {
                var pos = citizen.Home().transform.position;
                pos.y += 1;
                return pos;
            };
        }
        if (citizen.Workplace())
        {
            workPosFunc = () =>
            {
                var pos = citizen.Workplace().transform.position;
                pos.y += 1;
                return pos;
            };
        }
        UIController.ShowCitizenArrows(() => { return citizen.transform.position + new Vector3(0, 0.5f, 0); }, homePosFunc, workPosFunc);
    }

    public void Hide()
    {
        citizen = null;
        UIController.HideArrows();
    }
    
}
