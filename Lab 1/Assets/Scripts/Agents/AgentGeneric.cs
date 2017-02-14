using UnityEngine;
using System.Collections;

abstract public class AgentGeneric <T> : MonoBehaviour {

    public StateMachineGeneric<T> stateMachine;

    public int waitedTime = 0;
    public int createdTime = 0;
    public static int WAIT_TIME = 5;

    public void Update()
    {
        this.stateMachine.Update();
    }

    public void CreateTime()
    {
        this.createdTime++;
        this.waitedTime = 0;
    }

    public void IncreaseWaitedTime(int amount)
    {
        this.waitedTime += amount;
    }

    public bool WaitedLongEnough()
    {
        return this.waitedTime >= WAIT_TIME;
    }

    public void ChangeState(StateGeneric<T> state)
    {
        this.stateMachine.ChangeState(state);
    }
}