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
        Debug.Log("Sheriff: Time to kill this outlaw!");
    }

    public override void Execute(Sheriff agent)
    {
        agent.ShootOutlaw();
        Debug.Log("Sheriff: I just killed the outlaw!");
        // Could add extra states to take outlaw's money, return it to the bank, then go to the saloon
        agent.FindPath(Tiles.SheriffsOffice);
        agent.nextState = WaitInSheriffOffice.Instance;
        agent.ChangeState(Movement<Sheriff>.Instance);
    }

    public override void Exit(Sheriff agent)
    {
        Debug.Log("Sheriff: Leaving the bank!");
    }
}
