public class Farm : Factory {
    new void Awake() {
        base.Awake();
        maxWorkers = 2;
        productionDurationIGHours = 0.4f;
        outputs.Add(new ResAmount(ResourceHolder.ResType.FOOD, 1));
    }

    new void Start() {
        base.Start();
    }
}
