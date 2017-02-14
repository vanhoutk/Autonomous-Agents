using UnityEngine;

public class Else : MonoBehaviour
{
    private StateMachine<Else> stateMachine;

    public void Awake()
    {
        this.stateMachine = new StateMachine<Else>();
        //this.stateMachine.Init(this, WaitState.Instance);
    }

    public void ChangeState(State<Else> state)
    {
        this.stateMachine.ChangeState(state);
    }

    public void Update()
    {
        this.stateMachine.Update();
    }

    StateMachine<Else> GetFSM()
    {
        return this.stateMachine;
    }
}
