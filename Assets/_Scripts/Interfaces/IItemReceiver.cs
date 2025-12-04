using UnityEngine;

public interface IItemReceiver
{
    bool IsHoldingItem { get; }
    bool TryReceiveItem(IPickupable item);
}
