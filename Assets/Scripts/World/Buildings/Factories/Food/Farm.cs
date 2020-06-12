public class Farm : Factory {

    new void Awake() {
        base.Awake();
        maxWorkers = 2;
        productionDurationPerWorkerIGHours = 2f;
        outputs.Add(new ResAmount(ResourceHolder.ResType.FOOD, 1.5f));
    }
}
