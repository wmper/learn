using Example.Upload.Model;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Upload
{
    public class SystemUsageCollector : IHostedService, IDisposable
    {
        private readonly IOptions<CisSettings> _settings;
        private readonly TelemetryClient _telemetryClient;
        private Timer _timer;
        private Metric _totalCpuUsed;
        private Metric _privilegedCpuUsed;
        private Metric _userCpuUsed;
        private Metric _workingSet;
        private Metric _nonPagedSystemMemory;
        private Metric _pagedMemory;
        private Metric _pagedSystemMemory;
        private Metric _privateMemory;
        private Metric _virtualMemoryMemory;
        private readonly Process _process = Process.GetCurrentProcess();
        private DateTime _lastTimeStamp;
        private TimeSpan _lastTotalProcessorTime = TimeSpan.Zero;
        private TimeSpan _lastUserProcessorTime = TimeSpan.Zero;
        private TimeSpan _lastPrivilegedProcessorTime = TimeSpan.Zero;

        public SystemUsageCollector(IOptions<CisSettings> settings, TelemetryClient telemetryClient)
        {
            _settings = settings;
            _telemetryClient = telemetryClient;
            _lastTimeStamp = _process.StartTime;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_settings.Value.SystemUsageCollector.Enabled)
            {
                _totalCpuUsed = _telemetryClient.GetMetric(new MetricIdentifier("system.cpu", "Total % Used"));
                _privilegedCpuUsed = _telemetryClient.GetMetric(new MetricIdentifier("system.cpu", "Privileged % Used"));
                _userCpuUsed = _telemetryClient.GetMetric(new MetricIdentifier("system.cpu", "User % Used"));
                _workingSet = _telemetryClient.GetMetric(new MetricIdentifier("system.memory", "Working Set"));
                _nonPagedSystemMemory = _telemetryClient.GetMetric(new MetricIdentifier("system.memory", "Non-Paged System Memory"));
                _pagedMemory = _telemetryClient.GetMetric(new MetricIdentifier("system.memory", "Paged Memory"));
                _pagedSystemMemory = _telemetryClient.GetMetric(new MetricIdentifier("system.memory", "System Memory"));
                _privateMemory = _telemetryClient.GetMetric(new MetricIdentifier("system.memory", "Private Memory"));
                _virtualMemoryMemory = _telemetryClient.GetMetric(new MetricIdentifier("system.memory", "Virtual Memory"));

                _timer = new Timer(CollectData, null, 1000, 5000);
            }

            return Task.CompletedTask;
        }

        private void CollectData(object state)
        {
            double totalCpuTimeUsed = _process.TotalProcessorTime.TotalMilliseconds - _lastTotalProcessorTime.TotalMilliseconds;
            double privilegedCpuTimeUsed = _process.PrivilegedProcessorTime.TotalMilliseconds - _lastPrivilegedProcessorTime.TotalMilliseconds;
            double userCpuTimeUsed = _process.UserProcessorTime.TotalMilliseconds - _lastUserProcessorTime.TotalMilliseconds;

            _lastTotalProcessorTime = _process.TotalProcessorTime;
            _lastPrivilegedProcessorTime = _process.PrivilegedProcessorTime;
            _lastUserProcessorTime = _process.UserProcessorTime;

            double cpuTimeElapsed = (DateTime.UtcNow - _lastTimeStamp).TotalMilliseconds * Environment.ProcessorCount;
            _lastTimeStamp = DateTime.UtcNow;

            _totalCpuUsed.TrackValue(totalCpuTimeUsed * 100 / cpuTimeElapsed);
            _privilegedCpuUsed.TrackValue(privilegedCpuTimeUsed * 100 / cpuTimeElapsed);
            _userCpuUsed.TrackValue(userCpuTimeUsed * 100 / cpuTimeElapsed);

            _workingSet.TrackValue(_process.WorkingSet64);
            _nonPagedSystemMemory.TrackValue(_process.NonpagedSystemMemorySize64);
            _pagedMemory.TrackValue(_process.PagedMemorySize64);
            _pagedSystemMemory.TrackValue(_process.PagedSystemMemorySize64);
            _privateMemory.TrackValue(_process.PrivateMemorySize64);
            _virtualMemoryMemory.TrackValue(_process.VirtualMemorySize64);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            return Task.CompletedTask;
        }
    }
}
