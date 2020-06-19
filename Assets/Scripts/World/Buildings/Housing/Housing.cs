using System.Collections.Generic;
using UnityEngine;

public class Housing : Building {

    public int rooms = 1;

    private List<Citizen> residents = new List<Citizen>();

    void Awake() {
        HousingMarket.AddHouse(this);
    }
    void Start() {

    }


    void Update() {

    }

    public void AddResident(Citizen citizen) {
        residents.Add(citizen);
    }
    public int GetVacantRooms() {
        return rooms - residents.Count;
    }

    public int GetMaxRooms()
    {
        return rooms;
    }

    public int GetResidents()
    {
        return residents.Count;
    }

    public void RemoveResident(Citizen citizen) {
        residents.Remove(citizen);
    }
}
