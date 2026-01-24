using PurrNet;
using UnityEngine;

public class LocalPlayerIdentity : MonoBehaviour, ILocalPlayerIdentity
{
    [SerializeField] private NetworkManager _networkManager;
    public PlayerID PlayerId {  get; private set; }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_networkManager == null)
        {
            Debug.LogError("LocalPlayerIdentity missing required references.", this);
            enabled = false;
        }
        else
        {
            enabled = true;
        }
    }
#endif
    
    private void Start()
    {
        if (_networkManager != null) 
            PlayerId = _networkManager.localPlayer;
    }
}
