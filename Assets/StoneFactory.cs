using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneFactory : Factory
{
    new void Awake()
    {
        base.Awake();
        maxWorkers = 2;
        productionDurationIGHours = 5f;
        outputs.Add(new ResAmount(ResourceHolder.ResType.STONE, 1));
    }

    new void Start()
    {
        base.Start();
    }
}
