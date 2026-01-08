namespace Core.Models.Match
{
    public class PlayerTimer
    {
        public DateTime DeadlineDateTime { get; set; }
        public TimeSpan RemainingTime { get; set; }
        public CancellationTokenSource? Cts { get; set; }
    }
}
