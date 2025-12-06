using sys = System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

/// <summary>
/// Per-player input reader. Attach this to a player object.
/// </summary>
public class PlayerInputReader : MonoBehaviour, PlayerInputActions.IPlayerActions
{
    private PlayerInputActions _inputActionsAsset;

    public event sys.Action<Vector2> MovePerformed;

    public event sys.Action<bool> Attack;
    public event sys.Action AttackStarted;
    public event sys.Action AttackCancelled;

    public event sys.Action ToggleDebugMenu;
    public event sys.Action<bool> DebugMouseSelect;

    public event sys.Action<bool> Pickup;
    public event sys.Action PickupPerformed;
    public event sys.Action PickupCancelled;

    //// Store the assigned device
    //public InputDevice AssignedDevice => _playerInput ? _playerInput.devices[0] : null;

    /// <summary>
    /// Initialize the InputReader with a specific PlayerInput instance for multiplayer support.
    /// </summary>
    private void Awake()
    {
        // Create a dedicated actions instance for this player
        _inputActionsAsset = new PlayerInputActions();
        _inputActionsAsset.Player.SetCallbacks(this);

        Debug.Log($"[PlayerInputReader] Awake on {gameObject.name} in scene {gameObject.scene.name}");
    }

    private void OnEnable()
    {
        if (_inputActionsAsset == null)
        {
            _inputActionsAsset = new PlayerInputActions();
            _inputActionsAsset.Player.SetCallbacks(this);
        }

        _inputActionsAsset.Player.Enable();
        Debug.Log($"[PlayerInputReader] Enabled on {gameObject.name}");
    }

    private void OnDisable()
    {
        if (_inputActionsAsset != null)
        {
            _inputActionsAsset.Player.Disable();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
        {
            MovePerformed?.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnPickup(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
        {
            Pickup?.Invoke(context.ReadValueAsButton());
        }

        if (context.phase == InputActionPhase.Performed)
            PickupPerformed?.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            PickupCancelled?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // Placeholder for future implementation
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        //Debug.Log($"[PlayerInputReader] OnAttack fired on {gameObject.name} in scene {gameObject.scene.name}, phase={context.phase}");

        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("Attack Started!");
            AttackStarted?.Invoke();
        }

        if (context.phase == InputActionPhase.Canceled)
            AttackCancelled?.Invoke();

        Attack?.Invoke(context.ReadValueAsButton());
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        // Placeholder for future implementation
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Placeholder for future implementation
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        // Placeholder for future implementation
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        // Placeholder for future implementation
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        // Placeholder for future implementation
    }

    #region Debug Menu
    // Example debug menu handlers
    public void OnDebugToggle(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            ToggleDebugMenu?.Invoke();
    }

    public void OnMouseSelect(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
            DebugMouseSelect?.Invoke(true);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        throw new sys.NotImplementedException();
    }
    #endregion
}
