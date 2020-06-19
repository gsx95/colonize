using System;
using UnityEngine;
using UnityEngine.UI;

public class SpaceportInfo : MonoBehaviour
{

    private string timerId;
    public Slider slider;
    public Text arrivals;
    private Func<int> getArrivalsFunc;

    void Update()
    {
        var perc = 1 - Clock.CurrentTimerPercentage(timerId);
        slider.value = perc;
        arrivals.text = "+" + getArrivalsFunc().ToString();
    }

    public void Show(string timerId, Func<int> getArrivalsFunc)
    {
        this.timerId = timerId;
        this.getArrivalsFunc = getArrivalsFunc;
        gameObject.SetActive(true);
    }

}
