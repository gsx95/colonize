﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScheduleFollower : MonoBehaviour
{

    protected Building home = null;

    private Building currentWork = null;

    protected Building firstWork = null;
    protected Building secondWork = null;

    protected int workStart = 8;
    protected int leisureStart = 17;
    protected int sleepStart = 22;

    protected int secondWorkStartHour;
    protected int secondWorkStartMin = 30;

    protected Building walkTarget;
    protected bool isWalking = false;
    protected Schedule activeSchedule = Schedule.NONE;
    protected Schedule targetSchedule = Schedule.NONE;
    protected Building currentBuilding;

    protected Citizen me;

    public NavMeshAgent agent;

    private bool inFirstWork = false;

    public void Awake() {
        // typical schedule:   8.00 - 17.00 work  |  17.00 - 22.00 leisure time | 22.00 - 8.00 home/sleep
        // + 1 / -2 hours
        int offset = Random.Range(-2, 2);
        workStart += offset;
        leisureStart += offset;
        sleepStart += offset;

        if (sleepStart >= 24) {
            sleepStart -= 24;
        }
        int workHours = leisureStart - workStart;
        int firstWorkEnd = Mathf.FloorToInt(workHours / 2f);
        secondWorkStartHour = workStart + firstWorkEnd;
        Debug.Log(secondWorkStartHour);
        Debug.Log(workStart);
        Debug.Log(firstWorkEnd);
        Debug.Log(leisureStart);
        Debug.Log(workStart);
    }

    public void Update()
    {
        if (home == null || firstWork == null) {
            SearchHomeAndWork();
        }
        SetSchedule();
        if (isWalking)
            Walk();
    }

    public Building Home()
    {
        return home;
    }

    public void SetHome(Housing house)
    {
        home.GetComponent<Housing>().RemoveResident(me);
        home = house;
        walkTarget = null;
        targetSchedule = Schedule.NONE;
        activeSchedule = Schedule.NONE;
    }

    public Building Workplace()
    {
        return firstWork;
    }

    public Building SecondWorkplace()
    {
        return secondWork;
    }

    public void SetWork(Factory factory)
    {
        if (firstWork != null)
            firstWork.GetComponent<Factory>().RemoveEmployee(me);
        factory.GetComponent<Factory>().AddEmployee(me);
        firstWork = factory;
        walkTarget = null;
        targetSchedule = Schedule.NONE;
        activeSchedule = Schedule.NONE;
    }

    public void SetSecondWork(Factory factory)
    {
        if(secondWork != null)
            secondWork.GetComponent<Factory>().RemoveEmployee(me);
        factory.GetComponent<Factory>().AddEmployee(me);
        secondWork = factory;
        walkTarget = null;
        targetSchedule = Schedule.NONE;
        activeSchedule = Schedule.NONE;
    }

    public void RemoveSecondWork()
    {
        if(secondWork != null)
            secondWork.GetComponent<Factory>().RemoveEmployee(me);
        secondWork = null;
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
        var pos = walkTarget.entrance.transform.position;
        pos.y = transform.position.y;
        if (agent.destination != pos)
        {
            agent.SetDestination(pos);
            agent.isStopped = false;
        }
    }

    private void StopWalking()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    private void SetSchedule() {
        var currentTime = Clock.GetCurrentTime();

        if (currentTime.hour >= sleepStart || currentTime.hour < workStart) {
            SleepTime();
            return;
        }

        if (currentTime.hour >= leisureStart || firstWork == null) {
            LeisureTime();
            return;
        }
        Debug.Log(currentTime.hour + "  >=  " + workStart);
        if (currentTime.hour >= workStart) {
            currentWork = firstWork;
            if(secondWork != null && ((currentTime.hour > secondWorkStartHour) ||(currentTime.hour == secondWorkStartHour && currentTime.minute >= secondWorkStartMin)))
            {
                if(inFirstWork)
                {
                    Debug.Log("start 2nd work  " + secondWorkStartHour + "  " + currentTime.hour);
                    activeSchedule = Schedule.NONE;
                    targetSchedule = Schedule.NONE;
                }
                Debug.Log("do 2nd work");
                inFirstWork = false;
                currentWork = secondWork;
            } else
            {
                inFirstWork = true;
            }
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
                StopWalking();
                activeSchedule = targetSchedule;
            }
        }
    }

    private void SleepTime() {
        if (targetSchedule == Schedule.SLEEP)
            return;
        bool isInWrongBuilding = (isWalking == false && walkTarget != null && activeSchedule != Schedule.SLEEP);
        if (isInWrongBuilding) {
            transform.position = walkTarget.exit.transform.position;
            walkTarget.CheckOut(me);
        }
        if(isInWrongBuilding || activeSchedule == Schedule.NONE) {
            walkTarget = home;
            isWalking = true;
            GetComponent<Renderer>().enabled = true;
            targetSchedule = Schedule.SLEEP;
            activeSchedule = Schedule.NONE;
        }
    }

    private void LeisureTime()
    {
        if (targetSchedule == Schedule.LEISURE)
            return;
        bool isInWrongBuilding = (isWalking == false && walkTarget != null && activeSchedule != Schedule.LEISURE);
        if (isInWrongBuilding)
        {
            transform.position = walkTarget.exit.transform.position;
            walkTarget.CheckOut(me);
        }
        if (isInWrongBuilding || activeSchedule == Schedule.NONE)
        {
            targetSchedule = Schedule.LEISURE;
            activeSchedule = Schedule.NONE;

            var nextTarget = home;
            if (walkTarget == nextTarget)
            {
                activeSchedule = targetSchedule;
            }
            else
            {
                walkTarget = home;
                isWalking = true;
                GetComponent<Renderer>().enabled = true;
            }
        }
    }

    private void WorkTime()
    {
        if (targetSchedule == Schedule.WORK)
            return;
        bool isInWrongBuilding = (isWalking == false && walkTarget != null && activeSchedule != Schedule.WORK);
        if (isInWrongBuilding)
        {
            transform.position = walkTarget.exit.transform.position;
            walkTarget.CheckOut(me);
        }
        if (isInWrongBuilding || activeSchedule == Schedule.NONE)
        {
            walkTarget = currentWork;
            isWalking = true;
            GetComponent<Renderer>().enabled = true;
            targetSchedule = Schedule.WORK;
            activeSchedule = Schedule.NONE;
        }

    }

    private void SearchHomeAndWork() {
        if (home == null) {
            var housing = HousingMarket.GetVacantHouse();
            if (housing != null) {
                housing.AddResident(me);
                home = housing;
            }
        }

        if (home != null && firstWork == null) {
            var employer = LabourOffice.GetVacantEmployer();
            if (employer != null) {
                employer.AddEmployee(me);
                firstWork = employer;
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
