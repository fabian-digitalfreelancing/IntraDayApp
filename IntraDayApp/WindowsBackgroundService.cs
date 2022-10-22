using IntraDayApp.Domain.AppSettings;
using IntraDayApp.Service;
using Microsoft.Extensions.Options;

namespace IntraDayApp
{
    public sealed class WindowsBackgroundService : BackgroundService
    {
        private readonly IntraDayReportFacade _reportFacade;
        private readonly ILogger<WindowsBackgroundService> _logger;
        private readonly IOptions<ReportSettings> _options;
        private readonly WorkerHelper _workerHelper;

        public WindowsBackgroundService(
            IntraDayReportFacade reportFacade,
            ILogger<WindowsBackgroundService> logger,
            IOptions<ReportSettings> options,
            WorkerHelper workerHelper) =>
            (_reportFacade, _logger, _options, _workerHelper) = (reportFacade, logger, options, workerHelper);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"IntraDayApp Starting, new report to be saved every {_options.Value.Frequency} minute(s) to {_options.Value.Location}");
            try
            {
                while (!await _workerHelper.StartTimer(TimeSpan.FromMinutes(_options.Value.Frequency), stoppingToken))
                {
                    await _reportFacade.CreateCsvIntraDayReportAsync(_options.Value.Location);
                    await _workerHelper.DelayForMinutes(_options.Value.Frequency, stoppingToken);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                Environment.Exit(1);
            }
        }
    }
}