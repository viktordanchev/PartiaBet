namespace RestAPI.Services
{
    public class TimerState
    {
        public TimerState()
        {
            CancellationTokenSource = new CancellationTokenSource();
            IsPaused = false;
        }

        public CancellationTokenSource CancellationTokenSource { get; private set; }
        public TimeSpan RemainingTime { get; set; }
        public DateTime StartedAt { get; set; }
        public bool IsPaused { get; set; }
    }
}
