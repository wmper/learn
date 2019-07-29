using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Config.Models
{
    public class ConfigValue
    {
        public string EnvironmentTypeId { get; set; }

        public string Key { get; set; }

        public string JsonValue { get; set; }
    }
}
