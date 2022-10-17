using IntraDayApp.Remote;

namespace IntraDayApp
{
    public sealed class WindowsBackgroundService : BackgroundService
    {
        private readonly PowerServiceWrapper _service;
        private readonly ILogger<WindowsBackgroundService> _logger;

        public WindowsBackgroundService(
            PowerServiceWrapper service,
            ILogger<WindowsBackgroundService> logger) =>
            (_service, _logger) = (service, logger);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var result = await _service.GetTradesAsync(new DateTime(2022, 1, 1));
                //while (!stoppingToken.IsCancellationRequested)
                //{
                //    string result = _service.GetTest();
                //    _logger.LogWarning("{result}", result);

                //    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                //}

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                Environment.Exit(1);
            }
        }
    }
}