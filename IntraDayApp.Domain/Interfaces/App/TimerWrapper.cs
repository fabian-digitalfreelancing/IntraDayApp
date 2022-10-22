namespace IntraDayApp.Domain.Interfaces.App
{
    public interface TimerWrapper
    {
        void Initialize(TimeSpan timespan);
        Task<bool> WaitForNextTickAsync(CancellationToken stoppingToken);
    }
}
