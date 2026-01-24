using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

public class LobbyUIController : MonoBehaviour
{
    [Header("Systems")]
    [SerializeField] private GameSystemRefs _systems;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _playerList;

    private LocalInputController _inputController;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if ( _playerList == null || _systems == null)
        {
            Debug.LogError("LobbyUIController missing required references.", this);
            enabled = false;
        }
        else
        {
            enabled = true;
        }
    }
#endif

    private void Awake()
    {
        _inputController = _systems.InputController;
    }

    private void RebuildRoster()
    {
        _playerList.text = string.Empty;
    }

    private void Roster_onChanged(PurrNet.SyncListChange<LobbyPlayerEntry> change)
    {
        RebuildRoster();
    }

    private void OnEnable()
    {
        RebuildRoster();
    }

    private void OnDisable()
    {
    }
}
