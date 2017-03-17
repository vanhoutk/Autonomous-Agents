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
            agent.Log("Dodgin' bullets and shootin' sheriffs!");
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
