using UnityEngine;

public class Bob : Agent<Bob>
{

    //private StateMachine<Bob> stateMachine;

    public static int WAIT_TIME = 5;
    public int waitedTime = 0;
    public int createdTime = 0;

    public void Awake()
    {
        stateMachine = new StateMachine<Bob>();
        stateMachine.Init(this, WaitState.Instance);
    }

    public void IncreaseWaitedTime(int amount)
    {
        waitedTime += amount;
    }

    public bool WaitedLongEnough()
    {
        return waitedTime >= WAIT_TIME;
    }

    public void CreateTime()
    {
        createdTime++;
        waitedTime = 0;
    }

    /*public void ChangeState(State<Bob> state)
    {
        stateMachine.ChangeState(state);
    }

    public override void Update()
    {
        stateMachine.Update();
    }*/

    StateMachine<Bob> GetFSM()
    {
        return stateMachine;
    }
}
