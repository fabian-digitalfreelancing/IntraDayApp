namespace IntraDayApp.Service
{
    public interface WorkerHelper
    {
        Task<ValueTask<bool>> StartTimer(TimeSpan timespan, CancellationToken stoppingToken);
    }
}
