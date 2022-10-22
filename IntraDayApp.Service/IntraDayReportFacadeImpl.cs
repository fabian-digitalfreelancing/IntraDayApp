using IntraDayApp.Domain.Enums;
using IntraDayApp.Domain.Interfaces.Remote;
using IntraDayApp.Domain.Interfaces.Service;
using Microsoft.Extensions.Logging;

namespace IntraDayApp.Service
{
    public class IntraDayReportFacadeImpl : IntraDayReportFacade
    {
        private readonly PowerServiceWrapper _powerService;
        private readonly PowerServiceAggregator _aggregator;
        private readonly ReportService _reportService;
        private readonly ILogger<IntraDayReportFacadeImpl> _logger;
        private readonly TimeProvider _timeProvider;
        public IntraDayReportFacadeImpl(PowerServiceWrapper powerService,
            PowerServiceAggregator aggregator,
            ReportService reportService,
            TimeProvider timeProvider,
            ILogger<IntraDayReportFacadeImpl> logger) =>
            (_powerService, _aggregator, _reportService, _timeProvider, _logger) =
            (powerService, aggregator, reportService, timeProvider, logger);


        public async Task CreateCsvIntraDayReportAsync(string fileLocation)
        {
            _logger.LogInformation($"Starting report creation at {_timeProvider.Now()} in {fileLocation}");

            var powerTradesResponse = await _powerService.GetTradesAsync(_timeProvider.TodaysDate());
            if (powerTradesResponse.Status != ServiceResponseStatus.Success)
            {
                _logger.LogInformation($"Report creation aborted at {_timeProvider.Now()} because of a Power Trade Service Error ");
                return;
            }

            var aggregatedTradesResponse = _aggregator.Aggregate(powerTradesResponse.Data);
            if (aggregatedTradesResponse.Status != ServiceResponseStatus.Success)
            {
                _logger.LogInformation($"Report creation aborted at {_timeProvider.Now()} because of a Aggregator Service Error");
                return;
            }

            var reportResponse = _reportService.CreateReport(aggregatedTradesResponse.Data, fileLocation);
            if (reportResponse.Status != ServiceResponseStatus.Success)
            {
                _logger.LogInformation($"Report creation aborted at {_timeProvider.Now()} because of a Csv Creator Error");
                return;
            }

            _logger.LogInformation($"Successfully saved report in {reportResponse.Data} at {_timeProvider.Now()}");
        }
    }
}
