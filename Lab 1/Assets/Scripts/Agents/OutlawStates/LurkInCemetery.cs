﻿using UnityEngine;

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
        //if (agent.GetLocation() != Tiles.Cemetery)
        //{
            Debug.Log("Outlaw: Arrived at the cemetery!");
        //    agent.ChangeLocation(Tiles.Cemetery);
        //}
    }

    public override void Execute(Outlaw agent)
    {
        if (Random.Range(0.0f, 1.0f) < 0.05f)
        {
            Debug.Log("Outlaw: Gonna go lurk in my camp!");
            agent.FindPath(Tiles.OutlawCamp);
            agent.nextState = LurkInCamp.Instance;
            agent.ChangeState(MovementState.Instance);
        }
        else
        {
            Debug.Log("Outlaw: Just lurking in the cemetery!");
        }
    }

    public override void Exit(Outlaw agent)
    {
        agent.previousState = Instance;
        agent.previousLocation = Tiles.Cemetery;
        Debug.Log("Outlaw: Leaving the cemetery.");
    }
}
