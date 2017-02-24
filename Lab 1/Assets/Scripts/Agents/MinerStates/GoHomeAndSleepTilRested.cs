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
        // If the miner is not already located at the gold mine, he must
        // change location to the gold mine
        //if (agent.GetLocation() != Tiles.Shack)
        //{
            Debug.Log("Miner: Walkin' home");
        //    agent.ChangeLocation(Tiles.Shack);
        //}
    }

    public override void Execute(Miner agent)
    {
        if (!agent.Fatigued())
        {
            Debug.Log("Miner: What a God darn fantastic nap! Time to find more gold.");
            agent.FindPath(Tiles.GoldMine);
            agent.nextState = EnterMineAndDigForNugget.Instance;
            agent.ChangeState(MinerMovement.Instance);
            //agent.ChangeState(EnterMineAndDigForNugget.Instance);
        }
        else
        {
            agent.DecreaseFatigue();
            Debug.Log("Miner: Zzz...");
        }
    }

    public override void Exit(Miner agent)
    {
        agent.previousState = Instance;
        agent.previousLocation = agent.location;
        Debug.Log("Leavin' the house.");
    }
}
