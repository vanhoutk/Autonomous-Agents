using UnityEngine;

public sealed class FightSheriff : State<Outlaw>
{
    static readonly FightSheriff instance = new FightSheriff();

    public static FightSheriff Instance
    {
        get
        {
            return instance;
        }
    }

    static FightSheriff() { }
    private FightSheriff() { }

    public override void Enter(Outlaw agent)
    {
        agent.Log("Time to kill this sheriff!");
    }

    public override void Execute(Outlaw agent)
    {
        if(agent.isAlive)
        {
            GameObject sheriffObject = GameObject.Find(Sheriff.agentName);
            Sheriff sheriff = sheriffObject.GetComponent<Sheriff>();

            if(sheriff.isAlive)
            {
                agent.Log("Dodgin' bullets and shootin' sheriffs!");
                agent.ShootSheriff();
            }
            else
            {
                agent.SearchSheriff(AgentTypes.Sheriff);
            }
        }
        else
        {
            if(agent.stateMachine.GetState() != Dead<Outlaw>.Instance)
                agent.ChangeState(Dead<Outlaw>.Instance);
        }
    }

    public override void Exit(Outlaw agent)
    {
    }
}
