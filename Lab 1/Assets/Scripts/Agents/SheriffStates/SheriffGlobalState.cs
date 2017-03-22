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

            if (agent.nextState != FightOutlaw.Instance && agent.stateMachine.GetState() != FightOutlaw.Instance && outlaw.isAlive && agent.isAlive)
            {
                Vector2 agent_location = (agent.currentLocation - agent.tilingSystem.CurrentPosition) * agent.tilingSystem.tileSize;
                Vector2 target_location = (outlaw.currentLocation - outlaw.tilingSystem.CurrentPosition) * outlaw.tilingSystem.tileSize;
                Vector2 direction = target_location - agent_location;
                RaycastHit2D hit = Physics2D.Raycast(agent_location, direction, 5.0f * agent.tilingSystem.tileSize);

                if (Distance(agent.currentLocation, outlaw.currentLocation) <= 1.0)
                {
                    agent.YellAtOutlaw();

                    if (agent.currentPath != null)
                        agent.ClearCurrentPath();

                    agent.ChangeState(FightOutlaw.Instance);
                }
                else if (hit)
                {
                    if (hit.collider.gameObject.name == Outlaw.agentName)
                    {
                        agent.Log("I see the outlaw!");
                        agent.ChangeState(ChaseOutlaw.Instance);
                    }
                }
            }
        }
    }

    public override void Exit(Sheriff agent)
    {

    }
}
