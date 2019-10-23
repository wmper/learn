using Quartz;
using System;
using System.Threading.Tasks;

namespace Example.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class TestCodeJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("coding...");

            return Task.CompletedTask;
        }
    }
}
