using UnityEngine;

public sealed class FightOutlaw : State<Sheriff>
{
    static readonly FightOutlaw instance = new FightOutlaw();

    public static FightOutlaw Instance
    {
        get
        {
            return instance;
        }
    }

    static FightOutlaw() { }
    private FightOutlaw() { }

    public override void Enter(Sheriff agent)
    {
        agent.Log("Time to kill this outlaw!");
    }

    public override void Execute(Sheriff agent)
    {
        agent.ShootOutlaw();
        agent.Log("I just killed the outlaw!");
        agent.SearchOutlaw();
        agent.Log("Best get this money back to the bank!");
        agent.FindPath(Tiles.Bank);
        agent.nextState = ReturnMoneyToBank.Instance;
        agent.ChangeState(Movement<Sheriff>.Instance);
    }

    public override void Exit(Sheriff agent)
    {
    }
}
