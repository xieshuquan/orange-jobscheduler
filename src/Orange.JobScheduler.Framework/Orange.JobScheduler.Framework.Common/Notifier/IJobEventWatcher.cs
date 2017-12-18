using Orange.JobScheduler.Framework.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orange.JobScheduler.Job.Common.Notifier
{
    public interface IJobEventWatcher
    {
        void Do(JobEvent jobEvent);
    }
}
