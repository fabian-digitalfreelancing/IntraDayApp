using CsvHelper.Configuration.Attributes;

namespace IntraDayApp.Domain.Models
{
    public class AggregatedTradeItem
    {
        [Name("Local Time")]
        public TimeOnly LocalTime { get; set; }
        public double Volume { get; set; }

        public override bool Equals(Object obj)
        {
            AggregatedTradeItem tradeItem = obj as AggregatedTradeItem;
            if (tradeItem == null)
                return false;
            return this.LocalTime.Equals(tradeItem.LocalTime) && Volume.Equals(tradeItem.Volume);
        }
    }
}
