using UnityEngine;

[System.Serializable]
public class FSMState
{
    public FSMState previousState;

    public virtual void Enter() { }

    public virtual void Execute() { }

    public virtual void FixedExecute() { }

    public virtual void Exit() { }
}

public class FSM
{
    FSMState currentState = null;

    public bool active = true;

    public void Start(FSMState state)
    {
        currentState = state;
        currentState.Enter();
    }

	public void Update ()
    {
		if (currentState != null && active) { currentState.Execute(); }
	}

    public void FixedUpdate()
    {
        if (currentState != null && active) { currentState.FixedExecute(); }
    }

    public void ChangeState(FSMState newState, bool runUpdate = true)
    {
        if (newState == null) Debug.Log("StateMachine: change to null state");

        if (currentState != null)
        {
            newState.previousState = currentState;
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();

        if (runUpdate)
            currentState.Execute();

        currentState.FixedExecute();
    }

    public void GoToPreviousState()
    {
        if (currentState.previousState != null)
        {
            currentState.Exit();
            currentState = currentState.previousState;
            currentState.Enter();
        }
    }

    public void ExitState()
    {
        currentState.Exit();
        currentState = null;
    }

    public void SetCurrentState(FSMState state) { currentState = state; }

    public bool IsInState(FSMState state) { return currentState == state; }

    public FSMState CurrentState() { return currentState; }
}