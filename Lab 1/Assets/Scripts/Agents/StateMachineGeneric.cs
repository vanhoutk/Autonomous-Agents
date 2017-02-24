public class StateMachineGeneric <T> {
	
	private AgentGeneric<T> agent;
	private StateGeneric<T> state;

	public void Awake () {
        state = null;
	}

	public void Init (AgentGeneric<T> agent, StateGeneric<T> startState) {
		this.agent = agent;
        state = startState;
	}

	public void Update () {
		if (state != null) state.Execute(agent);
	}
	
	public void ChangeState (StateGeneric<T> nextState) {
		if (state != null) state.Exit(agent);
        state = nextState;
		if (state != null) state.Enter(agent);
	}
}