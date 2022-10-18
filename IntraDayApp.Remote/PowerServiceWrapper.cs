using IntraDayApp.Domain.Responses;

namespace IntraDayApp.Remote
{
    public interface PowerServiceWrapper
    {
        Task<PowerServiceGetTradesResponse> GetTradesAsync(DateTime date);
    }
}
