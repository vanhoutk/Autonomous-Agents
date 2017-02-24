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
        // If the miner is not already located at the gold mine, he must
        // change location to the gold mine
        if (agent.GetLocation() != Tiles.Bank)
        {
            Debug.Log("Miner: Goin' to the bank. Yes siree...");
            agent.ChangeLocation(Tiles.Bank);
        }
    }

    public override void Execute(Miner agent)
    {
        // Deposit the gold
        agent.AddToBank(agent.GetGoldCarried());
        agent.SetGoldCarried(0);

        Debug.Log("Miner: Depositing gold. Total savings now: " + agent.GetMoneyInBank());
        // If enough gold mined, go and put it in the bank
        if (agent.WealthyEnough())
        {
            Debug.Log("Miner: WooHoo! Rich enough for now.");
            agent.ChangeState(GoHomeAndSleepTilRested.Instance);
        }

        else
        {
            agent.ChangeState(EnterMineAndDigForNugget.Instance);
        }
    }

    public override void Exit(Miner agent)
    {
        agent.previousState = Instance;
        agent.previousLocation = agent.location;
        Debug.Log("Miner: Leavin' the bankt.");
    }
}
