using IntraDayApp.Domain.Responses;

namespace IntraDayApp.Domain.Interfaces.Remote
{
    public interface PowerServiceWrapper
    {
        Task<PowerServiceGetTradesResponse> GetTradesAsync(DateTime date);
    }
}
