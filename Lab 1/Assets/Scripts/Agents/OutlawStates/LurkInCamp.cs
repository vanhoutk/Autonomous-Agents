using UnityEngine;

public sealed class LurkInCamp : State<Outlaw>
{
    static readonly LurkInCamp instance = new LurkInCamp();

    public static LurkInCamp Instance
    {
        get
        {
            return instance;
        }
    }

    static LurkInCamp() { }
    private LurkInCamp() { }

    public override void Enter(Outlaw agent)
    {
        agent.Log("Arrived at my camp!");
    }

    public override void Execute(Outlaw agent)
    {
        if (Random.Range(0.0f, 1.0f) < 0.2f)
        {
            agent.Log("Gonna go lurk in the cemetery!");
            agent.FindPath(Tiles.Cemetery);
            agent.nextState = LurkInCemetery.Instance;
            agent.ChangeState(Movement<Outlaw>.Instance);
        }
        else
        {
            agent.Log("Just lurking in my camp!");
        }
    }

    public override void Exit(Outlaw agent)
    {
        agent.Log("Leaving my camp...");
    }
}
