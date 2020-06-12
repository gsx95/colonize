using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Clock : MonoBehaviour {

    private static Clock Instance;

    private GameTime gameTime;

    private Dictionary<Timer, Action<string>> activeTimers = new Dictionary<Timer, Action<string>>();

    private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();

    private static float secondsPerIGDay = 60;
    private static float secondsPerIGHour;
    void Awake() {
        Instance = this;
        secondsPerIGHour = secondsPerIGDay / 24;
        gameTime = new GameTime(0);
    }

    void Start() {
        AddTimer((a) => {
            gameTime.NextHalfHour();
        }, 0.5f);
        DebugPanel.AddDebug(() => { return GetCurrentTime().ToString(); }, "Time");
    }

    public static GameTime GetCurrentTime() {
        return Instance.gameTime;
    }

    public static string AddTimerInstantTrigger(Action<string> listener, float inGameHours) {
        string timerId = AddTimer(listener, inGameHours);
        listener(timerId);
        return timerId;
    }

    public static string AddTimer(Action<string> listener, float inGameHours){
        var id = TimerId();
        var timer = new Timer(secondsPerIGHour * inGameHours, id);
        Instance.timers.Add(id, timer);
        Instance.activeTimers.Add(timer, listener);
        return id;
    }

    public static string AddTimerMinutes(Action<string> listener, float inGameMinutes) {
        var id = TimerId();
        var timer = new Timer(secondsPerIGHour * inGameMinutes * 60, id);
        Instance.timers.Add(id, timer);
        Instance.activeTimers.Add(timer, listener);
        return id;
    }

    public static float CurrentTimerState(string timerId) {
        var timer = Instance.timers[timerId];
        return timer.CurrentTime();
    }
    public static void AddOneTimeTimer(Action<string> listener, float inGameHours) {
        var id = TimerId();
        Instance.activeTimers.Add(new Timer(secondsPerIGHour * inGameHours, id, true), listener);
    }

    void Update() {
        List<Timer> toDelete = new List<Timer>();
        var copy = new Dictionary<Timer, Action<string>>(activeTimers);
        foreach (Timer timer in copy.Keys) {
            Action<string> listener = copy[timer];
            if (timer.Update(Time.deltaTime)) {
                listener(timer.id);
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
        public int day = 1;

        public GameTime(int hour) {
            this.hour = hour;
        }

        public void NextHalfHour() {
            minute += 30;
            if(minute == 60)
            {
                minute = 0;
                hour++;
            }
            if(hour == 24) {
                hour = 0;
                day++;
            }
        }

        public override string ToString() {
            return "[" + day.ToString() + "]   " + hour.ToString("00") + ":" + minute.ToString("00");
        }
    }

    class Timer {
        float targetSeconds;
        float currentSeconds;
        bool oneTime;
        public string id;


        public Timer(float seconds, string id) {
            targetSeconds = seconds;
            currentSeconds = seconds;
            oneTime = false;
            this.id = id;
        }
        public Timer(float seconds, string id, bool oneTime) {
            targetSeconds = seconds;
            currentSeconds = seconds;
            this.oneTime = oneTime;
            this.id = id;
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
