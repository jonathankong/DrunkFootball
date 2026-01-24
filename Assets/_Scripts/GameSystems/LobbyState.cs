using UnityEngine;

public class LobbyState : IState
{

    public LobbyState()
    {

    }
    public void Enter()
    {
        Debug.Log("Enter Lobby State");
    }

    public void Exit()
    {
        Debug.Log("Exit Lobby State");
    }

    public void Tick()
    {
    }
}
