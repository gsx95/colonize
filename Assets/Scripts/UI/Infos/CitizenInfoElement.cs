using System;
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
        CitizenInfoWindow.ShowCitizen(citizen);

    }
}
