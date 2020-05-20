using System.Collections.Generic;
using UnityEngine;

public class ResourceHolder : MonoBehaviour
{

    private static readonly Dictionary<ResType, float> resources = new Dictionary<ResType, float>();
    private static int foodAlarms = 0;
    private static int waterAlarms = 0;
    private static int alarmTimeHours = 72;
    void Start()
    {
        resources.Add(ResType.FOOD, 30);
        resources.Add(ResType.WATER, 30);
        resources.Add(ResType.STONE, 30);

        DebugPanel.AddDebug(() =>
        {
            return foodAlarms;
        }, "Food Alarms");
        DebugPanel.AddDebug(() =>
        {
            return waterAlarms;
        }, "Water Alarms");

        Clock.AddTimer(() =>
        {
            if (FoodLow())
            {
                foodAlarms++;
                Clock.AddOneTimeTimer(() => { foodAlarms--; }, alarmTimeHours);
            }
        }, 1);

        Clock.AddTimer(() =>
        {
            if (WaterLow())
            {
                waterAlarms++;
                Clock.AddOneTimeTimer(() => { waterAlarms--; }, alarmTimeHours);
            }
        }, 1);
    }

    void Update()
    {

    }

    public static bool FoodAlarmLast3Days()
    {
        return foodAlarms > 0;
    }

    public static bool WaterAlarmLast3Days()
    {
        return waterAlarms > 0;
    }

    private bool FoodLow()
    {
       return resources[ResType.FOOD] < Census.GetCitizensNum() * 5 * 2;
    }

    private bool WaterLow()
    {
       return resources[ResType.WATER] < Census.GetCitizensNum() * 5 * 2;
    }

    public static bool CanAfford(List<ResAmount> costs)
    {
        foreach(ResAmount cost in costs)
        {
            if(resources[cost.GetResType()] < cost.GetAmount())
            {
                return false;
            }
        }
        return true;
    }
    public static bool Consume(ResType resType, float amount = 1)
    {
        var old = resources[resType];
        if(old < amount)
            return false;
        var newNum = old - amount;
        resources[resType] = newNum;
        return true;
    }

    public static void Consume(List<ResAmount> toConsume)
    {
        foreach(ResAmount res in toConsume)
        {
            Consume(res.GetResType(), res.GetAmount());
        }
    }

    public static void Produce(ResType resType, float amount = 1)
    {
        resources[resType] += amount;
    }

    public static void Produce(List<ResAmount> toProduce)
    {
        foreach (ResAmount res in toProduce)
        {
            Produce(res.GetResType(), res.GetAmount());
        }
    }

    public static float GetRes(ResType resType)
    {
        return resources[resType];
    }

    public enum ResType
    {
        FOOD = 0,
        WATER = 1,
        STONE = 2
    }
}
