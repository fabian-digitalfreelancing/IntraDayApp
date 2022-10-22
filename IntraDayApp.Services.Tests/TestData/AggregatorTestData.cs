using IntraDayApp.Domain.Models;
using System.Collections.Generic;

namespace IntraDayApp.Service.Tests
{
    public static class AggregatorTestData
    {
        public static List<Trade> TwoTradesWithTwoMatchingPeriods = new()
        {
            new()
            {
                Periods = new[] {
                        new TradePeriod(period: 1, volume: 100),
                        new TradePeriod(period: 2, volume: 200)
                    }
            },
            new()
            {
                Periods = new[] {
                        new TradePeriod(period: 1, volume: 50),
                        new TradePeriod(period: 2, volume: 60)
                    }
            }
        };
        public static List<AggregatedTradeItem> TwoTradesWithTwoMatchingPeriodsExpectedResult = new()
        {
            new()
            {
                LocalTime = new System.TimeOnly(23, 0),
                Volume = 150
            },
            new()
            {
                LocalTime = new System.TimeOnly(0, 0),
                Volume = 260
            }
        };

        public static List<Trade> TwoTradesWithTwoNonMatchingPeriods = new()
        {
            new()
            {
                Periods = new[] {
                        new TradePeriod(period: 1, volume: 100),
                        new TradePeriod(period: 2, volume: 200)
                    }
            },
            new()
            {
                Periods = new[] {
                        new TradePeriod(period: 3, volume: 50),
                        new TradePeriod(period: 4, volume: 60)
                    }
            }
        };
        public static List<AggregatedTradeItem> TwoTradesWithTwoNonMatchingPeriodsExpectedResult = new()
        {
            new()
            {
                LocalTime = new System.TimeOnly(23, 0),
                Volume = 100
            },
            new()
            {
                LocalTime = new System.TimeOnly(0, 0),
                Volume = 200
            },
            new()
            {
                LocalTime = new System.TimeOnly(1, 0),
                Volume = 50
            },
            new()
            {
                LocalTime = new System.TimeOnly(2, 0),
                Volume = 60
            }
        };

        public static List<Trade> TradeWithNegativePeriods = new()
        {
            new()
            {
                Periods = new[] {
                        new TradePeriod(period: -1, volume: 100),
                        new TradePeriod(period: -2, volume: 200)
                    }
            }
        };
        public static List<Trade> TradeWithTooHighPeriods = new()
        {
            new()
            {
                Periods = new[] {
                        new TradePeriod(period: 25, volume: 100)
                    }
            }
        };

        public static List<Trade> TradeWithZeroValuePeriods = new()
        {
            new()
            {
                Periods = new[] {
                        new TradePeriod(period: 0, volume: 100)
                    }
            }
        };
    };
}
