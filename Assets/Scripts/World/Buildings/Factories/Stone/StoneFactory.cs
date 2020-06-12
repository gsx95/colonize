public class StoneFactory : Factory {
    new void Awake() {
        base.Awake();
        maxWorkers = 2;
        productionDurationPerWorkerIGHours = 0.75f;
        outputs.Add(new ResAmount(ResourceHolder.ResType.STONE, 2));
    }

}
