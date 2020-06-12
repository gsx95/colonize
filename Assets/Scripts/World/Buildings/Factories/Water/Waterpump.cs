using UnityEngine;

public class Waterpump : Factory {
    new void Awake() {
        base.Awake();
        maxWorkers = 2;
        productionDurationPerWorkerIGHours = 2f;
        outputs.Add(new ResAmount(ResourceHolder.ResType.WATER, 1.5f));
    }
}
