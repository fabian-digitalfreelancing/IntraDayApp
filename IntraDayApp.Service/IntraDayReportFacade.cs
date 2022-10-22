
namespace IntraDayApp.Service
{
    public interface IntraDayReportFacade
    {
        Task CreateCsvIntraDayReportAsync(string fileLocation);
    }
}
