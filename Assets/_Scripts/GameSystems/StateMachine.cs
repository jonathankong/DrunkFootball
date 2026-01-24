using UnityEngine;

public sealed class StateMachine
{
    public IState CurrentState => _currentState;
    private IState _currentState;

    public void ChangeState(IState nextState)
    {
        if (nextState == null)
        {
            Debug.LogError("StateMachine.ChangeState called with null nextState.");
            return;
        }

        if (ReferenceEquals(_currentState, nextState))
            return;

        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();
    }

    public void Tick()
    {
        _currentState?.Tick();
    }
}
