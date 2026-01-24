using Game.Core;
using System;
using UnityEngine;

[RequireComponent (typeof(GameStateController))]
public class UIPanelController : MonoBehaviour
{
    private GameStateController _gameState;
    [SerializeField] private GameObject _startMenuPanel;
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _lobbyMenuPanel;
    [SerializeField] private GameObject _characterSelectPanel;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_startMenuPanel == null || _pauseMenuPanel == null || _lobbyMenuPanel == null || _characterSelectPanel == null)
        {
            Debug.LogError($"{nameof(UIPanelController)} missing required references.", this);
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
        _gameState = gameObject.GetComponent<GameStateController>();
    }

    private void OnEnable()
    {
        _gameState.OnGameStateChanged += HandleOnGameStateChanged;
    }
    
    private void OnDisable()
    {
        _gameState.OnGameStateChanged -= HandleOnGameStateChanged;
    }
    private void HandleOnGameStateChanged(GameState state)
    {
        _startMenuPanel.SetActive(state == GameState.Start);
        _pauseMenuPanel.SetActive(state == GameState.Paused);
        _lobbyMenuPanel.SetActive(state == GameState.Lobby);
        if (state == GameState.Playing)
            _characterSelectPanel.SetActive(false);
    }
}
