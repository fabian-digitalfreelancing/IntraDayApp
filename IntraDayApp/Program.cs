using IntraDayApp;
using IntraDayApp.Remote;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

using IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Intra Day Report Service";
    })
    .ConfigureServices(services =>
    {
        LoggerProviderOptions.RegisterProviderOptions<
            EventLogSettings, EventLogLoggerProvider>(services);

        services.AddPowerServices();
        services.AddHostedService<WindowsBackgroundService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    })
    .ConfigureLogging((context, logging) =>
    {
        logging.AddConfiguration(
            context.Configuration.GetSection("Logging"));
    })
    .Build();

await host.RunAsync();
