using UnityEngine;

public sealed class Dead<T> : State<T> where T : Agent<T>
{
    static readonly Dead<T> instance = new Dead<T>();

    public static Dead<T> Instance
    {
        get
        {
            return instance;
        }
    }

    static Dead() { }
    private Dead() { }

    public override void Enter(T agent)
    {
        agent.Log("I'm dead!");
    }

    public override void Execute(T agent)
    {
        if (agent.isAlive)
            agent.Log("Hmm... Something seems to be wrong, I'm dead & alive at the same time...");
        // Do nothing while dead (Body still stays in the same place until collected)
    }

    public override void Exit(T agent)
    {
        agent.Log("My body just got collected!");
    }
}