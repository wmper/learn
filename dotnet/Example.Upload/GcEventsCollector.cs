using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Metrics;
using Microsoft.Extensions.Options;
using Example.Upload.Model;

namespace Example.Upload
{
    public class GcEventsCollector : IHostedService, IDisposable
    {
        private readonly IOptions<CisSettings> _settings;
        private readonly TelemetryClient _telemetryClient;
        private Timer _timer;
        private Metric _gen0Collections;
        private Metric _gen1Collections;
        private Metric _gen2Collections;
        private Metric _totalMemory;
        private GcEventListener _gcTest;

        public GcEventsCollector(IOptions<CisSettings> settings, TelemetryClient telemetryClient)
        {
            _settings = settings;
            _telemetryClient = telemetryClient;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _gcTest?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_settings.Value.GcEventsCollector.Enabled)
            {
                const string MetricNamespace = "dotnet.gc";
                _gen0Collections = _telemetryClient.GetMetric(new MetricIdentifier(MetricNamespace, "Gen 0 Collections"));
                _gen1Collections = _telemetryClient.GetMetric(new MetricIdentifier(MetricNamespace, "Gen 1 Collections"));
                _gen2Collections = _telemetryClient.GetMetric(new MetricIdentifier(MetricNamespace, "Gen 2 Collections"));
                _totalMemory = _telemetryClient.GetMetric(new MetricIdentifier(MetricNamespace, "Total Memory"));

                _timer = new Timer(CollectData, null, 0, 5000);
                _gcTest = new GcEventListener(_telemetryClient, _settings.Value.GcEventsCollector.EnableAllocationEvents);
            }

            return Task.CompletedTask;
        }

        private void CollectData(object state)
        {
            _gen0Collections.TrackValue(GC.CollectionCount(0));
            _gen1Collections.TrackValue(GC.CollectionCount(1));
            _gen2Collections.TrackValue(GC.CollectionCount(2));
            _totalMemory.TrackValue(GC.GetTotalMemory(false));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }

        sealed class GcEventListener : EventListener
        {
            // from https://docs.microsoft.com/en-us/dotnet/framework/performance/garbage-collection-etw-events
            private const int GC_KEYWORD = 0x0000001;
            private readonly TelemetryClient _client;
            private readonly Metric _allocatedMemory;
            private readonly Metric _gen0Size;
            private readonly Metric _gen1Size;
            private readonly Metric _gen2Size;
            private readonly Metric _lohSize;
            private readonly Metric _gen0Promoted;
            private readonly Metric _gen1Promoted;
            private readonly Metric _gen2Survived;
            private readonly Metric _lohSurvived;
            private EventSource _dotNetRuntime;
            private readonly EventLevel _eventLevel;

            public GcEventListener(TelemetryClient client, bool enableAllocationEvents = false)
            {
                _client = client ?? throw new ArgumentNullException(nameof(client));

                const string MetricNamespace = "dotnet.gc";
                _gen0Size = _client.GetMetric(new MetricIdentifier(MetricNamespace, "Gen 0 Heap Size"));
                _gen1Size = _client.GetMetric(new MetricIdentifier(MetricNamespace, "Gen 1 Heap Size"));
                _gen2Size = _client.GetMetric(new MetricIdentifier(MetricNamespace, "Gen 2 Heap Size"));
                _lohSize = _client.GetMetric(new MetricIdentifier(MetricNamespace, "Large Object Heap Size"));
                _gen0Promoted = _client.GetMetric(new MetricIdentifier(MetricNamespace, "Bytes Promoted From Gen 0"));
                _gen1Promoted = _client.GetMetric(new MetricIdentifier(MetricNamespace, "Bytes Promoted From Gen 1"));
                _gen2Survived = _client.GetMetric(new MetricIdentifier(MetricNamespace, "Bytes Survived Gen 2"));
                _allocatedMemory = _client.GetMetric(new MetricIdentifier(MetricNamespace, "Allocated Memory", "Type"));
                _lohSurvived = _client.GetMetric(new MetricIdentifier(MetricNamespace, "Bytes Survived Large Object Heap"));

                _eventLevel = enableAllocationEvents ? EventLevel.Verbose : EventLevel.Informational;
            }

            protected override void OnEventSourceCreated(EventSource eventSource)
            {
                // look for .NET Garbage Collection events
                if (eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
                {
                    _dotNetRuntime = eventSource;
                    // EventLevel.Verbose enables the AllocationTick events, but also a heap of other stuff and will increase the memory allocation of your application since it's a lot of data to digest. EventLevel.Information is more light weight and is recommended if you don't need the allocation data.
                    EnableEvents(eventSource, EventLevel.Verbose, (EventKeywords)GC_KEYWORD);
                }
            }

            // from https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-2-2/
            // Called whenever an event is written.
            protected override void OnEventWritten(EventWrittenEventArgs eventData)
            {
                switch (eventData.EventName)
                {
                    case "GCHeapStats_V1":
                        ProcessHeapStats(eventData);
                        break;
                    case "GCAllocationTick_V3":
                        ProcessAllocationEvent(eventData);
                        break;
                }
            }

            private void ProcessAllocationEvent(EventWrittenEventArgs eventData)
            {
                _allocatedMemory.TrackValue((ulong)eventData.Payload[3], (string)eventData.Payload[5]);
            }

            private void ProcessHeapStats(EventWrittenEventArgs eventData)
            {
                _gen0Size.TrackValue((ulong)eventData.Payload[0]);
                _gen0Promoted.TrackValue((ulong)eventData.Payload[1]);
                _gen1Size.TrackValue((ulong)eventData.Payload[2]);
                _gen1Promoted.TrackValue((ulong)eventData.Payload[3]);
                _gen2Size.TrackValue((ulong)eventData.Payload[4]);
                _gen2Survived.TrackValue((ulong)eventData.Payload[5]);
                _lohSize.TrackValue((ulong)eventData.Payload[6]);
                _lohSurvived.TrackValue((ulong)eventData.Payload[7]);
            }
        }
    }
}
