namespace Game.Lobby
{
    public enum JoinFailureReason
    {
        Unknown,
        LobbyFull,
        Timeout,
        ServerRejected,
        AlreadyJoined,
        VersionMismatch,
        NetworkError
    }
}
