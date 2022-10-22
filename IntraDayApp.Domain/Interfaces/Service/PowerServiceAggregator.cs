using IntraDayApp.Domain.Models;
using IntraDayApp.Domain.Responses;

namespace IntraDayApp.Domain.Interfaces.Service
{
    public interface PowerServiceAggregator
    {
        AggregateTradesResponse Aggregate(IEnumerable<Trade> tradeItems);
    }
}
