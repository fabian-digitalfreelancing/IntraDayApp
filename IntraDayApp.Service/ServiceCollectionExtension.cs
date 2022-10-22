using IntraDayApp.Domain.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;

namespace IntraDayApp.Service
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ReportService, ReportServiceImpl>();
            services.AddSingleton<IntraDayReportFacade, IntraDayReportFacadeImpl>();
            services.AddSingleton<PowerServiceAggregator, PowerServiceAggregatorImpl>();
            services.AddSingleton<CsvServiceWrapper, CsvServiceWrapperImpl>();
            services.AddSingleton<TimeProvider, TimeProviderImpl>();

            return services;
        }
    }
}
