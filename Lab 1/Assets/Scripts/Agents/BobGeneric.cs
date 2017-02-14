using UnityEngine;

public class BobGeneric : AgentGeneric<BobGeneric> {

	public void Awake () {
		this.stateMachine = new StateMachineGeneric<BobGeneric>();
		this.stateMachine.Init(this, WaitStateGeneric<BobGeneric>.Instance);
	}
}
