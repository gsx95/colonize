using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CitizenInfoElement : MonoBehaviour
{
    public StatusBar hunger;
    public StatusBar thirst;
    public GameObject homeAlarm;
    public GameObject workAlarm;
    public Text number;
    public Text citizenName;

    private Citizen citizen;
    private int num;


    public void SetCitizen(Citizen citizen, int num) {
        this.citizen = citizen;
        this.num = num;
    }

    void Update() {
        if (citizen != null) {
            this.citizenName.text = citizen.Name();
            this.number.text = "#" + num;
            if(citizen.HasHome()) {
                homeAlarm.SetActive(false);
            }
            if(citizen.HasWork()) {
                workAlarm.SetActive(false);
            }
            hunger.SetStatus(citizen.GetHunger());
            thirst.SetStatus(citizen.GetThirst());
        }
    }

    public void Clicked()
    {
        Vector3 homePos = new Vector3(0, 0, 0);
        Vector3 workPos = new Vector3(0, 0, 0);
        if (citizen.Home()) {
            var hPos = citizen.Home().transform.position;
            hPos.y += 1;
            homePos = Camera.main.WorldToScreenPoint(hPos);
        }
        if(citizen.Workplace())
        {
            var wPos = citizen.Workplace().transform.position;
            wPos.y += 1;
            workPos = Camera.main.WorldToScreenPoint(wPos);
        }
        UIController.ShowCitizenArrows(homePos, workPos);

    }
}
