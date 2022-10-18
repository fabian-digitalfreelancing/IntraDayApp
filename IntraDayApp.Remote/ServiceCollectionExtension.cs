using Microsoft.Extensions.DependencyInjection;
using Services;

namespace IntraDayApp.Remote
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddPowerServices(this IServiceCollection services)
        {
            services.AddSingleton<IPowerService, PowerService>();
            services.AddSingleton<PowerServiceWrapper, PowerServiceWrapperImpl>();
            return services;
        }
    }
}
