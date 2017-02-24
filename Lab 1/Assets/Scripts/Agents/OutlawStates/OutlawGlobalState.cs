using UnityEngine;

public sealed class OutlawGlobalState : State<Outlaw>
{

    static readonly OutlawGlobalState instance = new OutlawGlobalState();

    public static OutlawGlobalState Instance
    {
        get
        {
            return instance;
        }
    }

    static OutlawGlobalState() { }
    private OutlawGlobalState() { }

    public override void Enter(Outlaw agent)
    {

    }

    public override void Execute(Outlaw agent)
    {
        if (Random.Range(0.0f, 1.0f) < 0.005f)
        {
            agent.previousLocation = agent.destination;
            agent.previousState = agent.nextState;
            agent.FindPath(Tiles.Bank);
            agent.nextState = RobBank.Instance;
            agent.ChangeState(OutlawMovement.Instance);
            //agent.ChangeState(RobBank.Instance);
        }
    }

    public override void Exit(Outlaw agent)
    {

    }
}
