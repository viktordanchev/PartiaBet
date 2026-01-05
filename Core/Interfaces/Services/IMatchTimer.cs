namespace Core.Interfaces.Services
{
    public interface IMatchTimer
    {
        void StartLeaverTimer(Guid matchId, Guid palyerId);
        void StopTimer(Guid matchId);
        bool HasActiveTimer(Guid matchId);
    }
}
