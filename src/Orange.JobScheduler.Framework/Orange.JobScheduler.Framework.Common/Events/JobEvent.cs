using Orange.JobScheduler.Framework.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orange.JobScheduler.Framework.Common.Events
{
    public class JobEvent
    {
        public JobEventType EventType { get; set; }

        public int JobId { get; set; }

        public string JobName { get; set; }

        public string Message { get; set; }
    }

}
