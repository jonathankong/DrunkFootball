using PurrNet;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerInputReader))]
public class PlayerInputController : NetworkIdentity
{
    [Header("Input")]
    private PlayerInputReader _playerInputReader;
    private PlayerInput _playerInput;

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

        _playerInput = GetComponent<PlayerInput>();

        // IMPORTANT: tie this reader to THIS player's PlayerInput
        _playerInputReader.Initialize(_playerInput);
    }

    private void OnEnable()
    {
        // Subscribe to input events from the reader
        _playerInputReader.MovePerformed += OnMove;

        _playerInputReader.Attack1 += OnAttack1;
        _playerInputReader.Attack1Started += OnAttack1Started;
        _playerInputReader.Attack1Cancelled += OnAttack1Cancelled;

        _playerInputReader.Pickup += OnPickupButton;
        _playerInputReader.PickupPerformed += OnPickupPerformed;
        _playerInputReader.PickupCancelled += OnPickupCancelled;

        _playerInputReader.ToggleDebugMenu += OnToggleDebugMenu;
    }

    private void OnDisable()
    {
        // Always unsubscribe
        _playerInputReader.MovePerformed -= OnMove;

        _playerInputReader.Attack1 -= OnAttack1;
        _playerInputReader.Attack1Started -= OnAttack1Started;
        _playerInputReader.Attack1Cancelled -= OnAttack1Cancelled;

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

    private void OnAttack1(bool isPressed)
    {
        Debug.Log("Attack 1 pressed");
    }

    [ServerRpc]
    private void OnAttack1Started()
    {
        _renderer.material.color = _color;
    }

    private void OnAttack1Cancelled()
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

    // === Main update loop using cached input ===

    private void Update()
    {
        //if (_movement != null)
        //{
        //    _movement.Move(_moveInput);
        //}
    }
}
