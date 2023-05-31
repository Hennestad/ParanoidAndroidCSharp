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
            config.InstrumentationKey = "InstrumentationKey=3a0beed7-cf40-45a8-963c-573d9c8aaaaa;IngestionEndpoint=https://norwayeast-0.in.applicationinsights.azure.com/;LiveEndpoint=https://norwayeast.livediagnostics.monitor.azure.com/";
            _client = new TelemetryClient(config);
        }

        public static TelemetryClient GetClient()
        {
            return _client;
        }
    }
}
