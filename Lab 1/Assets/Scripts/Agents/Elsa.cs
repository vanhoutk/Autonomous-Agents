using UnityEngine;

public class Else : MonoBehaviour
{
    private StateMachine<Else> stateMachine;

    public void Awake()
    {
        stateMachine = new StateMachine<Else>();
        //this.stateMachine.Init(this, WaitState.Instance);
    }

    public void ChangeState(State<Else> state)
    {
        stateMachine.ChangeState(state);
    }

    public void Update()
    {
        stateMachine.Update();
    }

    StateMachine<Else> GetFSM()
    {
        return stateMachine;
    }
}
