using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.Rendering.Universal;
using sys = System;

/// <summary>
/// Handles local input c# event calls from PlayerInputActions asset
/// </summary>
public class LocalInputReader : MonoBehaviour
{
    private InputActions _inputActions;
    private InputUser _inputUser;

    private int _maxPlayers = 1;

    private bool _isAcceptingJoins = true;

    public event Action JumpStarted;

    public void Awake()
    {
        _inputActions = new InputActions();
        _inputUser = InputUser.CreateUserWithoutPairedDevices();

    }

    private void OnEnable()
    {
        Subscribe();
        _inputActions.Player.Jump.started += OnJump;
        _inputActions.Player.Enable();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("This is being called");
        transform.position += new Vector3(0 ,  10,  0);
    }

    private void OnDisable()
    {
        Unsubscribe();
        _inputActions.Disable();
        _inputActions.Player.Jump.started -= OnJump;
    }

    private bool HasJoined() => _inputUser.pairedDevices.Count > 0;

    private void Unsubscribe()
    {
        --InputUser.listenForUnpairedDeviceActivity;
        InputUser.onUnpairedDeviceUsed -= OnUnpairedDeviceUsed;
    }

    private void Subscribe()
    {
        ++InputUser.listenForUnpairedDeviceActivity;
        InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
    }

    private void OnUnpairedDeviceUsed(InputControl control, InputEventPtr ptr)
    {
        // can players keep joining?
        if (!_isAcceptingJoins)
            return;

        // Only accept intentional "press" type input
        if (control is not ButtonControl)
            return;

        // Only accept gamepads for Player 1
        if (control.device is not Gamepad gamepad)
            return;

        SetupInputUser(gamepad);
    }

    private void SetupInputUser(Gamepad device)
    {
        Debug.Log($"this device {device} is calling {nameof(SetupInputUser)}. Input user is {_inputUser.id}");
            //Check device
            //if (device is Keyboard)
            //{
            //    _inputUser = InputUser.PerformPairingWithDevice(device);
            //    if (Mouse.current != null)
            //        InputUser.PerformPairingWithDevice(Mouse.current, _inputUser);
            //}
            //else if (device is Mouse)
            //{
            //    _inputUser = InputUser.PerformPairingWithDevice(device);
            //    if (Keyboard.current != null)
            //    {
            //        InputUser.PerformPairingWithDevice(Keyboard.current, _inputUser);
            //    }
            //}
            //else 
            if (device is Gamepad)
            {
                InputUser.PerformPairingWithDevice(device, _inputUser);
                _inputUser.AssociateActionsWithUser(_inputActions);
                Debug.Log($"Input User {_inputUser.id}, num of devices {_inputUser.pairedDevices.Count} created with {string.Join(",", _inputUser.pairedDevices.Select(d => $"{d.displayName} {d.device}"))}");
            }

        if (_inputUser.pairedDevices.Count > 0)
        {
            _isAcceptingJoins = false;
            Unsubscribe();
        }
    }
}
