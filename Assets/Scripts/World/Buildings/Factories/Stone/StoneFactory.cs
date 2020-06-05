﻿public class StoneFactory : Factory {
    new void Awake() {
        base.Awake();
        maxWorkers = 2;
        productionDurationIGHours = 5f;
        outputs.Add(new ResAmount(ResourceHolder.ResType.STONE, 1));
    }

}