using Amazon.CloudWatchLogs;
using Microsoft.Extensions.Options;
using Serilog.Core;
using Serilog.Sinks.AwsCloudWatch;
using Serilog;
using Serilog.Formatting.Compact;
using Amazon;

namespace API.Configurations
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

            if (loggingSettings.ShouldLogToCloudWatch)
            {
                LoggingLevelSwitch levelSwitch = new();

                CloudWatchSinkOptions cloudWatchSinkOptions = new()
                {
                    LogGroupName = loggingSettings.ApplicationName,
                    TextFormatter = new CompactJsonFormatter(),
                    MinimumLogEventLevel = levelSwitch.MinimumLevel,
                    LogGroupRetentionPolicy = loggingSettings.LogGroupRetentionPolicy,
                    BatchSizeLimit = 100,
                    Period = TimeSpan.FromSeconds(10),
                    CreateLogGroup = true,
                    LogStreamNameProvider = new DefaultLogStreamProvider(),
                    RetryAttempts = 5
                };

                AmazonCloudWatchLogsClient amazonCloudWatchLogsClient = new(RegionEndpoint.GetBySystemName(loggingSettings.Region));

                loggerConfiguration
                    .WriteTo.Logger(loggerBuilder => loggerBuilder
                        .MinimumLevel.ControlledBy(levelSwitch)
                        .Enrich.With<LogEnrichmentFilter>()
                        .WriteTo.AmazonCloudWatch(cloudWatchSinkOptions, amazonCloudWatchLogsClient)
                    );
            }

            Log.Logger = loggerConfiguration.CreateLogger();
        }
    }
}
