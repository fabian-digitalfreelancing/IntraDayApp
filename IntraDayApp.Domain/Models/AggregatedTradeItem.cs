namespace IntraDayApp.Domain.Models
{
    public class AggregatedTradeItem
    {
        public TimeOnly LocalTime { get; set; }
        public double Volume { get; set; }

        public override bool Equals(Object obj)
        {
            AggregatedTradeItem tradeItem = obj as AggregatedTradeItem;
            if (tradeItem == null)
                return false;
            else
                return this.LocalTime.Equals(tradeItem.LocalTime) && Volume.Equals(tradeItem.Volume);
        }
    }
}
