using UnityEngine;

public sealed class RelaxInSaloon : State<Sheriff>
{
    static readonly RelaxInSaloon instance = new RelaxInSaloon();

    public static RelaxInSaloon Instance
    {
        get
        {
            return instance;
        }
    }

    static RelaxInSaloon() { }
    private RelaxInSaloon() { }

    public override void Enter(Sheriff agent)
    {
        agent.Log("Just arrived at the saloon!");
    }

    public override void Execute(Sheriff agent)
    {
        if (Random.Range(0.0f, 1.0f) < 0.1f)
        {
            agent.Log("Time to go back to my office!");
            agent.FindPath(Tiles.SheriffsOffice);
            agent.nextState = WaitInSheriffOffice.Instance;
            agent.ChangeState(Movement<Sheriff>.Instance);
        }
        else
        {
            agent.Log("It's great to relax once in a while!");
        }
    }

    public override void Exit(Sheriff agent)
    {
        agent.Log("Leaving the saloon!");
    }
}
