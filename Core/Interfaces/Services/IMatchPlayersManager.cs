namespace Core.Interfaces.Services
{
    public interface IMatchPlayersManager
    {
        void MarkAsDisconnected(Guid userId);
        bool IsStillDisconnected(Guid userId);
        void MarkAsConnected(Guid userId);
    }
}
