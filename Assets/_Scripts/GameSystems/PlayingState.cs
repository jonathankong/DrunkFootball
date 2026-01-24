using UnityEngine;

public class PlayingState : IState
{

    public PlayingState()
    {
    }
    public void Enter()
    {
        Debug.Log("Enter Playing State");
    }

    public void Exit()
    {
        Debug.Log("Exit Playing State");
    }

    public void Tick()
    {
    }
}
