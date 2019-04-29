using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Example.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class TestJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("doing...");

            return Task.CompletedTask;
        }
    }
}
