using CsvHelper;
using IntraDayApp.Domain.Interfaces.Service;
using IntraDayApp.Domain.Models;
using System.Globalization;

namespace IntraDayApp.Service
{
    public class CsvServiceWrapperImpl : CsvServiceWrapper
    {
        public void WriteToFile(IEnumerable<AggregatedTradeItem> tradeItems, string filePath)
        {
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(tradeItems);
        }
    }
}
