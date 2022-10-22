using IntraDayApp.Domain.Models;
using IntraDayApp.Domain.Responses;
using Microsoft.Extensions.Logging;

namespace IntraDayApp.Service
{
    public class PowerServiceAggregatorImpl : PowerServiceAggregator
    {
        private readonly ILogger<PowerServiceAggregatorImpl> _logger;
        public PowerServiceAggregatorImpl(ILogger<PowerServiceAggregatorImpl> logger) => (_logger) = (logger);
        public AggregateTradesResponse Aggregate(IEnumerable<Trade> tradeItems)
        {
            try
            {
                return AggregateTradesResponse.SuccessResponse(AggregateTrades(tradeItems));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Aggregate failed while aggregating trade items");
                return AggregateTradesResponse.ErrorResponse(ex);
            }
        }

        private IEnumerable<AggregatedTradeItem> AggregateTrades(IEnumerable<Trade> tradeItems)
        {
            var result = new Dictionary<TimeOnly, double>();

            foreach (var trade in tradeItems)
            {
                foreach (var item in trade.Periods)
                {
                    var time = GetTimeFromPeriod(item.Period);
                    if (result.ContainsKey(time))
                    {
                        result[time] += item.Volume;
                    }
                    else
                    {
                        result[time] = item.Volume;
                    }
                }
            }

            return result.Select(item => new AggregatedTradeItem
            {
                LocalTime = item.Key,
                Volume = item.Value
            });
        }
        private TimeOnly GetTimeFromPeriod(int period)
        {
            try
            {
                if (period == 1)
                {
                    return new TimeOnly(23, 0);
                }
                else if (period <= 24)
                {
                    return new TimeOnly(period - 2, 0);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(period));
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.LogError($"GetTimeFromPeriod out of range error with period: {period}", ex);
                throw ex;
            }

        }
    }
}
