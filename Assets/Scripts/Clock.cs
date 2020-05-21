using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;

public class Clock : MonoBehaviour
{

    private static Clock Instance;
    
    private Dictionary<Timer, Action> activeTimers = new Dictionary<Timer, Action>();

    private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();

    private static float secondsPerIGDay = 60;
    private static float secondsPerIGHour;
    private void Awake()
    {
        Instance = this;
        secondsPerIGHour = secondsPerIGDay / 24;
    }

    public static string AddTimer(Action listener, float inGameHours)
    {
        var timer = new Timer(secondsPerIGHour * inGameHours);
        var id = TimerId();
        Instance.timers.Add(id, timer);
        Instance.activeTimers.Add(timer, listener);
        return id;
    }

    public static string AddTimerInstantTrigger(Action listener, float inGameHours)
    {
        listener();
        return AddTimer(listener, inGameHours);
    }

    public static float CurrentTime(string timerId)
    {
        var timer = Instance.timers[timerId];
        return timer.CurrentTime();
    }

    public static void AddOneTimeTimer(Action listener, float inGameHours)
    {
        Instance.activeTimers.Add(new Timer(secondsPerIGHour * inGameHours, true), listener);
    }


    // Update is called once per frame
    void Update()
    {
        List<Timer> toDelete = new List<Timer>();
        var copy = new Dictionary<Timer, Action>(activeTimers);
        foreach (Timer timer in copy.Keys)
        {
            Action listener = copy[timer];
            if(timer.Update(Time.deltaTime))
            {
                listener();
                if (timer.OneTime())
                    toDelete.Add(timer);
            }
        }
        foreach(Timer timer in toDelete)
        {
            activeTimers.Remove(timer);
        }
    }

    class Timer
    {
        float targetSeconds;
        float currentSeconds;
        bool oneTime;

        public Timer(float seconds)
        {
            targetSeconds = seconds;
            currentSeconds = seconds;
            oneTime = false;
        }
        public Timer(float seconds, bool oneTime)
        {
            targetSeconds = seconds;
            currentSeconds = seconds;
            this.oneTime = oneTime;
        }

        public bool OneTime()
        {
            return oneTime;
        }

        public float CurrentTime()
        {
            return currentSeconds;
        }

        public bool Update(float timeDelta)
        {
            currentSeconds -= timeDelta;
            if(currentSeconds <= 0)
            {
                if (!oneTime)
                    currentSeconds = targetSeconds;
                return true;
            }
            return false;
        }
    }

    private static System.Random random = new System.Random();
    private static string TimerId()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 50)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
