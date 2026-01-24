using Cysharp.Threading.Tasks;
using PurrNet;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using Game.Lobby;

public class LobbyController : MonoBehaviour
{
    [SerializeField] private LobbyManager _lobbyManager;
    [SerializeField] private int _networkManager;

    //Limits user to make multi requests for some reason
    private bool _requestedJoin;

    private CancellationTokenSource _cts;

    public event Action JoinedLobby;
    public event Action<JoinFailureReason> JoinFailed;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_lobbyManager == null)
        {
            Debug.LogError("LobbyController missing required references.", this);
            enabled = false;
        }
        else
        {
            enabled = true;
        }
    }
#endif



    //like a coroutine that runs every frame until the lobby manager is not null and has spawned in
    //this is to allow us to rebuild the roster right when lobby manager is ready
    private async UniTaskVoid WaitForLobbyManagerSpawnAsync(CancellationToken cancelToken)
    {
        await UniTask.WaitUntil(() =>
        {
            if (_lobbyManager == null)
                return false;
            return _lobbyManager.isSpawned;
        }, cancellationToken: cancelToken);
    }

    public void RequestEnterLobby()
    {
        if (_requestedJoin) return;
        _requestedJoin = true;

        _lobbyManager.RequestJoinLobby_ServerRpc();
    }

    private void OnEnable()
    {
        _cts = new CancellationTokenSource();
        WaitForLobbyManagerSpawnAsync(_cts.Token).Forget();
        _lobbyManager.Roster.onChanged += Roster_onChanged;
    }

    private void Roster_onChanged(SyncListChange<LobbyPlayerEntry> change)
    {
        if (change.operation != SyncListOperation.Added)
            return;

        //if (change.value. != _localClientId)
        //    return;

        Debug.Log($"Roster {change.operation} at index {change.index}. Now contains: {string.Join(", ", change.value.DisplayName)}");

    }

    private void OnDisable()
    {
        _lobbyManager.Roster.onChanged -= Roster_onChanged;
        _cts.Cancel();
        _cts.Dispose();
    }


}
