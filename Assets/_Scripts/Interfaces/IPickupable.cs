using System;
using UnityEngine;
public interface IPickupable
{
    public event Action OnItemPickup;
    public event Action OnItemDropped;

    void OnPickup(Transform holder);
    void OnDrop(); 
}
