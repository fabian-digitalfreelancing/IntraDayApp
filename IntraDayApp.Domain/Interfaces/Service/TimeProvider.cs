namespace IntraDayApp.Domain.Interfaces.Service
{
    public interface TimeProvider
    {
        DateTime Now();
        DateTime TodaysDate();
    }
}
