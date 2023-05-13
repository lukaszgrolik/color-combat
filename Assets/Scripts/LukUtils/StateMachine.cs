namespace SM
{
	public sealed class StateMachine
	{
	    private State state;
	    public State State => state;

	    public void SetState(State nextState)
	    {
	        if (state != null) state.Exit();

	        state = nextState;
	        state.SetStateMachine(this);
	        nextState.Enter();
	    }

	    public void Exit()
	    {
	        if (state != null) state.Exit();

	        state = null;
	    }
	}

	public abstract class State
	{
	    protected StateMachine stateMachine;

	    public void SetStateMachine(StateMachine stateMachine)
	    {
	        this.stateMachine = stateMachine;
	    }

	    public virtual void Enter() { }
	    public virtual void Exit() { }
	}
}