using System;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Building {

    protected int maxWorkers = 2;
    protected float productionDurationPerWorkerIGHours = 4;
    protected List<ResAmount> outputs = new List<ResAmount>();
    protected List<ResAmount> inputs = new List<ResAmount>();
    private List<Citizen> employees = new List<Citizen>();

    private float productionRound = 0f;

    public void AddEmployee(Citizen citizen) {
        employees.Add(citizen);
    }

    public void RemoveEmployee(Citizen citizen) {
        employees.Remove(citizen);
    }

    public int VacantPositions() {
        return maxWorkers - employees.Count;
    }

    public int getCurrent() {
        return citizens.Count;
    }
    public int getEmployees() {
        return employees.Count;
    }
    public int getMax() {
        return maxWorkers;
    }
    public float getValue() {
        return productionRound / 100f;
    }

    public void Awake() {
        LabourOffice.AddEmployer(this);
    }
    public void Start() {
        Clock.AddTimer((id) => { Produce(); }, productionDurationPerWorkerIGHours / 10f);
    }

    public float CurrentMaxProductivity() {
        float productionPercentage = (float)employees.Count / (float)maxWorkers;
        return productionPercentage;
    }

    public void ShowInfo() {
        FactoryInfo.display(this);
    }

    private float CitizenFactor()
    {
        float val = 0f;
        foreach(Citizen c in citizens)
        {
            float hungerFactor = ((float)c.GetHunger() / 3f);
            float thirstFactor = ((float)c.GetThirst() / 3f);
            float min = Mathf.Min(hungerFactor, thirstFactor);
            val += min;
        }
        return val;
    }

    protected void Produce() {
        float maxPercentageToAdd = 100f / 10f;
        float percentageToAdd = maxPercentageToAdd * CitizenFactor();
        productionRound += percentageToAdd;

        if(productionRound < 100f) {
            return;
        }
        productionRound = 0f;
        List<ResAmount> actualInputs = new List<ResAmount>();
        foreach (ResAmount input in inputs) {
            actualInputs.Add(new ResAmount(input.GetResType(), input.GetAmount()));
        }

        List<ResAmount> actualOutputs = new List<ResAmount>();
        foreach (ResAmount output in outputs) {
            actualOutputs.Add(new ResAmount(output.GetResType(), output.GetAmount()));
        }

        if (ResourceHolder.CanAfford(actualInputs)) {
            ResourceHolder.Consume(actualInputs);
            ResourceHolder.Produce(actualOutputs);
        }
    }
}
