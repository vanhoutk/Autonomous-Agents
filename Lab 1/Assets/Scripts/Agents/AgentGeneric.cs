using UnityEngine;
using System.Collections;

abstract public class AgentGeneric <T> : MonoBehaviour {

    public StateMachineGeneric<T> stateMachine;

    public int waitedTime = 0;
    public int createdTime = 0;
    public static int WAIT_TIME = 5;

    public void Update()
    {
        stateMachine.Update();
    }

    public void CreateTime()
    {
        createdTime++;
        waitedTime = 0;
    }

    public void IncreaseWaitedTime(int amount)
    {
        waitedTime += amount;
    }

    public bool WaitedLongEnough()
    {
        return waitedTime >= WAIT_TIME;
    }

    public void ChangeState(StateGeneric<T> state)
    {
        stateMachine.ChangeState(state);
    }
}