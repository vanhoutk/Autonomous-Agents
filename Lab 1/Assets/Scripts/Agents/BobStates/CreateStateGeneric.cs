using UnityEngine;

public sealed class CreateStateGeneric <T>: StateGeneric<T> {
	
	static readonly CreateStateGeneric<T> instance = new CreateStateGeneric<T>();
	
	public static CreateStateGeneric<T> Instance {
		get {
			return instance;
		}
	}
	
	static CreateStateGeneric() {}
	private CreateStateGeneric() {}
	
	public override void Enter (AgentGeneric<T> agent) {
		Debug.Log("Gathering creative energies...");
	}
	
	public override void Execute (AgentGeneric<T> agent) {
		agent.CreateTime();
		Debug.Log("...creating more time, for a total of " + agent.createdTime + " unit" + (agent.createdTime > 1 ? "s" : "") + "...");
		agent.ChangeState(WaitStateGeneric<T>.Instance);
	}
	
	public override void Exit (AgentGeneric<T> agent) {
		Debug.Log("...creativity spent!");
	}
}
