using UnityEngine;

public class BobGeneric : AgentGeneric<BobGeneric> {

	public void Awake () {
        stateMachine = new StateMachineGeneric<BobGeneric>();
        stateMachine.Init(this, WaitStateGeneric<BobGeneric>.Instance);
	}
}
