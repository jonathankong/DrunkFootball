using System;
using UnityEngine;

public interface IHasHealth
{
    event Action PlayerDied;
    event Action Damaged;
    void TakeDamage(float TakeDamage);
    void Heal(float amount);
    void FullHeal();
}
