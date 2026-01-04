namespace Core.Interfaces.Services
{
    public interface IMatchTimer
    {
        void StartTimer(Guid matchId, Guid palyerId, TimeSpan duration);
        void StopTimer(Guid matchId);
        bool HasActiveTimer(Guid matchId);
    }
}
