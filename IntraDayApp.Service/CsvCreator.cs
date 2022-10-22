using IntraDayApp.Domain.Models;
using IntraDayApp.Domain.Responses;

namespace IntraDayApp.Service
{
    public interface CsvCreator
    {
        CreateReportResponse CreateReport(IEnumerable<AggregatedTradeItem> tradeItems, string path);
    }
}
