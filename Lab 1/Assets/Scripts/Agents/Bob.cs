using UnityEngine;

public class Bob : Agent
{

    private StateMachine<Bob> stateMachine;

    public static int WAIT_TIME = 5;
    public int waitedTime = 0;
    public int createdTime = 0;

    public void Awake()
    {
        this.stateMachine = new StateMachine<Bob>();
        this.stateMachine.Init(this, WaitState.Instance);
    }

    public void IncreaseWaitedTime(int amount)
    {
        this.waitedTime += amount;
    }

    public bool WaitedLongEnough()
    {
        return this.waitedTime >= WAIT_TIME;
    }

    public void CreateTime()
    {
        this.createdTime++;
        this.waitedTime = 0;
    }

    public void ChangeState(State<Bob> state)
    {
        this.stateMachine.ChangeState(state);
    }

    public override void Update()
    {
        this.stateMachine.Update();
    }

    StateMachine<Bob> GetFSM()
    {
        return this.stateMachine;
    }
}
