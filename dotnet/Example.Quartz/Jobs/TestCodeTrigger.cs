using Autofac;
using Quartz;
using SDK.Infrastructure.Interface;

namespace Example.Quartz.Jobs
{
    public class TestCodeTrigger //: IStartup
    {
        public void Initialize(ILifetimeScope container, params string[] assemblyStringPattens)
        {
            var scheduler = container.Resolve<IScheduler>();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<TestCodeJob>()
                .WithIdentity("job1", "group1")
                .Build();

            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            scheduler.ScheduleJob(job, trigger).GetAwaiter().GetResult();
        }
    }
}
