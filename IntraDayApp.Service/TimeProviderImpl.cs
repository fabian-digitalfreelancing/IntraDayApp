using IntraDayApp.Domain.Interfaces.Service;

namespace IntraDayApp.Service
{
    public class TimeProviderImpl : TimeProvider
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
        public DateTime TodaysDate()
        {
            return DateTime.Today.Date;
        }
    }
}
