using System;
using System.Collections.Generic;
using System.Text;

namespace Example.ML
{
    public class SSQ
    {
        public int Type { get; set; }
        public DateTime DateTime { get; set; }
        public int No { get; set; }
        public int H1 { get; set; }
        public int H2 { get; set; }
        public int H3 { get; set; }
        public int H4 { get; set; }
        public int H5 { get; set; }
        public int H6 { get; set; }
        public int Total { get; set; }
        public int L1 { get; set; }
        public int L2 { get; set; }
        public string Data { get; set; }
        public decimal TotalMoney { get; set; }
        public int F1 { get; set; }
        public decimal FMoney { get; set; }
        public int S1 { get; set; }
        public decimal SMoney { get; set; }
        public decimal PayMoney { get; set; }

    }

    public class SSQDto
    {
        public int H1 { get; set; }
        public int H2 { get; set; }
    }
}
