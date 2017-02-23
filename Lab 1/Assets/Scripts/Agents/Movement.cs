using UnityEngine;

public sealed class Movement : State<Outlaw>
{

    static readonly Movement instance = new Movement();

    public static Movement Instance
    {
        get
        {
            return instance;
        }
    }

    static Movement() { }
    private Movement() { }

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
    }

    public override void Exit(Outlaw agent)
    {
        Debug.Log("Outlaw: Leaving the bank!");
    }
}