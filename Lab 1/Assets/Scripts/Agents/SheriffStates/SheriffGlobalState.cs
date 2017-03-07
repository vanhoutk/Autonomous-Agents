using System;
using UnityEngine;

public sealed class SheriffGlobalState : State<Sheriff>
{
    static public double Distance(Vector2 a, Vector2 b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    static readonly SheriffGlobalState instance = new SheriffGlobalState();

    public static SheriffGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static SheriffGlobalState() { }
    private SheriffGlobalState() { }

    public override void Enter(Sheriff agent)
    {

    }

    public override void Execute(Sheriff agent)
    {
        if (agent.nextState != FightOutlaw.Instance && agent.stateMachine.GetState() != FightOutlaw.Instance)
        {
            GameObject outlawObject = GameObject.Find("Jesse");
            Outlaw outlaw = outlawObject.GetComponent<Outlaw>();
            if (Distance(agent.currentLocation, outlaw.currentLocation) <= 1.0)
            {
                if (agent.currentPath != null)
                    agent.ClearCurrentPath();
                //agent.previousLocation = agent.destination;
                //agent.previousState = agent.nextState;
                agent.ChangeState(FightOutlaw.Instance);
            }
        }
    }

    public override void Exit(Sheriff agent)
    {

    }
}
