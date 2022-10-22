namespace IntraDayApp.Domain.Models
{
    public class TradePeriod
    {
        public TradePeriod()
        {
        }

        public TradePeriod(int period, double volume)
        {
            Period = period;
            Volume = volume;
        }

        public int Period { get; set; }

        public double Volume { get; set; }
    }
}
