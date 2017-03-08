using UnityEngine;

public sealed class WaitInSheriffOffice : State<Sheriff>
{
    static readonly WaitInSheriffOffice instance = new WaitInSheriffOffice();

    public static WaitInSheriffOffice Instance
    {
        get
        {
            return instance;
        }
    }

    static WaitInSheriffOffice() { }
    private WaitInSheriffOffice() { }

    public override void Enter(Sheriff agent)
    {
        Debug.Log("Sheriff: Arrived at my office!");
    }

    public override void Execute(Sheriff agent)
    {
        // Do nothing until a new outlaw spawns
        Debug.Log("Sheriff: Just waiting in my office!");
    }

    public override void Exit(Sheriff agent)
    {
        Debug.Log("Sheriff: Leaving my office.");
    }
}
