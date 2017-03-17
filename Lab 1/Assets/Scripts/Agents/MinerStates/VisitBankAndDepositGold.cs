using UnityEngine;

public sealed class VisitBankAndDepositGold : State<Miner>
{
    static readonly VisitBankAndDepositGold instance = new VisitBankAndDepositGold();

    public static VisitBankAndDepositGold Instance
    {
        get
        {
            return instance;
        }
    }

    static VisitBankAndDepositGold() { }
    private VisitBankAndDepositGold() { }

    public override void Enter(Miner agent)
    {
        agent.Log("I've arrived at the bank!");
    }

    public override void Execute(Miner agent)
    {
        agent.AddToBank(agent.GetGoldCarried());
        agent.SetGoldCarried(0);

        agent.Log("Depositing gold. Total savings now: " + agent.GetMoneyInBank());

        if (agent.WealthyEnough())
        {
            agent.Log("WooHoo! Rich enough for now.");
            agent.FindPath(Tiles.Shack);
            agent.nextState = GoHomeAndSleepTilRested.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
        }
        else
        {
            agent.Log("Time to mine more gold!");
            agent.FindPath(Tiles.GoldMine);
            agent.nextState = EnterMineAndDigForNugget.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
        }
    }

    public override void Exit(Miner agent)
    {
        agent.Log("Leavin' the bank.");
    }
}
