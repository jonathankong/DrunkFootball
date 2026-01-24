using Game.Core;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(GameStateController))]
public class LocalInputController : MonoBehaviour
{
    private InputActions _actions;
    private GameStateController _gameState;

    public Action OnSubmitCancelled;

    private void Awake()
    {
        _actions = new InputActions();
        _gameState = GetComponent<GameStateController>();
    }

    public void HandleGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                _actions.Player.Enable();
                _actions.UI.Disable();
                break;
            case GameState.Paused:
            case GameState.Start:
            case GameState.Lobby:
                _actions.Player.Disable();
                _actions.UI.Enable();
                break;
        }
    }
   

    private void OnEnable()
    {
        _gameState.OnGameStateChanged += HandleGameStateChanged;

        _actions.UI.Submit.canceled += Submit_released;
        _actions.Enable();
    }

    private void Submit_released(InputAction.CallbackContext obj)
    {
        Debug.Log($"{nameof(LocalInputController)} Submit cancelled");
        OnSubmitCancelled?.Invoke();
    }

    private void OnDisable()
    {
        _gameState.OnGameStateChanged -= HandleGameStateChanged;
        
        _actions.UI.Submit.canceled -= Submit_released;
        _actions.Disable();
    }
}
