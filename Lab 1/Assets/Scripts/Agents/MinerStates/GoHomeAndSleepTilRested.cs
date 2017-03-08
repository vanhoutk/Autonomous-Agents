using UnityEngine;

public sealed class GoHomeAndSleepTilRested : State<Miner>
{
    static readonly GoHomeAndSleepTilRested instance = new GoHomeAndSleepTilRested();

    public static GoHomeAndSleepTilRested Instance
    {
        get
        {
            return instance;
        }
    }

    static GoHomeAndSleepTilRested() { }
    private GoHomeAndSleepTilRested() { }

    public override void Enter(Miner agent)
    {
        Debug.Log("Miner: Walkin' home");
    }

    public override void Execute(Miner agent)
    {
        if (!agent.Fatigued())
        {
            Debug.Log("Miner: What a God darn fantastic nap! Time to find more gold.");
            agent.FindPath(Tiles.GoldMine);
            agent.nextState = EnterMineAndDigForNugget.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
        }
        else
        {
            agent.DecreaseFatigue();
            Debug.Log("Miner: Zzz...");
        }
    }

    public override void Exit(Miner agent)
    {
        Debug.Log("Miner: Leavin' the house.");
    }
}
