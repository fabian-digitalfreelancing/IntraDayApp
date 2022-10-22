using CsvHelper;
using IntraDayApp.Domain.Models;
using IntraDayApp.Domain.Responses;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace IntraDayApp.Service
{
    public class CsvCreatorImpl : CsvCreator
    {
        private readonly ILogger<CsvCreatorImpl> _logger;
        public CsvCreatorImpl(ILogger<CsvCreatorImpl> logger) => (_logger) = (logger);
        public CreateReportResponse CreateReport(IEnumerable<AggregatedTradeItem> tradeItems, string path)
        {
            var filePath = $"{path}/PowerPosition_{DateTime.Now.ToString("yyyyMMdd_HHmm")}.csv";

            try
            {
                return WriteRecordsToFile(tradeItems, filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"CreateReport error with path: {path}", ex.Message);
                return CreateReportResponse.ErrorResponse(ex);
            }

        }

        private CreateReportResponse WriteRecordsToFile(IEnumerable<AggregatedTradeItem> tradeItems, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(tradeItems);
            }

            return CreateReportResponse.SuccessResponse(filePath);
        }
    }
}
