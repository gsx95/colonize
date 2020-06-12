using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Citizen : ScheduleFollower {

    private Dictionary<ResourceHolder.ResType, SatisfactionLevel> satisfaction = new Dictionary<ResourceHolder.ResType, SatisfactionLevel>();
    private string citizenName;

    void Awake()
    {
        base.me = this;
    }

    void Start() {
        satisfaction.Add(ResourceHolder.ResType.WATER, SatisfactionLevel.FULL);
        satisfaction.Add(ResourceHolder.ResType.FOOD, SatisfactionLevel.FULL);
        string TID = Clock.AddTimerInstantTrigger((id) => {
            Consume(ResourceHolder.ResType.FOOD, 0.25f);
        }, 3);
        Clock.AddTimerInstantTrigger((id) => {
            Consume(ResourceHolder.ResType.WATER, 0.25f);
        }, 3);
    }


    new void Update() {
        base.Update();
        CheckLiveliness();
    }

    // Consume Check //
    private void CheckLiveliness() {
        foreach (ResourceHolder.ResType resType in satisfaction.Keys) {
            var level = satisfaction[resType];
            if (level == SatisfactionLevel.NONE) {
                Die("no " + resType.ToString());
            }
        }
    }

    private void Consume(ResourceHolder.ResType resType, float amount=1f) {
        var satisfiying = ResourceHolder.ConsumeAllLeft(resType, amount);
        if (satisfiying) {
            var currentLevel = satisfaction[resType];
            if (currentLevel == SatisfactionLevel.FULL)
                return;
            satisfaction[resType] = currentLevel + 1;
        } else {
            var currentLevel = satisfaction[resType];
            if (currentLevel == SatisfactionLevel.NONE)
                return;
            satisfaction[resType] = currentLevel - 1;
        }
    }

    private void Die(string reason) {
        if (currentBuilding) {
            currentBuilding.CheckOut(this);
        }
        if (home != null)
            home.GetComponent<Housing>().RemoveResident(this);
        if (work != null)
            work.GetComponent<Factory>().RemoveEmployee(this);
        Census.RemoveCitizen(this);
        ToastBox.ShowMsg(citizenName + " died because of " + reason);
        Destroy(gameObject);
    }


    // SETTER //
    public void SetName(string name) {
        citizenName = name;
        gameObject.name = "Citizen (" + name + ")";
    }

    // GETTER //

    public string Name() {
        return citizenName;
    }

    public bool HasHome() {
        return home != null;
    }

    public bool HasWork() {
        return work != null;
    }

    public int GetHunger() {
        return (int)satisfaction[ResourceHolder.ResType.FOOD];
    }

    public int GetThirst() {
        return (int)satisfaction[ResourceHolder.ResType.WATER];
    }

    private enum SatisfactionLevel {
        FULL = 3,
        OKAY = 2,
        DREADING = 1,
        NONE = 0
    }
}
