using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour
{

    private Dictionary<ResourceHolder.ResType, SatisfactionLevel> satisfaction = new Dictionary<ResourceHolder.ResType, SatisfactionLevel>();
    private string citizenName;

    private bool hasHousing = false;
    private bool hasWork = false;

    // Start is called before the first frame update
    void Start()
    {
        satisfaction.Add(ResourceHolder.ResType.WATER, SatisfactionLevel.FULL);
        satisfaction.Add(ResourceHolder.ResType.FOOD, SatisfactionLevel.FULL);
        Clock.AddTimerInstantTrigger(() => {

            Consume(ResourceHolder.ResType.FOOD);
        }, 3);
        Clock.AddTimerInstantTrigger(() => {
            Consume(ResourceHolder.ResType.WATER);
        }, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasHousing)
        {
            var housing = HousingMarket.GetVacantHouse();
            if (housing != null)
            {
                housing.AddResident(this);
                hasHousing = true;
            }
        }

        if(hasHousing && !hasWork)
        {
            var employer = LabourOffice.GetVacantEmployer();
            if(employer != null)
            {
                employer.AddEmployee(this);
                hasWork = true;
            }
        }

        foreach(ResourceHolder.ResType resType in satisfaction.Keys)
        {
            var level = satisfaction[resType];
            if(level == SatisfactionLevel.NONE)
            {
                Die("no " + resType.ToString());
            }
        }
    }

    public void SetName(string name)
    {
        citizenName = name;
        gameObject.name = "Citizen (" + name + ")";
    }

    private void Die(string reason)
    {
        Destroy(gameObject);
    }

    private void Consume(ResourceHolder.ResType resType)
    {
        var satisfiying = ResourceHolder.Consume(resType);
        if(satisfiying)
        {
            var currentLevel = satisfaction[resType];
            if (currentLevel == SatisfactionLevel.FULL)
                return;
            satisfaction[resType] = currentLevel + 1;
        } else
        {
            var currentLevel = satisfaction[resType];
            if (currentLevel == SatisfactionLevel.NONE)
                return;
            satisfaction[resType] = currentLevel - 1;
        }
    }



    private enum SatisfactionLevel
    {
        FULL = 3,
        OKAY = 2,
        DREADING = 1,
        NONE = 0
    }
}
