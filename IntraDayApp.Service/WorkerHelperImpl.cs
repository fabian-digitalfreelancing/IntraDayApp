namespace IntraDayApp.Service
{
    public class WorkerHelperImpl : WorkerHelper
    {
        public async Task<ValueTask<bool>> StartTimer(TimeSpan timespan, CancellationToken stoppingToken)
        {
            var timer = new PeriodicTimer(timespan);
            return timer.WaitForNextTickAsync(stoppingToken);
        }
    }
}
