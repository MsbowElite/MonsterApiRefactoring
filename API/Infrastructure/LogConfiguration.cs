using Microsoft.Extensions.Options;
using Serilog;

namespace API.Infrastructure
{
    public static class LogConfiguration
    {
        public static void ConfigureLogging(this IConfiguration configuration, IServiceCollection services)
        {
            var loggingSection = configuration.GetSection(nameof(LoggingSettings));
            var loggingSettings = new LoggingSettings();
            new ConfigureFromConfigurationOptions<LoggingSettings>(loggingSection)
                .Configure(loggingSettings);

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(loggingSection);
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
        }

        public static void ConfigureSerilog(this IConfiguration configuration)
        {
            var loggingSection = configuration.GetSection(nameof(LoggingSettings));
            LoggingSettings loggingSettings = new();
            new ConfigureFromConfigurationOptions<LoggingSettings>(loggingSection)
                .Configure(loggingSettings);

            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(loggingSection)
                .Enrich.FromLogContext()
                .WriteTo.Console();

            Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}
