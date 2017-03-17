using UnityEngine;

public sealed class BuryBody : State<Undertaker>
{

    static readonly BuryBody instance = new BuryBody();

    public static BuryBody Instance
    {
        get
        {
            return instance;
        }
    }

    static BuryBody() { }
    private BuryBody() { }

    public override void Enter(Undertaker agent)
    {
        agent.Log("I've arrived at the cemetery.");
    }

    public override void Execute(Undertaker agent)
    {
        if (Random.Range(0.0f, 1.0f) < 0.2f) // Take a random amount of "time" to bury the body
        {
            agent.BuryBody();
            agent.Log("All done! Back to the office for me.");
            agent.FindPath(Tiles.Undertakers);
            agent.nextState = WaitInUndertakers.Instance;
            agent.ChangeState(Movement<Undertaker>.Instance);
        }
        else
        {
            agent.Log("Diggin' this grave!");
        }
    }

    public override void Exit(Undertaker agent)
    {
        agent.Log("Leaving the cemetery.");
    }
}
