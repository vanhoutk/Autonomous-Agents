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
        //if (agent.GetLocation() != Tiles.OutlawCamp)
        //{
            Debug.Log("Outlaw: Arrived at my camp!");
            //agent.ChangeLocation(Tiles.OutlawCamp);

        //    agent.FindPath(Tiles.OutlawCamp);
        //    agent.MoveAlongPath();

            //Debug.Log("Outlaw: Found the path to my camp!");
            //while (agent.GetLocation() != Tiles.OutlawCamp)
            //{
            //  agent.MoveAlongPath();
            //}
        //}
    }

    public override void Execute(Outlaw agent)
    {
        if (Random.Range(0.0f, 1.0f) < 0.2f)
        {
            Debug.Log("Outlaw: Gonna go lurk in the cemetery!");
            agent.FindPath(Tiles.Cemetery);
            agent.nextState = LurkInCemetery.Instance;
            agent.ChangeState(OutlawMovement.Instance);
            //agent.ChangeState(LurkInCemetery.Instance);
        }
        else
        {
            Debug.Log("Outlaw: Just lurking in my camp!");
        }
    }

    public override void Exit(Outlaw agent)
    {
        //agent.previousState = Instance;
        //agent.previousLocation = agent.location;
        Debug.Log("Leaving my camp...");
    }
}
