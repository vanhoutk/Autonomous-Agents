using UnityEngine;

public class Bob : Agent {

	private StateMachine<Bob> stateMachine;

	public void Awake () {
		this.stateMachine = new StateMachine<Bob>();
		this.stateMachine.Init(this, WaitState.Instance);
	}

	public void ChangeState (State<Bob> state) {
		this.stateMachine.ChangeState(state);
	}

	public void Update () {
		this.stateMachine.Update();
	}
}
