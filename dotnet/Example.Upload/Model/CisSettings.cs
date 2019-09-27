using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Upload.Model
{
    public class CisSettings
    {
        public GcEventsCollectorModel GcEventsCollector { get; set; }
        public SystemUsageCollectorModel SystemUsageCollector { get; set; }
    }

    public class SystemUsageCollectorModel
    {
        public bool Enabled { get; set; } = true;
    }

    public class GcEventsCollectorModel
    {
        public bool Enabled { get; set; } = true;
        public bool EnableAllocationEvents { get; set; } = true;
    }
}
