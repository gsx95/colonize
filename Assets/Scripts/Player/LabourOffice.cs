using System.Collections.Generic;
using UnityEngine;

public class LabourOffice : MonoBehaviour {

    private static List<Factory> employers = new List<Factory>();

    public static void AddEmployer(Factory employer) {
        employers.Add(employer);
    }

    public static void RemoveEmployer(Factory employer) {
        employers.Remove(employer);
    }

    public static Factory GetVacantEmployer() {
        float minProd = 200f;
        Factory minProdFact = null;

        foreach (Factory factory in employers) {
            if (factory.VacantPositions() > 0) {
                if (factory.CurrentMaxProductivity() < minProd) {
                    minProd = factory.CurrentMaxProductivity();
                    minProdFact = factory;
                }
            }
        }
        if (minProdFact != null) {
            return minProdFact;
        }
        return null;
    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
