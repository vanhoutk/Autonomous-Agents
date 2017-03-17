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
        agent.Log("Walkin' to the gold mine...");
    }

    public override void Execute(Miner agent)
    {
        agent.AddToGoldCarried(1);
        agent.IncreaseFatigue();
        agent.Log("Pickin' up a nugget");
        
        if (agent.PocketsFull())
        {
            agent.FindPath(Tiles.Bank);
            agent.nextState = VisitBankAndDepositGold.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
        }
        else if (agent.Thirsty() && agent.GetMoneyInBank() >= 2)
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
        agent.Log("Ah'm leavin' the gold mine with mah pockets full o' sweet gold!");
    }
}
