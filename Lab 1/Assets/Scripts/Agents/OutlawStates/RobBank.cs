using UnityEngine;

public sealed class RobBank : State<Outlaw>
{
    static readonly RobBank instance = new RobBank();

    public static RobBank Instance
    {
        get
        {
            return instance;
        }
    }

    static RobBank() { }
    private RobBank() { }

    public override void Enter(Outlaw agent)
    {
        Debug.Log("Outlaw: Going to rob the bank!");
    }

    public override void Execute(Outlaw agent)
    {
        agent.RobBank();
        Debug.Log("Outlaw: Just robbed the bank! Now I have " + agent.GetGoldCarried() + " gold!");
        agent.FindPath(agent.previousLocation);
        agent.nextState = agent.previousState;
        agent.GetFSM().RevertToPreviousState();
    }

    public override void Exit(Outlaw agent)
    {
        Debug.Log("Outlaw: Leaving the bank!");
    }
}
