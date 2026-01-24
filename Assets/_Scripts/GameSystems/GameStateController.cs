using Game.Core;
using Game.Lobby;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class GameStateController : MonoBehaviour
{
    [SerializeField] private GameSystemRefs _systems;
    [SerializeField] private GameState _initialGameState;

    private InputActions _actions;

    private StateMachine _sm;

    private IState _startState;
    private IState _playingState;
    private IState _pausedState;
    private IState _lobbyState;

    //A way for states to react when state has changed
    public Action<GameState> OnGameStateChanged;

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        _sm = new StateMachine();
        _actions = new InputActions();

        _startState = new StartState();
        _playingState = new PlayingState();
        _pausedState = new PausedState();
        _lobbyState = new LobbyState();
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_systems == null)
        {
            Debug.LogError("GameStateController missing required references.", this);
            enabled = false;
        }
        else
        {
            enabled = true;
        }
    }
#endif

    private void OnEnable()
    {
        SetInitialState(_initialGameState);
    }

    private void SetInitialState(GameState state)
    {
        switch (state)
        {
            case GameState.Start:
                ChangeState(_startState, GameState.Start);
                break;

            case GameState.Lobby:
                ChangeState(_lobbyState, GameState.Lobby);
                break;

            case GameState.Playing:
                ChangeState(_playingState, GameState.Playing);
                break;

            case GameState.Paused:
                ChangeState(_pausedState, GameState.Paused);
                break;

            default:
                ChangeState(_startState, GameState.Start);
                break;
        }
    }

    private void Update()
    {
        _sm.Tick();
    }

    // Single choke point so events always fire
    private void ChangeState(IState newState, GameState newStateId)
    {
        _sm.ChangeState(newState);
        CurrentState = newStateId;
        OnGameStateChanged?.Invoke(newStateId);
    }

    // Transitions (states call these)
    public void RequestStartGame()
    {
        // Typical: Lobby -> Playing (or Start -> Playing for debugging)
        if (CurrentState != GameState.Lobby && CurrentState != GameState.Start)
            return;

        ChangeState(_playingState, GameState.Playing);
    }

    public void RequestPauseGame()
    {
        if (CurrentState != GameState.Playing)
            return;

        ChangeState(_pausedState, GameState.Paused);
    }

    public void RequestResumeGame()
    {
        if (CurrentState != GameState.Paused)
            return;

        ChangeState(_playingState, GameState.Playing);
    }
}
