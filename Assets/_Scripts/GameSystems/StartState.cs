using UnityEngine;

public sealed class StartState : IState
{
    public StartState()
    {

    }
    public void Enter()
    {
        Debug.Log("Entered Start Menu");

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Exit()
    {
        Debug.Log("Exit Start Menu");
    }

    public void Tick()
    {
    }
}