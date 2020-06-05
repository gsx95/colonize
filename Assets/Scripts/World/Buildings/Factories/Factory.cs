using System.Collections.Generic;
using UnityEngine;

public class Factory : Building {

    protected int maxWorkers = 2;
    protected float productionDurationIGHours = 4;
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
        Clock.AddTimer(() => { Produce(); }, (productionDurationIGHours / 60));
    }

    public float CurrentMaxProductivity() {
        float productionPercentage = (float)employees.Count / (float)maxWorkers;
        return productionPercentage;
    }

    public void ShowInfo() {
        FactoryInfo.display(this);
    }

    protected void Produce() {
        float maxPercentageToAdd = 100f / 60f;
        float productionPercentage = (float)citizens.Count / (float)maxWorkers;
        float percentageToAdd = maxPercentageToAdd * productionPercentage;
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
