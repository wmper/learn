using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Quartz
{
    public class Properties
    {
        public Scheduler Scheduler { get; set; }
        public ThreaPpool ThreadPool { get; set; }
        public Jobstore JobStore { get; set; }
        public Plugin Plugin { get; set; }
    }

    public class Scheduler
    {
        public string InstanceName { get; set; } = "Quartz.Example";
    }

    public class ThreaPpool
    {
        public string Type { get; set; } = "Quartz.Simpl.SimpleThreadPool, Quartz";
        public int ThreadCount { get; set; } = 10;
    }

    public class Jobstore
    {
        public int MisfireThreshold { get; set; } = 6000;
    }

    public class Plugin
    {
        public Jobinitializer JobInitializer { get; set; }
    }

    public class Jobinitializer
    {
        public string Type { get; set; } = "Quartz.Plugin.Xml.XMLSchedulingDataProcessorPlugin, Quartz.Plugins";
        public string FileNames { get; set; } = "quartz_jobs.xml";
        public bool FailOnFileNotFound { get; set; } = true;
        public int ScanInterval { get; set; } = 120;
    }
}
