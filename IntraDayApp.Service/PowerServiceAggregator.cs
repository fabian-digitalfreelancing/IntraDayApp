using IntraDayApp.Domain.Models;
using IntraDayApp.Domain.Responses;

namespace IntraDayApp.Service
{
    public interface PowerServiceAggregator
    {
        AggregateTradesResponse Aggregate(IEnumerable<Trade> tradeItems);
    }
}
