using System.Collections;
using UnityEngine;

public class StateMachine<T>
{

    private T agent;
    private State<T> previousState;
    private State<T> globalState;
    private State<T> state;

    public void Awake()
    {
        state = null;
        previousState = null;
        globalState = null;
    }

    public void Init(T agent, State<T> startState)
    {
        this.agent = agent;
        state = startState;
    }

    public void Init(T agent, State<T> startState, State<T> globalState)
    {
        this.agent = agent;
        state = startState;
        this.globalState = globalState;
    }

    public void Update()
    {
        if (globalState != null) globalState.Execute(agent);
        if (state != null) state.Execute(agent);
    }

    public void ChangeState(State<T> nextState)
    {
        previousState = state;
        if (state != null) state.Exit(agent);
        state = nextState;
        if (state != null) state.Enter(agent);
    }

    public void RevertToPreviousState()
    {
        if (state != null && previousState != null)
            ChangeState(previousState);
    }

    public State<T> GetState()
    {
        return state;
    }
}