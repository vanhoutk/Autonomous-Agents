public class StateMachineGeneric <T> {
	
	private AgentGeneric<T> agent;
	private StateGeneric<T> state;

	public void Awake () {
		this.state = null;
	}

	public void Init (AgentGeneric<T> agent, StateGeneric<T> startState) {
		this.agent = agent;
		this.state = startState;
	}

	public void Update () {
		if (this.state != null) this.state.Execute(this.agent);
	}
	
	public void ChangeState (StateGeneric<T> nextState) {
		if (this.state != null) this.state.Exit(this.agent);
		this.state = nextState;
		if (this.state != null) this.state.Enter(this.agent);
	}
}