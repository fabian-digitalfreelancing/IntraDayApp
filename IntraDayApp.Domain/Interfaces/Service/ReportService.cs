using IntraDayApp.Domain.Models;
using IntraDayApp.Domain.Responses;

namespace IntraDayApp.Domain.Interfaces.Service
{
    public interface ReportService
    {
        CreateReportResponse CreateReport(IEnumerable<AggregatedTradeItem> tradeItems, string path);
    }
}
