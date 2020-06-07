using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    private Light sunlight;
    private float max;
    private float min = 0.4f;

    void Awake() {
        sunlight = gameObject.GetComponent<Light>();
        max = sunlight.intensity;
    }

    void Update()
    {
        float percentage = TimePercentage();
        float val = Mathf.Lerp(min, max, percentage);
        sunlight.intensity = val;
    }

    private float TimePercentage() {
        var currentTime = Clock.GetCurrentTime();
        float mid = 12 * 60;
        bool isMorning = currentTime.hour < 12;
        float currentMins = (currentTime.hour * 60) + currentTime.minute;
        currentMins = isMorning ? currentMins : currentMins - mid;
        float percentage = currentMins / mid;
        return isMorning ? percentage : 1 - percentage;
    }
}
