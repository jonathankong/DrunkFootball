using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.InputSystem.Users;
using UnityEngine.Windows;

public class CustomPlayerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputReader _inputReader;
    [SerializeField] private GameObject _playerPrefab;
    private List<Transform> _spawnPoints = new List<Transform>();

    [Header("Attributes")]
    [SerializeField] private int maxPlayers = 2;

    private int _spawnPointCounter = 0;

    private List<GameObject> players = new List<GameObject>();
    private List<InputUser> inputUsers = new List<InputUser>();
    public List<string> deviceNames;

    private void Awake()
    {
        //CreateInputUser();
        //InstantiatePlayerInput();
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void Start()
    {
        GetSpawnPoints();
        if (_spawnPoints.Count == 0) return;
        //SpawnPlayer();
    }

    private void SpawnPlayer(PlayerInputReader ir)
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
                CreateInputUser(device);
                //Spawning second player
                CreateInputUser();
                break;

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
    private void CreateInputUser(InputDevice device = null)
    {
        Debug.Log($"# of InputUsers {InputUser.all.Count} # of maxPlayers {maxPlayers}");
        if (InputUser.all.Count < maxPlayers)
        {
            // Create a new InputUser
            var inputUser = InputUser.CreateUserWithoutPairedDevices();

            if (device == null)
            {
                // Pair the keyboard and mouse to the same InputUser
                InputUser.PerformPairingWithDevice(Keyboard.current, inputUser);
                InputUser.PerformPairingWithDevice(Mouse.current, inputUser);
            }
            else
            {
                InputUser.PerformPairingWithDevice(device, inputUser);
            }

            //Setting up inputreader to read callbacks
            var ir = Instantiate(_inputReader);
            //ir.Initialize(inputUser);

            SpawnPlayer(ir);
            
            //Add inputuser to collection
            inputUsers.Add(inputUser);
        }
    }

    private void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
}
