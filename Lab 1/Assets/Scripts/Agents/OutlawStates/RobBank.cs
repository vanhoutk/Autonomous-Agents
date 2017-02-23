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
        if (agent.GetLocation() != Tiles.Bank)
        {
            Debug.Log("Outlaw: Going to rob the bank!");
            agent.ChangeLocation(Tiles.Bank);
        }
    }

    public override void Execute(Outlaw agent)
    {
        agent.RobBank();
        //Debug.Log("Outlaw: Just robbed the bank! Now I have " + agent.GetGoldCarried() + " gold!");
        agent.GetFSM().RevertToPreviousState();
    }

    public override void Exit(Outlaw agent)
    {
        Debug.Log("Outlaw: Leaving the bank!");
    }
}
