
namespace IntraDayApp.Domain.Interfaces.Service
{
    public interface IntraDayReportFacade
    {
        Task CreateCsvIntraDayReportAsync(string fileLocation);
    }
}
