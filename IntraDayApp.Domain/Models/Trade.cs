namespace IntraDayApp.Domain.Models
{
    public class Trade
    {
        public DateTime Date { get; set; }

        public TradePeriod[] Periods { get; set; }
    }
}
