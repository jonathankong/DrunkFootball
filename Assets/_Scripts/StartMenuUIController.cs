using System;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuUIController : MonoBehaviour
{
    [SerializeField] private GameSystemRefs _systems;
    [SerializeField] private Button _startButton;

    private LobbyController _lobbyController;
    private LocalInputController _inputController;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_systems == null || _startButton == null)
        {
            Debug.LogError("StartMenuController missing required references.", this);
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
        _lobbyController = _systems.LobbyController;
        _inputController = _systems.InputController;
    }

    private void OnEnable()
    {
        _inputController.OnSubmitCancelled += OnStartClicked;
    }

    private void OnDisable()
    {
        _inputController.OnSubmitCancelled -= OnStartClicked;
    }

    private void OnStartClicked()
    {
        _lobbyController.RequestEnterLobby();
    }
}
