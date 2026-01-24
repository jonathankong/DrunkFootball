using UnityEngine;

public interface IControllable
{
    // Called when this pawn becomes / stops being controlled
    void OnPossessed();
    void OnUnpossessed();
    void AttackPressed();
    void JumpPressed();
}
