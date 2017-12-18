using Common.Logging;
using Orange.JobScheduler.Framework.Common;
using Orange.JobScheduler.Framework.Common.Enums;
using Orange.JobScheduler.Framework.Common.Events;
using Orange.JobScheduler.Framework.Common.Notifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orange.JobScheduler.Job.JobCenter
{
    public class JobThread
    {
        private readonly Thread _thread;
        private readonly BaseJob _job;
        private readonly JobTrigger _trigger;
        private readonly ILog _logger;
        private readonly JobContext _jobContext;

        public ThreadState State => _thread.ThreadState;

        public JobThread(JobTrigger trigger, BaseJob job, ILog logger)
        {
            this._job = job;
            this._trigger = trigger;
            this._jobContext = new JobContext()
            {
                JobTrigger = trigger
            };
            this._thread = new Thread(() => job.Run(_jobContext));
            this._logger = logger;
        }

        public void Start()
        {
            _thread.Start();
            _logger.Info($"{this.ToString()} 线程启动成功");
        }

        public void Stop()
        {
            _thread.Abort();            
            _logger.Info($"{this.ToString()} 线程abort成功");
        }

        public override string ToString()
        {
            return $"jobId: {_trigger.JobId}, jobName:{_trigger.JobName}, thread:{base.ToString()}";
        }
    }
}
