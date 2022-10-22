using Microsoft.Extensions.DependencyInjection;

namespace IntraDayApp.Service
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<CsvCreator, CsvCreatorImpl>();
            services.AddSingleton<IntraDayReportFacade, IntraDayReportFacadeImpl>();
            services.AddSingleton<PowerServiceAggregator, PowerServiceAggregatorImpl>();
            services.AddSingleton<WorkerHelper, WorkerHelperImpl>();
            return services;
        }
    }
}
