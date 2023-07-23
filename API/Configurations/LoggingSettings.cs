using Serilog.Sinks.AwsCloudWatch;

namespace API.Configurations
{
    public class LoggingSettings
    {
        public string ApplicationName { get; set; } = default!;
        public string TeamsWebhookUrl { get; set; } = default!;
        public bool ShouldMonitorException { get; set; }
        public bool ShouldLogToCloudWatch { get; set; }
        public string Region { get; set; } = default!;
        public string Retention { get; set; } = default!;
    }
}
