using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleFollower : MonoBehaviour
{
    protected Building home = null;
    protected Building work = null;

    protected int workStart = 8;
    protected int leisureStart = 17;
    protected int sleepStart = 22;

    protected Building walkTarget;
    protected bool isWalking = false;
    protected Schedule activeSchedule = Schedule.NONE;
    protected Schedule targetSchedule = Schedule.NONE;
    protected Building currentBuilding;

    protected Citizen me;

    void Awake() {
        // typical schedule:   8.00 - 17.00 work  |  17.00 - 22.00 leisure time | 22.00 - 8.00 home/sleep
        // + 1 / -2 hours
        int offset = Random.Range(-2, 2);
        workStart += offset;
        leisureStart += offset;
        sleepStart += offset;

        if (sleepStart >= 24) {
            sleepStart -= 24;
        }
    }

    void Start()
    {
        
    }

    public void Update()
    {
        if (home == null || work == null) {
            SearchHomeAndWork();
        }
        SetSchedule();
        if (isWalking)
            Walk();
    }

    // Schedule //

    public void ChangeSchedule(int workStart) {
        this.workStart = workStart;
        this.leisureStart = workStart + 9;
        this.sleepStart = workStart - 10;
        if(this.sleepStart < 0) {
            this.sleepStart = 24 + this.sleepStart;
        }
    }

    private void Walk() {
        float step = 1.0f * Time.deltaTime;
        var pos = walkTarget.transform.position;
        pos.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, pos, step);
    }
    private void SetSchedule() {
        var currentTime = Clock.GetCurrentTime();

        if (currentTime.hour >= sleepStart || currentTime.hour < workStart) {
            SleepTime();
            return;
        }

        if (currentTime.hour >= leisureStart || work == null) {
            LeisureTime();
            return;
        }

        if (currentTime.hour >= workStart) {
            WorkTime();
            return;
        }
        throw new System.Exception("I dont know what to do! " + currentTime.hour + "  " + workStart + " " + leisureStart + " " + sleepStart);
    }

    void OnTriggerEnter(Collider other) {
        Building building;
        if (other.gameObject.TryGetComponent(out building)) {
            if (building == walkTarget) {
                building.CheckIn(me);
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
            walkTarget.CheckOut(me);
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
            walkTarget.CheckOut(me);
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
            walkTarget.CheckOut(me);
        }
        walkTarget = work;
        isWalking = true;
        GetComponent<Renderer>().enabled = true;
        targetSchedule = Schedule.WORK;
        activeSchedule = Schedule.NONE;
    }

    private void SearchHomeAndWork() {
        if (home == null) {
            var housing = HousingMarket.GetVacantHouse();
            if (housing != null) {
                housing.AddResident(me);
                home = housing;
            }
        }

        if (home != null && work == null) {
            var employer = LabourOffice.GetVacantEmployer();
            if (employer != null) {
                employer.AddEmployee(me);
                work = employer;
            }
        }
    }

    public enum Schedule {
        WORK,
        SLEEP,
        LEISURE,
        NONE
    }
}
