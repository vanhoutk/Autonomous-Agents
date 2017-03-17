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
        agent.Log("Back in my office!");
    }

    public override void Execute(Undertaker agent)
    {
        // Wait in office until a "death" causes a state change
        agent.Log("Just waiting in my office!");
    }

    public override void Exit(Undertaker agent)
    {
        agent.Log("Off to collect a body.");
    }
}
