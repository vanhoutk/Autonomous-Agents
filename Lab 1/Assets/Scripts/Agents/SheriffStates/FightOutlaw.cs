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
        agent.Log("Time to kill this outlaw!");
    }

    public override void Execute(Sheriff agent)
    {
        if (agent.isAlive)
        {
            GameObject outlawObject = GameObject.Find(Outlaw.agentName);
            Outlaw outlaw = outlawObject.GetComponent<Outlaw>();

            if (outlaw.isAlive)
            {
                agent.Log("I'm gonna kill this filthy outlaw!");
                agent.ShootOutlaw();
            }
            else
            {
                agent.SearchOutlaw(AgentTypes.Outlaw);
            }
        }
        else
        {
            if (agent.stateMachine.GetState() != Dead<Sheriff>.Instance)
                agent.ChangeState(Dead<Sheriff>.Instance);
        }
    }

    public override void Exit(Sheriff agent)
    {
    }
}
