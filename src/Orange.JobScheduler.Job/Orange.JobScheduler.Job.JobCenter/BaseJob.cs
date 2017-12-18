using Common.Logging;
using Orange.JobScheduler.Framework.Common;
using Orange.JobScheduler.Framework.Common.Enums;
using Orange.JobScheduler.Framework.Common.Events;
using Orange.JobScheduler.Framework.Common.Notifier;
using Orange.JobScheduler.Job.JobCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orange.JobScheduler.Job.JobCenter
{
    public abstract class BaseJob
    {
        protected ILog _logger;
        public abstract JobExecuteResult Exec(JobContext context);

        public BaseJob(ILog logger)
        {
            this._logger = logger;
        }

        public void Run(JobContext context)
        {
            JobExecuteResult result = null;

            JobEventNotifier.Notify(new JobEvent()
            {
                JobId = context.JobTrigger.JobId,
                JobName = context.JobTrigger.JobName,
                EventType = JobEventType.BeginExecute
            });

            try
            {
                result = Exec(context);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                result = new JobExecuteResult()
                {
                    IsSuccess = false,
                    Message = $"{context.JobTrigger.ToString()} 执行发生异常",
                    Exception = ex
                };
            }

            JobEventNotifier.Notify(new JobEvent()
            {
                JobId = context.JobTrigger.JobId,
                JobName = context.JobTrigger.JobName,
                EventType = result.IsSuccess ? JobEventType.RunComplete :
                    JobEventType.RunError,
                Message = result.Message
            });
        }
    }
}
