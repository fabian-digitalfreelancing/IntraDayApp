using IntraDayApp.Domain.Models;

namespace IntraDayApp.Domain.Interfaces.Service
{
    public interface CsvServiceWrapper
    {
        void WriteToFile(IEnumerable<AggregatedTradeItem> tradeItems, string filePath);
    }
}
