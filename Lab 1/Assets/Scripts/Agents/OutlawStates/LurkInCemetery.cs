using UnityEngine;

public sealed class LurkInCemetery : State<Outlaw>
{
    static readonly LurkInCemetery instance = new LurkInCemetery();

    public static LurkInCemetery Instance
    {
        get
        {
            return instance;
        }
    }

    static LurkInCemetery() { }
    private LurkInCemetery() { }

    public override void Enter(Outlaw agent)
    {
        agent.Log("Arrived at the cemetery!");
    }

    public override void Execute(Outlaw agent)
    {
        if (Random.Range(0.0f, 1.0f) < 0.05f)
        {
            agent.Log("Gonna go lurk in my camp!");
            agent.FindPath(Tiles.OutlawCamp);
            agent.nextState = LurkInCamp.Instance;
            agent.ChangeState(Movement<Outlaw>.Instance);
        }
        else
        {
            agent.Log("Just lurking in the cemetery!");
        }
    }

    public override void Exit(Outlaw agent)
    {
        agent.Log("Leaving the cemetery.");
    }
}
