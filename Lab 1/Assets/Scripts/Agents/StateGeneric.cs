abstract public class StateGeneric <T> {

	abstract public void Enter (AgentGeneric<T> agent);
	abstract public void Execute (AgentGeneric<T> agent);
	abstract public void Exit (AgentGeneric<T> agent);
}