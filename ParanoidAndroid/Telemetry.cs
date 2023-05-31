using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParanoidAndroid
{
    public static class Telemtry
    {
        private static readonly TelemetryClient _client;

        static Telemtry()
        {
            TelemetryConfiguration config = TelemetryConfiguration.CreateDefault();
            config.InstrumentationKey = Environment.GetEnvironmentVariable("APPLICATIONINSIGHTS_CONNECTION_STRING").ToString();
            _client = new TelemetryClient(config);
        }

        public static TelemetryClient GetClient()
        {
            return _client;
        }
    }
}
