using Orange.JobScheduler.Framework.Common.Events;
using Orange.JobScheduler.Job.Common.Notifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orange.JobScheduler.Framework.Common.Notifier
{
    public static class JobEventNotifier
    {
        private static readonly List<IJobEventWatcher> _watchers = new List<IJobEventWatcher>();

        public static void Notify(JobEvent jobEvent)
        {
            foreach (var watcher in _watchers)
            {
                watcher.Do(jobEvent);
            }
        }

        public static void Register(IJobEventWatcher watcher)
        {
            _watchers.Add(watcher);
        }
    }
}
