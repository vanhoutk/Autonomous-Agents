using UnityEngine;

public sealed class WaitInUndertakers : State<Undertaker>
{

    static readonly WaitInUndertakers instance = new WaitInUndertakers();

    public static WaitInUndertakers Instance
    {
        get
        {
            return instance;
        }
    }

    static WaitInUndertakers() { }
    private WaitInUndertakers() { }

    public override void Enter(Undertaker agent)
    {
        Debug.Log("Undertaker: Back in my office!");
    }

    public override void Execute(Undertaker agent)
    {
        Debug.Log("Undertaker: Just waiting in my office!");
    }

    public override void Exit(Undertaker agent)
    {
        Debug.Log("Undertaker: Off to collect a body.");
    }
}
