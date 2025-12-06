using PurrNet;
using System.Data;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputReader))]
public class PlayerInputController : NetworkIdentity
{
    [Header("Input")]
    private PlayerInputReader _playerInputReader;

    [Header("Gameplay")]
    [SerializeField] private Color _color;
    [SerializeField] private Renderer _renderer;

    private Vector2 _moveInput;

    private void Awake()
    {
        // Grab components on the same GameObject if not wired in Inspector
        if (_playerInputReader == null)
            _playerInputReader = GetComponent<PlayerInputReader>();

        if (_renderer == null)
            _renderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        // Subscribe to input events from the reader
        _playerInputReader.MovePerformed += OnMove;

        _playerInputReader.AttackStarted += OnAttackStarted;
        _playerInputReader.AttackCancelled += OnAttackCancelled;

        _playerInputReader.Pickup += OnPickupButton;
        _playerInputReader.PickupPerformed += OnPickupPerformed;
        _playerInputReader.PickupCancelled += OnPickupCancelled;

        _playerInputReader.ToggleDebugMenu += OnToggleDebugMenu;
    }

    private void OnDisable()
    {
        if (!isOwner || _playerInputReader == null) return;

        // Always unsubscribe
        _playerInputReader.MovePerformed -= OnMove;

        _playerInputReader.AttackStarted -= OnAttackStarted;
        _playerInputReader.AttackCancelled -= OnAttackCancelled;

        _playerInputReader.Pickup -= OnPickupButton;
        _playerInputReader.PickupPerformed -= OnPickupPerformed;
        _playerInputReader.PickupCancelled -= OnPickupCancelled;

        _playerInputReader.ToggleDebugMenu -= OnToggleDebugMenu;
    }

    // === Input callbacks from PlayerInputReader ===

    private void OnMove(Vector2 move)
    {
        _moveInput = move;
    }

    private void OnAttackStarted()
    {
        Debug.Log("Attack 1 started");
        SetColor(_color);
    }
    [ObserversRpc]
    private void SetColor(Color color)
    {
        Debug.Log(color);
        _renderer.material.color = color;
    }

    private void OnAttackCancelled()
    {
        //if (_combat != null)
        //{
        //    _combat.CancelPrimaryAttack();
        //}
    }

    private void OnPickupButton(bool isPressed)
    {
        //if (_pickup != null)
        //{
        //    _pickup.SetPickupHeld(isPressed);
        //}
    }

    private void OnPickupPerformed()
    {
        //if (_pickup != null)
        //{
        //    _pickup.TryPickup();
        //}
    }

    private void OnPickupCancelled()
    {
        //if (_pickup != null)
        //{
        //    _pickup.CancelPickup();
        //}
    }

    private void OnToggleDebugMenu()
    {
        // Call into your debug UI / manager
        Debug.Log("Toggle debug menu");
    }
}
