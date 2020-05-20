using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Factory : MonoBehaviour
{

    protected int maxWorkers = 2;
    protected float productionDurationIGHours = 4;
    protected List<ResAmount> outputs = new List<ResAmount>();
    protected List<ResAmount> inputs = new List<ResAmount>();
    
    private List<Citizen> employees = new List<Citizen>();


    public void AddEmployee(Citizen citizen)
    {
        employees.Add(citizen);
    }

    public void RemoveEmployee(Citizen citizen)
    {
        employees.Remove(citizen);
    }

    public int VacantPositions()
    {
        return maxWorkers - employees.Count;
    }

    public void Awake()
    {
        LabourOffice.AddEmployer(this);
    }
    public void Start()
    {
        Debug.Log("start");
        Clock.AddTimer(() => { Produce();  }, productionDurationIGHours);
    }

    public float CurrentProductivity()
    {
        float productionPercentage = (float)employees.Count / (float)maxWorkers;
        return productionPercentage;
    }
    protected void Produce()
    {
        float productionPercentage = (float)employees.Count / (float)maxWorkers;

        Debug.Log("producing " + productionPercentage);
        Debug.Log("empl " + employees.Count);
        Debug.Log("max " + maxWorkers);
        List<ResAmount> actualInputs = new List<ResAmount>();
        foreach(ResAmount input in inputs)
        {
            float actualInput = input.GetAmount() * productionPercentage;
            actualInputs.Add(new ResAmount(input.GetResType(), actualInput));
        }
    
        List<ResAmount> actualOutputs = new List<ResAmount>();
        foreach (ResAmount output in outputs)
        {
            float actualOutput = output.GetAmount() * productionPercentage;
            actualOutputs.Add(new ResAmount(output.GetResType(), actualOutput));
        }

        if (ResourceHolder.CanAfford(actualInputs))
        {
            ResourceHolder.Consume(actualInputs);
            ResourceHolder.Produce(actualOutputs);
        }
    }
}
