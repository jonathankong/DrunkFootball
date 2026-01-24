using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public sealed class InputUserRegistry: MonoBehaviour
{
    private readonly List<LocalPlayerHandle> _players = new();

    public IReadOnlyList<LocalPlayerHandle> Players => _players;

    public event Action<LocalPlayerHandle> PlayerAdded;
    public event Action<LocalPlayerHandle> PlayerRemoved;

    public LocalPlayerHandle CreatePlayer(InputActions actions, InputDevice primaryDevice)
    {
        if (actions == null) throw new ArgumentNullException(nameof(actions));
        if (primaryDevice == null) throw new ArgumentNullException(nameof(primaryDevice));

        var user = InputUser.CreateUserWithoutPairedDevices();
        InputUser.PerformPairingWithDevice(primaryDevice, user);

        user.AssociateActionsWithUser(actions.asset);
        actions.Enable();

        var handle = new LocalPlayerHandle(user, actions, primaryDevice);

        _players.Add(handle);
        PlayerAdded?.Invoke(handle);

        return handle;
    }

    public void RemovePlayer(LocalPlayerHandle player)
    {
        if (player == null)
            return;

        if (!_players.Remove(player))
            return;

        try
        {
            player.Actions?.Disable();

            if (player.User.valid)
                player.User.UnpairDevicesAndRemoveUser();
        }
        finally
        {
            PlayerRemoved?.Invoke(player);
        }
    }

    public bool IsDeviceAlreadyClaimed(InputDevice device)
    {
        if (device == null) return false;

        foreach (var p in _players)
        {
            if (p.User.valid && p.User.pairedDevices.Contains(device))
                return true;
        }

        return false;
    }
}

public sealed class LocalPlayerHandle
{
    public InputUser User { get; }
    public InputActions Actions { get; }
    public InputDevice PrimaryDevice { get; }
    public GameObject PlayerObject { get; internal set; }

    public LocalPlayerHandle(InputUser user, InputActions actions, InputDevice primaryDevice)
    {
        User = user;
        Actions = actions;
        PrimaryDevice = primaryDevice;
    }
}
