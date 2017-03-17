using UnityEngine;

public sealed class ReturnMoneyToBank : State<Sheriff>
{
    static readonly ReturnMoneyToBank instance = new ReturnMoneyToBank();

    public static ReturnMoneyToBank Instance
    {
        get
        {
            return instance;
        }
    }

    static ReturnMoneyToBank() { }
    private ReturnMoneyToBank() { }

    public override void Enter(Sheriff agent)
    {
        agent.Log("Just arrived at the bank!");
    }

    public override void Execute(Sheriff agent)
    {
        agent.ReturnGold();
        agent.Log("Returned the gold to the bank! Time to head to the saloon to celebrate a good day's work!");
        agent.FindPath(Tiles.Saloon);
        agent.nextState = RelaxInSaloon.Instance;
        agent.ChangeState(Movement<Sheriff>.Instance);
    }

    public override void Exit(Sheriff agent)
    {
        agent.Log("Leaving the bank!");
    }
}
