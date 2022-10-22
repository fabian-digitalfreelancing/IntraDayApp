using IntraDayApp.Domain.AppSettings;
using IntraDayApp.Domain.Interfaces.App;
using IntraDayApp.Domain.Interfaces.Service;
using Microsoft.Extensions.Options;

namespace IntraDayApp
{
    public sealed class WindowsBackgroundService : BackgroundService
    {
        private readonly IntraDayReportFacade _reportFacade;
        private readonly ILogger<WindowsBackgroundService> _logger;
        private readonly IOptions<ReportSettings> _options;
        private readonly TimerWrapper _timer;

        public WindowsBackgroundService(
            IntraDayReportFacade reportFacade,
            ILogger<WindowsBackgroundService> logger,
            IOptions<ReportSettings> options,
            TimerWrapper timer) =>
            (_reportFacade, _logger, _options, _timer) = (reportFacade, logger, options, timer);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("IntraDayApp Starting");
            try
            {
                _timer.Initialize(TimeSpan.FromMinutes(_options.Value.Frequency));
                do
                {
                    await _reportFacade.CreateCsvIntraDayReportAsync(_options.Value.Location);
                } while (await _timer.WaitForNextTickAsync(stoppingToken)
                    && !stoppingToken.IsCancellationRequested);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                Environment.Exit(1);
            }
        }
    }
}