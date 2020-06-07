public class Farm : Factory {
    new void Awake() {
        base.Awake();
        maxWorkers = 2;
        productionDurationIGHours = 0.3f;
        outputs.Add(new ResAmount(ResourceHolder.ResType.FOOD, 2));
    }

}
