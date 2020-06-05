using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Citizen : MonoBehaviour {

    private Dictionary<ResourceHolder.ResType, SatisfactionLevel> satisfaction = new Dictionary<ResourceHolder.ResType, SatisfactionLevel>();
    private string citizenName;

    private Building home = null;
    private Building work = null;

    private int workStart = 8;
    private int leisureStart = 17;
    private int sleepStart = 22;

    private Building walkTarget;
    private bool isWalking = false;
    public Schedule activeSchedule = Schedule.NONE;
    public Schedule targetSchedule = Schedule.NONE;


    private Building currentBuilding;

    void Awake() {
        // typical schedule:   8.00 - 17.00 work  |  17.00 - 22.00 leisure time | 22.00 - 8.00 home/sleep
        // + 1 / -2 hours
        int offset = Random.Range(-2, 2);
        workStart += offset;
        leisureStart += offset;
        sleepStart += offset;

        if(sleepStart >= 24) {
            sleepStart -= 24;
        }
    }

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
        return (int) satisfaction[ResourceHolder.ResType.FOOD];
    }

    public int GetThirst() {
        return (int)satisfaction[ResourceHolder.ResType.WATER];
    }

    void Start() {
        satisfaction.Add(ResourceHolder.ResType.WATER, SatisfactionLevel.FULL);
        satisfaction.Add(ResourceHolder.ResType.FOOD, SatisfactionLevel.FULL);
        Clock.AddTimerInstantTrigger(() => {
            Consume(ResourceHolder.ResType.FOOD);
        }, 3);
        Clock.AddTimerInstantTrigger(() => {
            Consume(ResourceHolder.ResType.WATER);
        }, 3);
    }


    void Update() {
        if (home == null || work == null) {
            SearchHomeAndWork();
        }

        CheckLiveliness();
        SetSchedule();
        if(isWalking)
            Walk();
    }

    private void Walk() {
        float step = 1.0f * Time.deltaTime;
        var pos = walkTarget.transform.position;
        pos.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, pos, step);
    }
    private void SetSchedule() {
        var currentTime = Clock.GetCurrentTime();

        if(currentTime.hour >= sleepStart || currentTime.hour < workStart) {
            SleepTime();
            return;
        }

        if(currentTime.hour >= leisureStart || work == null) {
            LeisureTime();
            return;
        }

        if(currentTime.hour >= workStart) {
            WorkTime();
            return;
        }
        throw new System.Exception("I dont know what to do! " + currentTime.hour + "  " + workStart + " " + leisureStart + " " + sleepStart);
    }

    void OnTriggerEnter(Collider other) {
        Building building;
        if(other.gameObject.TryGetComponent(out building)) {
            if(building == walkTarget) {
                building.CheckIn(this);
                currentBuilding = building;
                GetComponent<Renderer>().enabled = false;
                isWalking = false;
                activeSchedule = targetSchedule;
            }
        }
    }

    private void SleepTime() {
        if (targetSchedule == Schedule.SLEEP)
            return;
        if (isWalking == false && walkTarget != null && activeSchedule != Schedule.SLEEP) {
            transform.position = walkTarget.exit.transform.position;
            walkTarget.CheckOut(this);
        }
        walkTarget = home;
        isWalking = true;
        GetComponent<Renderer>().enabled = true;
        targetSchedule = Schedule.SLEEP;
        activeSchedule = Schedule.NONE;
    }

    private void LeisureTime() {
        if (targetSchedule == Schedule.LEISURE)
            return;
        if (isWalking == false && walkTarget != null && activeSchedule != Schedule.LEISURE) {
            transform.position = walkTarget.exit.transform.position;
            walkTarget.CheckOut(this);
        }
        walkTarget = home;
        isWalking = true;
        GetComponent<Renderer>().enabled = true;
        targetSchedule = Schedule.LEISURE;
        activeSchedule = Schedule.NONE;
    }

    private void WorkTime() {
        if (targetSchedule == Schedule.WORK)
            return;
        if (isWalking == false && walkTarget != null && activeSchedule != Schedule.WORK) {
            transform.position = walkTarget.exit.transform.position;
            walkTarget.CheckOut(this);
        }
        walkTarget = work;
        isWalking = true;
        GetComponent<Renderer>().enabled = true;
        targetSchedule = Schedule.WORK;
        activeSchedule = Schedule.NONE;
    }

    private void CheckLiveliness() {
        foreach (ResourceHolder.ResType resType in satisfaction.Keys) {
            var level = satisfaction[resType];
            if (level == SatisfactionLevel.NONE) {
                Die("no " + resType.ToString());
            }
        }
    }

    private void SearchHomeAndWork() {
        if (home == null) {
            var housing = HousingMarket.GetVacantHouse();
            if (housing != null) {
                housing.AddResident(this);
                home = housing;
            }
        }

        if (home != null && work == null) {
            var employer = LabourOffice.GetVacantEmployer();
            if (employer != null) {
                employer.AddEmployee(this);
                work = employer;
            }
        }
    }

    public void SetName(string name) {
        citizenName = name;
        gameObject.name = "Citizen (" + name + ")";
    }

    private void Die(string reason) {
        if(currentBuilding) {
            currentBuilding.CheckOut(this);
        }
        if(home != null)
            home.GetComponent<Housing>().RemoveResident(this);
        if(work != null)
            work.GetComponent<Factory>().RemoveEmployee(this);
        Census.RemoveCitizen(this);
        ToastBox.ShowMsg(citizenName + " died because of " + reason);
        Destroy(gameObject);
    }

    private void Consume(ResourceHolder.ResType resType) {
        var satisfiying = ResourceHolder.Consume(resType);
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


    public enum Schedule {
        WORK,
        SLEEP,
        LEISURE,
        NONE
    }
    private enum SatisfactionLevel {
        FULL = 3,
        OKAY = 2,
        DREADING = 1,
        NONE = 0
    }
}
