using IntraDayApp.Domain.Interfaces.App;

namespace IntraDayApp
{
    public class TimerWrapperImpl : TimerWrapper
    {
        private PeriodicTimer _timer;
        public void Initialize(TimeSpan timespan)
        {
            _timer = new PeriodicTimer(timespan);
        }
        public async Task<bool> WaitForNextTickAsync(CancellationToken stoppingToken)
        {
            return await _timer.WaitForNextTickAsync(stoppingToken);
        }
    }
}
