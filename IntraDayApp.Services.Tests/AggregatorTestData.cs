using IntraDayApp.Domain.Models;
using System.Collections.Generic;

namespace IntraDayApp.Service.Tests
{
    public static class AggregatorTestData
    {
        public static IEnumerable<Trade> TwoTradesWithTwoMatchingPeriods = new List<Trade>()
            {
                new Trade
                {
                    Periods = new TradePeriod[] {
                        new TradePeriod{
                            Period = 1,
                            Volume = 100
                        },
                        new TradePeriod{
                            Period = 2,
                            Volume = 200
                        }
                    }
                },
                new Trade
                {
                    Periods = new TradePeriod[] {
                        new TradePeriod{
                            Period = 1,
                            Volume = 50
                        },
                        new TradePeriod{
                            Period = 2,
                            Volume = 60
                        }
                    }
                }
            };
        public static IEnumerable<AggregatedTradeItem> TwoTradesWithTwoMatchingPeriodsExpectedResult = new List<AggregatedTradeItem>()
        {
            new AggregatedTradeItem
            {
                LocalTime = new System.TimeOnly(23, 0),
                Volume = 150
            },
            new AggregatedTradeItem
            {
                LocalTime = new System.TimeOnly(0, 0),
                Volume = 260
            }
        };

        public static IEnumerable<Trade> TwoTradesWithTwoNonMatchingPeriods = new List<Trade>()
            {
                new Trade
                {
                    Periods = new TradePeriod[] {
                        new TradePeriod{
                            Period = 1,
                            Volume = 100
                        },
                        new TradePeriod{
                            Period = 2,
                            Volume = 200
                        }
                    }
                },
                new Trade
                {
                    Periods = new TradePeriod[] {
                        new TradePeriod{
                            Period = 3,
                            Volume = 50
                        },
                        new TradePeriod{
                            Period = 4,
                            Volume = 60
                        }
                    }
                }
            };
        public static IEnumerable<AggregatedTradeItem> TwoTradesWithTwoNonMatchingPeriodsExpectedResult = new List<AggregatedTradeItem>()
        {
            new AggregatedTradeItem
            {
                LocalTime = new System.TimeOnly(23, 0),
                Volume = 100
            },
            new AggregatedTradeItem
            {
                LocalTime = new System.TimeOnly(0, 0),
                Volume = 200
            },
            new AggregatedTradeItem
            {
                LocalTime = new System.TimeOnly(1, 0),
                Volume = 50
            },
            new AggregatedTradeItem
            {
                LocalTime = new System.TimeOnly(2, 0),
                Volume = 60
            }
        };

        public static IEnumerable<Trade> TradeWithNegativePeriods = new List<Trade>()
            {
                new Trade
                {
                    Periods = new TradePeriod[] {
                        new TradePeriod{
                            Period = -1,
                            Volume = 100
                        },
                        new TradePeriod{
                            Period = -2,
                            Volume = 200
                        }
                    }
                }
            };
        public static IEnumerable<Trade> TradeWithTooHighPeriods = new List<Trade>()
            {
                new Trade
                {
                    Periods = new TradePeriod[] {
                        new TradePeriod{
                            Period = 25,
                            Volume = 100
                        }
                    }
                }
            };

        public static IEnumerable<Trade> TradeWithZeroValuePeriods = new List<Trade>()
            {
                new Trade
                {
                    Periods = new TradePeriod[] {
                        new TradePeriod{
                            Period = 0,
                            Volume = 100
                        }
                    }
                }
            };
    };
}
