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
        agent.Log("Just got home.");
    }

    public override void Execute(Miner agent)
    {
        if (!agent.Fatigued())
        {
            agent.Log("What a God darn fantastic nap! Time to find more gold.");
            agent.FindPath(Tiles.GoldMine);
            agent.nextState = EnterMineAndDigForNugget.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
        }
        else
        {
            agent.DecreaseFatigue();
            agent.Log("Zzz...");
        }
    }

    public override void Exit(Miner agent)
    {
        agent.Log("Leavin' the house.");
    }
}
