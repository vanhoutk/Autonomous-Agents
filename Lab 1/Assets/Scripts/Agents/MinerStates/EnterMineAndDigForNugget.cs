using UnityEngine;

public sealed class EnterMineAndDigForNugget : State<Miner>
{
    static readonly EnterMineAndDigForNugget instance = new EnterMineAndDigForNugget();

    public static EnterMineAndDigForNugget Instance
    {
        get
        {
            return instance;
        }
    }

    static EnterMineAndDigForNugget() { }
    private EnterMineAndDigForNugget() { }

    public override void Enter(Miner agent)
    {
        Debug.Log("Miner: Walkin' to the gold mine...");
    }

    public override void Execute(Miner agent)
    {
        agent.AddToGoldCarried(1);
        agent.IncreaseFatigue();
        Debug.Log("Miner: Pickin' up a nugget");
        
        if (agent.PocketsFull())
        {
            agent.FindPath(Tiles.Bank);
            agent.nextState = VisitBankAndDepositGold.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
        }
        else if (agent.Thirsty())
        {
            agent.FindPath(Tiles.Saloon);
            agent.nextState = QuenchThirst.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
        }
        else if (agent.Fatigued())
        {
            agent.FindPath(Tiles.Shack);
            agent.nextState = GoHomeAndSleepTilRested.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
        }
    }

    public override void Exit(Miner agent)
    {
        Debug.Log("Miner: Ah'm leavin' the gold mine with mah pockets full o' sweet gold");
    }
}
