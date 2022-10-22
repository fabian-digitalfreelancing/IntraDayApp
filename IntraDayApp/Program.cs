using IntraDayApp;
using IntraDayApp.Domain.AppSettings;
using IntraDayApp.Domain.Interfaces.App;
using IntraDayApp.Remote;
using IntraDayApp.Service;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using Serilog;

using IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Intra Day Report Service";
    })
    .ConfigureServices((hostContext, services) =>
    {
        LoggerProviderOptions.RegisterProviderOptions<
            EventLogSettings, EventLogLoggerProvider>(services);

        services.AddPowerServices();
        services.AddServices();
        services.AddSingleton<TimerWrapper, TimerWrapperImpl>();
        services.AddHostedService<WindowsBackgroundService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        IConfiguration configuration = hostContext.Configuration;
        services.Configure<ReportSettings>(configuration.GetSection(nameof(ReportSettings)));

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    })
    .UseSerilog()
    .Build();

await host.RunAsync();
