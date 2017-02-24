using UnityEngine;

public class Elsa : MonoBehaviour
{
    private StateMachine<Elsa> stateMachine;

    public void Awake()
    {
        stateMachine = new StateMachine<Elsa>();
        //this.stateMachine.Init(this, WaitState.Instance);
    }

    public void ChangeState(State<Elsa> state)
    {
        stateMachine.ChangeState(state);
    }

    public void Update()
    {
        stateMachine.Update();
    }

    StateMachine<Elsa> GetFSM()
    {
        return stateMachine;
    }
}
