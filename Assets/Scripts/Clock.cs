using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Clock : MonoBehaviour {

    private static Clock Instance;

    private GameTime gameTime;

    private Dictionary<Timer, Action> activeTimers = new Dictionary<Timer, Action>();

    private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();

    private static float secondsPerIGDay = 60;
    private static float secondsPerIGHour;
    void Awake() {
        Instance = this;
        secondsPerIGHour = secondsPerIGDay / 24;
        gameTime = new GameTime(7, 0);
    }

    void Start() {
        AddTimer(() => {
            gameTime.NextMinute();
        }, (1f / 60f));
        DebugPanel.AddDebug(() => { return GetCurrentTime().ToString(); }, "Time");
    }

    public static GameTime GetCurrentTime() {
        return Instance.gameTime;
    }

    public static string AddTimerInstantTrigger(Action listener, float inGameHours) {
        listener();
        return AddTimer(listener, inGameHours);
    }

    public static string AddTimer(Action listener, float inGameHours) {
        var timer = new Timer(secondsPerIGHour * inGameHours);
        var id = TimerId();
        Instance.timers.Add(id, timer);
        Instance.activeTimers.Add(timer, listener);
        return id;
    }

    public static string AddTimerMinutes(Action listener, float inGameMinutes) {
        var timer = new Timer(secondsPerIGHour * inGameMinutes * 60);
        var id = TimerId();
        Instance.timers.Add(id, timer);
        Instance.activeTimers.Add(timer, listener);
        return id;
    }

    public static float CurrentTimerState(string timerId) {
        var timer = Instance.timers[timerId];
        return timer.CurrentTime();
    }
    public static void AddOneTimeTimer(Action listener, float inGameHours) {
        Instance.activeTimers.Add(new Timer(secondsPerIGHour * inGameHours, true), listener);
    }

    void Update() {
        List<Timer> toDelete = new List<Timer>();
        var copy = new Dictionary<Timer, Action>(activeTimers);
        foreach (Timer timer in copy.Keys) {
            Action listener = copy[timer];
            if (timer.Update(Time.deltaTime)) {
                listener();
                if (timer.OneTime())
                    toDelete.Add(timer);
            }
        }
        foreach (Timer timer in toDelete) {
            activeTimers.Remove(timer);
        }
    }

    public class GameTime {
        public int hour = 0;
        public int minute = 0;

        public GameTime(int hour, int minute) {
            this.hour = hour;
            this.minute = minute;
        }

        public void NextMinute() {
            minute++;
            if(minute == 60) {
                hour++;
                minute = 0;
            }
            if(hour == 24) {
                hour = 0;
            }
        }

        public override string ToString() {
            return hour.ToString("00") + ":" + minute.ToString("00");
        }
    }

    class Timer {
        float targetSeconds;
        float currentSeconds;
        bool oneTime;

        public Timer(float seconds) {
            targetSeconds = seconds;
            currentSeconds = seconds;
            oneTime = false;
        }
        public Timer(float seconds, bool oneTime) {
            targetSeconds = seconds;
            currentSeconds = seconds;
            this.oneTime = oneTime;
        }

        public bool OneTime() {
            return oneTime;
        }

        public float CurrentTime() {
            return currentSeconds;
        }

        public bool Update(float timeDelta) {
            currentSeconds -= timeDelta;
            if (currentSeconds <= 0) {
                if (!oneTime)
                    currentSeconds = targetSeconds;
                return true;
            }
            return false;
        }
    }

    private static System.Random random = new System.Random();
    private static string TimerId() {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 50)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
