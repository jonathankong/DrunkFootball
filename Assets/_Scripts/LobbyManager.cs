using PurrNet;
using System.Collections.Generic;
using UnityEngine;

public struct LobbyPlayerEntry
{
    public PlayerID PlayerId;
    public string DisplayName;
    public bool IsReady;
}

public class LobbyManager : NetworkIdentity
{
    [SerializeField] private SyncList<LobbyPlayerEntry> _roster = new();
    public SyncList<LobbyPlayerEntry> Roster => _roster;

    [ServerRpc(requireOwnership:false)]
    public void RequestJoinLobby_ServerRpc(RPCInfo info = default)
    {
        // Identify who called this
        var sender = info.sender;

        // Prevent duplicates
        for (int i = 0; i < _roster.Count; i++)
            if (_roster[i].PlayerId == sender)
                return;

        // Add to roster (this replicates to everyone)
        _roster.Add(new LobbyPlayerEntry
        {
            PlayerId = sender,
            DisplayName = $"Player {sender}",
            IsReady = false
        });
    }

    [ServerRpc(requireOwnership: false)]
    public void ToggleReady_ServerRpc(RPCInfo info = default)
    {
        var sender = info.sender;

        for (int i = 0; i < _roster.Count; i++)
        {
            if (_roster[i].PlayerId != sender)
                continue;

            var entry = _roster[i];
            entry.IsReady = !entry.IsReady;
            _roster[i] = entry; // update triggers replication/change notifications
            return;
        }
    }
}


