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

    public event sys.Action<bool> Attack1;
    public event sys.Action Attack1Started;
    public event sys.Action Attack1Cancelled;

    public event sys.Action<bool> Attack2;
    public event sys.Action Attack2Started;
    public event sys.Action Attack2Cancelled;

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
    public void Initialize(PlayerInput playerInput)
    {
        if (_inputActionsAsset == null)
        {
            _inputActionsAsset = new PlayerInputActions();
            _inputActionsAsset.Player.SetCallbacks(this);
        }

        //playerInput.user.AssociateActionsWithUser(_inputActionsAsset);

        _inputActionsAsset.Player.Enable();
        // If you use a UI map:
        // _inputActionsAsset.UI.Enable();
    }

    private void OnEnable()
    {
        // No automatic enabling to avoid shared instances in multiplayer.
    }

    private void OnDisable()
    {
        Cleanup();
    }

    /// <summary>
    /// Disable and clean up any active input mappings.
    /// </summary>
    public void Cleanup()
    {
        if (_inputActionsAsset != null)
        {
            _inputActionsAsset.Player.Disable();
            // _inputActionsAsset.UI.Disable();
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
        if (context.phase == InputActionPhase.Started)
            Attack1Started?.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            Attack1Cancelled?.Invoke();

        Attack1?.Invoke(context.ReadValueAsButton());
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
