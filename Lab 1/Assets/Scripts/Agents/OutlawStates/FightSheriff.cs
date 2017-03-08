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
        Debug.Log("Outlaw: Time to kill this sheriff!");
    }

    public override void Execute(Outlaw agent)
    {
        if(agent.isAlive)
        {
            Debug.Log("Outlaw: Dodgin' bullets and shootin' sheriffs!");
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
