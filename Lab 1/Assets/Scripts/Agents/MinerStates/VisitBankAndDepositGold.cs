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
        Debug.Log("Miner: I've arrived at the bank!");
    }

    public override void Execute(Miner agent)
    {
        agent.AddToBank(agent.GetGoldCarried());
        agent.SetGoldCarried(0);

        Debug.Log("Miner: Depositing gold. Total savings now: " + agent.GetMoneyInBank());

        if (agent.WealthyEnough())
        {
            Debug.Log("Miner: WooHoo! Rich enough for now.");
            agent.FindPath(Tiles.Shack);
            agent.nextState = GoHomeAndSleepTilRested.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
        }
        else
        {
            Debug.Log("Miner: Time to mine more gold!");
            agent.FindPath(Tiles.GoldMine);
            agent.nextState = EnterMineAndDigForNugget.Instance;
            agent.ChangeState(Movement<Miner>.Instance);
        }
    }

    public override void Exit(Miner agent)
    {
        Debug.Log("Miner: Leavin' the bank.");
    }
}
