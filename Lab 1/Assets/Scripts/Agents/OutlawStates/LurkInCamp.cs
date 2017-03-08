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
        Debug.Log("Outlaw: Arrived at my camp!");
    }

    public override void Execute(Outlaw agent)
    {
        if (Random.Range(0.0f, 1.0f) < 0.2f)
        {
            Debug.Log("Outlaw: Gonna go lurk in the cemetery!");
            agent.FindPath(Tiles.Cemetery);
            agent.nextState = LurkInCemetery.Instance;
            agent.ChangeState(Movement<Outlaw>.Instance);
        }
        else
        {
            Debug.Log("Outlaw: Just lurking in my camp!");
        }
    }

    public override void Exit(Outlaw agent)
    {
        Debug.Log("Outlaw: Leaving my camp...");
    }
}
