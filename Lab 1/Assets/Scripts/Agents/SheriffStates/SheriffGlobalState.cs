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
        GameObject outlawObject = GameObject.Find(Outlaw.agentName);
        if(outlawObject != null)
        {
            Outlaw outlaw = outlawObject.GetComponent<Outlaw>();

            if (agent.nextState != FightOutlaw.Instance && agent.stateMachine.GetState() != FightOutlaw.Instance && outlaw.isAlive)
            {
                if (Distance(agent.currentLocation, outlaw.currentLocation) <= 1.0)
                {
                    agent.YellAtOutlaw();

                    if (agent.currentPath != null)
                        agent.ClearCurrentPath();

                    agent.ChangeState(FightOutlaw.Instance);
                }
            }
        }
    }

    public override void Exit(Sheriff agent)
    {

    }
}
