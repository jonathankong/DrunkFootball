using UnityEngine;

public sealed class PausedState : IState
{

    public PausedState()
    {

    }

    public void Enter()
    {
        Debug.Log("Enter Paused State");
    }

    public void Exit()
    {
        Debug.Log("Exit Paused State");
    }

    public void Tick()
    {
        throw new System.NotImplementedException();
    }
}
