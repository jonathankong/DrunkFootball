using UnityEngine;

public interface IState
{
    void Enter();
    void Exit();
    //Performs on Update() when state is active
    //For timebased logic
    void Tick();
}
