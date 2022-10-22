using IntraDayApp.Domain.Interfaces.Service;
using IntraDayApp.Domain.Models;
using IntraDayApp.Domain.Responses;
using Microsoft.Extensions.Logging;

namespace IntraDayApp.Service
{
    public class ReportServiceImpl : ReportService
    {
        private readonly ILogger<ReportServiceImpl> _logger;
        private readonly CsvServiceWrapper _csvService;
        private readonly TimeProvider _timeProvider;

        public ReportServiceImpl(ILogger<ReportServiceImpl> logger,
            CsvServiceWrapper csvService,
            TimeProvider timeProvider) => (_logger, _csvService, _timeProvider) = (logger, csvService, timeProvider);
        public CreateReportResponse CreateReport(IEnumerable<AggregatedTradeItem> tradeItems, string path)
        {
            var filePath = $"{path}/PowerPosition_{_timeProvider.Now():yyyyMMdd_HHmm}.csv";

            try
            {
                _csvService.WriteToFile(tradeItems, filePath);
                return CreateReportResponse.SuccessResponse(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CreateReport error with path: {path}", ex.Message);
                return CreateReportResponse.ErrorResponse(ex);
            }

        }
    }
}
