using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.InputSystem.Users;
using UnityEngine.Windows;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Controls;

public class CustomPlayerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();

    [Header("Attributes")]
    [SerializeField] private int maxPlayers = 2;

    private int _spawnPointCounter = 0;

    private List<GameObject> players = new List<GameObject>();
    private List<InputUser> inputUsers = new List<InputUser>();
    public List<string> deviceNames;

    private void Awake()
    {
        ++InputUser.listenForUnpairedDeviceActivity;
        InputUser.onUnpairedDeviceUsed += OnUnpairedDeviceUsed;
        //CreateInputUser();
        //InstantiatePlayerInput();
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnUnpairedDeviceUsed(InputControl control, InputEventPtr ptr)
    {
        //Checks intentional input
        if (!(control is ButtonControl))
            return;

        SetupInputUser(control.device);
    }

    private void Start()
    {
        GetSpawnPoints();
        if (_spawnPoints.Count == 0)
        {
            Debug.LogError($"No Spawn Points under {transform.name}");
            return;
        }
    }

    [ContextMenu("Dump InputUsers")]
    private void DumpUsers()
    {
        Debug.Log($"InputUsers: {InputUser.all.Count}");

        for (int i = 0; i < InputUser.all.Count; i++)
        {
            var user = InputUser.all[i];
            var devices = user.pairedDevices;

            var deviceList = devices.Count == 0
                ? "(none)"
                : string.Join(", ", devices);

            Debug.Log($"User[{i}] valid={user.valid} id={user.id} devices={deviceList}");
        }
    }

    [ContextMenu("Dump InputDevices")]
    private void DumpDevices()
    {
        Debug.Log($"InputDevices: {InputSystem.devices.Count}");

        for (int i = 0; i < InputSystem.devices.Count; i++)
        {
            var device = InputSystem.devices[i];

            Debug.Log(
                $"Device[{i}] " +
                $"id={device.deviceId} " +
                $"type={device.GetType().Name} " +
                $"name={device.name} " +
                $"layout={device.layout} " +
                $"enabled={device.enabled}"
            );
        }
    }

    private void SpawnPlayer(LocalInputReader ir)
    {
        Debug.Log("Spawning player");
        var player = Instantiate(_playerPrefab, _spawnPoints[_spawnPointCounter].transform.position, _spawnPoints[_spawnPointCounter].rotation);
        var playerInputComp = player.GetComponent<PlayerInputController>();
        if (playerInputComp != null)
        {
            //playerInputComp.AssignInputReader(ir);
        }
        _spawnPointCounter++;
    }

    private void GetSpawnPoints()
    {
        foreach (Transform t in GetComponentInChildren<Transform>())
        {
            _spawnPoints.Add(t);
        }
    }

    private void InstantiatePlayerInput(InputUser inputUser)
    {
        Debug.Log("Instantiating first player");
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        Debug.Log($"New Device! {device.name} {change}");
        switch (change)
        {
            case InputDeviceChange.Added:
                Debug.Log($"Device added: {device.displayName}");
                if (InputUser.all.Count < maxPlayers)
                {
                    inputUsers.Add(SetupInputUser(device));
                    Debug.Log($"{inputUsers[InputUser.all.Count - 1]} uses device: {device.name}");
                }
                break;
            case InputDeviceChange.Enabled:
            {
                break;
            }

            case InputDeviceChange.Removed:
                Debug.Log($"Device removed: {device.displayName}");
                break;

            case InputDeviceChange.Reconnected:
                Debug.Log($"Device reconnected: {device.displayName}");
                break;

            case InputDeviceChange.ConfigurationChanged:
                Debug.Log($"Device configuration changed: {device.displayName}");
                break;
        }
    }

    /// <summary>
    /// Uses the project-wide InputActionsAsset
    /// </summary>
    private InputUser SetupInputUser(InputDevice device = null)
    {
        Debug.Log($"# of InputUsers {InputUser.all.Count} # of maxPlayers {maxPlayers}");
        // Create a new InputUser
        // Keyboard → pair keyboard + mouse together
        InputUser inputUser = new InputUser();
        if (InputUser.all.Count < maxPlayers)
        {
            if (device is Keyboard)
            {
                inputUser = InputUser.PerformPairingWithDevice(device);

                if (Mouse.current != null)
                {
                    InputUser.PerformPairingWithDevice(Mouse.current, inputUser);
                }
            }
            // Mouse click → pair mouse + keyboard together
            else if (device is Mouse)
            {
                inputUser = InputUser.PerformPairingWithDevice(device);

                if (Keyboard.current != null)
                {
                    InputUser.PerformPairingWithDevice(Keyboard.current, inputUser);
                }
            }
            // Gamepad → just pair gamepad
            else
            {
                inputUser = InputUser.PerformPairingWithDevice(device);
            }

            if (inputUser != null)
            {
                Debug.Log($"Input User {inputUser.id} created with {device.name}");
            }
        }
        else
        {
            --InputUser.listenForUnpairedDeviceActivity;      
        }
        return inputUser;
    }

    private void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
        InputUser.onUnpairedDeviceUsed -= OnUnpairedDeviceUsed;
        --InputUser.listenForUnpairedDeviceActivity;
    }
}
