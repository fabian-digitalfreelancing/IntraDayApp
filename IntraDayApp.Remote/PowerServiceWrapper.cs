using IntraDayApp.Domain.Models;

namespace IntraDayApp.Remote
{
    public interface PowerServiceWrapper
    {
        Task<IEnumerable<Trade>> GetTradesAsync(DateTime date);
    }
}
