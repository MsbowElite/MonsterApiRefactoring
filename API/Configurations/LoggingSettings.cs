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
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        public LogGroupRetentionPolicy LogGroupRetentionPolicy =>
            Enum.TryParse(typeof(LogGroupRetentionPolicy), Retention, out object retentionTypeObject)
                    ? (LogGroupRetentionPolicy)retentionTypeObject
                    : LogGroupRetentionPolicy.Indefinitely;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}
