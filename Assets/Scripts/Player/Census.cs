using System.Collections.Generic;
using UnityEngine;

public class Census : MonoBehaviour {

    private static List<Citizen> citizens = new List<Citizen>();

    public static void AddCitizen(Citizen citizen) {
        citizens.Add(citizen);
    }

    public static void RemoveCitizen(Citizen citizen) {
        citizens.Remove(citizen);
    }

    public static int GetCitizensNum() {
        return citizens.Count;
    }
}
