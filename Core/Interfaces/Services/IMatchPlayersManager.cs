namespace Core.Interfaces.Services
{
    public interface IMatchPlayersManager
    {
        void MarkAsDisconnected(Guid playerId);
        bool IsStillDisconnected(Guid playerId);
        void MarkAsConnected(Guid playerId);
    }
}
