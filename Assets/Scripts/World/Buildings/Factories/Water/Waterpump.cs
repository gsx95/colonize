using UnityEngine;

public class Waterpump : Factory {
    new void Awake() {
        base.Awake();
        maxWorkers = 2;
        productionDurationIGHours = 0.4f;
        outputs.Add(new ResAmount(ResourceHolder.ResType.WATER, 1));
    }


}
