using Common.Logging;
using Orange.JobScheduler.Framework.Common;
using Orange.JobScheduler.Framework.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orange.JobScheduler.Job.JobCenter
{
    public class JobCenter
    {
        private readonly JobExectuor _exectuor;
        private readonly ILog _logger;

        public JobCenter(int maxThreads, ILog logger)
        {
            _exectuor = new JobExectuor(maxThreads, logger);
            _logger = logger;
        }

        public HeartBeatStatus Heartbeat()
        {
            return _exectuor.IsBusy() ? HeartBeatStatus.Busy : HeartBeatStatus.Free;
        }

        public bool Trigger(JobTrigger trigger)
        {
            return _exectuor.AddJobTrigger(trigger);
        }

        public bool Stop(JobTrigger trigger)
        {
            return _exectuor.StopJob(trigger);
        }
    }
}
