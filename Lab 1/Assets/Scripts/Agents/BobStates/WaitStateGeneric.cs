using UnityEngine;

public sealed class WaitStateGeneric <T> : StateGeneric<T> {

	static readonly WaitStateGeneric<T> instance = new WaitStateGeneric<T>();

	public static WaitStateGeneric<T> Instance {
		get {
			return instance;
		}
	}

	static WaitStateGeneric() {}
	private WaitStateGeneric() {}

	public override void Enter (AgentGeneric<T> agent) {
		Debug.Log("Starting to wait...");
	}

	public override void Execute (AgentGeneric<T> agent) {
		agent.IncreaseWaitedTime(1);
		Debug.Log("...waiting for " + agent.waitedTime + " cycle" + (agent.waitedTime > 1 ? "s" : "") + " so far...");
		if (agent.WaitedLongEnough()) agent.ChangeState(CreateStateGeneric<T>.Instance);
	}

	public override void Exit (AgentGeneric<T> agent) {
		Debug.Log("...waited long enough!");
	}
}
