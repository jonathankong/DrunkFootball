using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Hooks into Player Input Manager events to handle spawning
/// </summary>
public class PlayerSetupManager : MonoBehaviour
{
    [Header("Input")]
    //[SerializeField] private InputReader _inputReaderTemplate;

    [Header("Spawn")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int spawnCount;

    private int playerCounter = 1;

    public void HandlePlayerJoined(PlayerInput player)
    {
        SpawnPlayer(player);
        InitializePlayerInput(player);
    }

    public void HandlePlayerLeft(PlayerInput player)
    {
        DespawnPlayer(player);
    }

    private void InitializePlayerInput(PlayerInput player)
    {

        //player.gameObject.name = $"Player {playerCounter}";
        //playerCounter++;

        //var device = player.devices[0];
        //Debug.Log($"{player.gameObject.name} joined with device: {device.displayName}");
        ////Create a copy of the _inputreader asset;
        ////InputReader inputReaderInstance = Instantiate(_iknputReaderTemplate);

        //// Assign and lock the control scheme
        //if (device is Keyboard || device is Mouse)
        //{
        //    player.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
        //}
        //else if (device is Gamepad gamepad)
        //{
        //    player.SwitchCurrentControlScheme("Gamepad", gamepad);
        //}

        ////Initialize the InputReader copy related to PlayerInput component
        ////inputReaderInstance.Initialize(player);

        ////Pass Player's instance of input reader to all components that need it
        //foreach (var initializable in player.GetComponents<IInitializable>())
        //{
        //    initializable.Initialize(inputReaderInstance);
        //}
        //Debug.Log($"Initialized {player.gameObject.name} with InputReader: {inputReaderInstance.GetInstanceID()}");
    }

    private void SpawnPlayer(PlayerInput player)
    {
        player.gameObject.name = $"Player {playerCounter}";
        playerCounter++;

        if (TryGetSpawnPoint(out Transform spawnPoint))
        {
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
            spawnCount++;
        }
    }

    private bool TryGetSpawnPoint(out Transform transform)
    {
        if (spawnPoints.Length == 0) { 
            transform = null;
            return false;
        }

        transform = spawnPoints[spawnCount];
        return true;
    }

    private void DespawnPlayer(PlayerInput player)
    {
        spawnCount--;
    }
}
