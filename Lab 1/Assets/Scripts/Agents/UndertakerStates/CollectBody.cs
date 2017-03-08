using UnityEngine;

public sealed class CollectBody : State<Undertaker>
{
    static readonly CollectBody instance = new CollectBody();

    public static CollectBody Instance
    {
        get
        {
            return instance;
        }
    }

    static CollectBody() { }
    private CollectBody() { }

    public override void Enter(Undertaker agent)
    {
        Debug.Log("Undertaker: Ah! I've found the dead body.");
    }

    public override void Execute(Undertaker agent)
    {
        if (Random.Range(0.0f, 1.0f) < 0.2f) // Take a random amount of "time" to deal with the body
        {
            agent.CollectABody();
            Debug.Log("Undertaker: Body's all wrapped up!");
            agent.FindPath(Tiles.Cemetery);
            agent.nextState = BuryBody.Instance;
            agent.ChangeState(Movement<Undertaker>.Instance);
        }
        else
        {
            Debug.Log("Undertaker: Got to take care of this body!");
        }
    }

    public override void Exit(Undertaker agent)
    {
        Debug.Log("Undertaker: Let's take this body to the cemetery!");
    }
}
