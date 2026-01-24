using Game.Core;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.Windows;

public sealed class GameFlowController : MonoBehaviour
{
    [SerializeField] private GameSystemRefs _systems;

    private GameStateController _gameState;
    private LobbyController _lobby;
    private bool _playersSpawned;
    private int numOfInputUsers;


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_systems == null)
            _systems = GetComponent<GameSystemRefs>();
    }
#endif

    private void Awake()
    {
        _gameState = _systems.GameStateController;
        _lobby = _systems.LobbyController;
    }

    private void Update()
    {
        if (numOfInputUsers != InputUser.all.Count)
        {
            numOfInputUsers = InputUser.all.Count;
            Debug.Log($"New count of input users {numOfInputUsers}");
        }
    }

    private void OnEnable()
    {
        _gameState.OnGameStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        _gameState.OnGameStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Start:
                {
                    _playersSpawned = false;
                    // optional cleanup
                    // _players.DespawnAll();
                    break;
                }

            case GameState.Lobby:
                {
                    _playersSpawned = false;
                    break;
                }

            case GameState.Playing:
                {
                    // IMPORTANT: only spawn once (and usually only on the host/server)
                    if (!_playersSpawned)
                    {
                        _playersSpawned = true;
                        //_players.SpawnPlayers();
                    }

                    break;
                }

            case GameState.Paused:
                {
                    // If single-player / offline:
                    // Time.timeScale = 0f;

                    break;
                }

            default:
                {
                    Debug.LogWarning($"Unhandled game state: {newState}");
                    break;
                }
        }
    }
}