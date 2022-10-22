using IntraDayApp.Domain.Enums;
using IntraDayApp.Remote;
using Microsoft.Extensions.Logging;

namespace IntraDayApp.Service
{
    public class IntraDayReportFacadeImpl : IntraDayReportFacade
    {
        private readonly PowerServiceWrapper _powerService;
        private readonly PowerServiceAggregator _aggregator;
        private readonly CsvCreator _csvCreator;
        private readonly ILogger<IntraDayReportFacadeImpl> _logger;
        public IntraDayReportFacadeImpl(PowerServiceWrapper powerService,
            PowerServiceAggregator aggregator,
            CsvCreator csvCreator,
            ILogger<IntraDayReportFacadeImpl> logger) =>
            (_powerService, _aggregator, _csvCreator, _logger) =
            (powerService, aggregator, csvCreator, logger);


        public async Task CreateCsvIntraDayReportAsync(string fileLocation)
        {
            _logger.LogInformation($"Starting report creation at {DateTime.Now} in {fileLocation}");

            var powerTradesResponse = await _powerService.GetTradesAsync(DateTime.Today.Date);
            if (powerTradesResponse.Status != ServiceResponseStatus.Success)
            {
                _logger.LogError($"Report creation aborted at {DateTime.UtcNow} because of a Power Trade Service Error ");
                return;
            }

            var aggregatedTradesResponse = _aggregator.Aggregate(powerTradesResponse.Data);
            if (aggregatedTradesResponse.Status != ServiceResponseStatus.Success)
            {
                _logger.LogError($"Report creation aborted at {DateTime.UtcNow} because of a Aggregator Service Error");
                return;
            }

            var csvResponse = _csvCreator.CreateReport(aggregatedTradesResponse.Data, fileLocation);
            if (csvResponse.Status != ServiceResponseStatus.Success)
            {
                _logger.LogError($"Report creation aborted at {DateTime.UtcNow} because of a Csv Creator Error");
                return;
            }

            _logger.LogInformation($"Successfully saved report in {csvResponse.Data} at ${DateTime.Now}");
        }
    }
}
