using LaFlair.Core.IoC;
using Quartz;
using Quartz.Spi;

namespace LaFlair.Core.Quartz
{
    public class AutofacJobFactory : IJobFactory
    {
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var job = (IJob)ContainerFactory.Resolve(bundle.JobDetail.JobType);

            return job;
        }
    }
}