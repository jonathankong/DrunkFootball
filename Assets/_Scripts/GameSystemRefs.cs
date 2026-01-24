using UnityEngine;

public sealed class GameSystemRefs : MonoBehaviour
{
    [field: SerializeField] public LocalInputController InputController { get; private set; }
    [field: SerializeField] public GameStateController GameStateController { get; private set; }
    [field: SerializeField] public LobbyController LobbyController { get; private set; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Auto-fill from same GameObject if not assigned
        if (InputController == null)
            InputController = GetComponent<LocalInputController>();

        if (GameStateController == null)
            GameStateController = GetComponent<GameStateController>();

        if (LobbyController == null)
            LobbyController = GetComponent<LobbyController>();
    }
#endif
}
