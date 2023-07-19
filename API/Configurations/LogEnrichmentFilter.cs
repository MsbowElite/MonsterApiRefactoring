using Serilog.Core;
using Serilog.Events;
using System.Text;

namespace API.Configurations
{
    public class LogEnrichmentFilter : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var propertyLevel = propertyFactory.CreateProperty("level", logEvent.Level);
            logEvent.AddOrUpdateProperty(propertyLevel);

            if (logEvent.Exception != null)
            {
                var propertyException = propertyFactory.CreateProperty("exception", GetException(logEvent.Exception));
                logEvent.AddOrUpdateProperty(propertyException);
            }

        }

        private string GetException(Exception exception)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append($"exception: {exception}");

            if (exception.InnerException != null)
            {
                stringBuilder.AppendLine($"exceptionDetails: {GetException(exception.InnerException)}");
            }

            return stringBuilder.ToString();
        }
    }
}
