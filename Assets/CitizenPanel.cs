using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitizenPanel : MonoBehaviour
{
    public GameObject contentView;
    public GameObject prefab;

    private int activeEntries = 0;

    void Start() {
    }

    void Update() {
        if(activeEntries == Census.GetCitizensNum()) {
            return;
        }

        foreach (Transform child in contentView.transform) {
            Destroy(child.gameObject);
        }

        var citizens = Census.GetCitizens();
        activeEntries = citizens.Count;
        for(int i = 0; i < citizens.Count; i++) {
            var entry = Instantiate(prefab);
            entry.transform.SetParent(contentView.transform, false);
            entry.GetComponent<CitizenInfoElement>().SetCitizen(citizens[i], i+1);
        }
    }
}
