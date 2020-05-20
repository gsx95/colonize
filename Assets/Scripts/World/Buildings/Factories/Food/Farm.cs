using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Factory
{
    new void Awake()
    {
        base.Awake();
        maxWorkers = 2;
        productionDurationIGHours = 0.85f;   // so that one person (aka 50% of employees) produce one water every 1.7 hours -> a tad more than consumed by 3 citizens.
        outputs.Add(new ResAmount(ResourceHolder.ResType.FOOD, 1));
    }

    new void Start()
    {
        base.Start();
    }
}
