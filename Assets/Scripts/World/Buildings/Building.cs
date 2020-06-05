using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public GameObject entrance;
    public GameObject exit;
    public GameObject bottom;

    protected List<Citizen> citizens = new List<Citizen>();

    protected void SpawnCitizen(Citizen citizen) {

    }

    public void CheckIn(Citizen citizen) {
        citizens.Add(citizen);
    }

    public void CheckOut(Citizen citizen) {
        citizens.Remove(citizen);
    }
}
