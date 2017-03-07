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
        Debug.Log("Undertaker: I've arrived at the cemetery.");
    }

    public override void Execute(Undertaker agent)
    {
        if (Random.Range(0.0f, 1.0f) < 0.2f)
        {
            Debug.Log("Undertaker: All done! Back to the office for me.");
            agent.FindPath(Tiles.Undertakers);
            agent.nextState = WaitInUndertakers.Instance;
            agent.ChangeState(Movement<Undertaker>.Instance);
        }
        else
        {
            Debug.Log("Undertaker: Diggin' this grave!");
        }
    }

    public override void Exit(Undertaker agent)
    {
        Debug.Log("Undertaker: Leaving the cemetery.");
    }
}
