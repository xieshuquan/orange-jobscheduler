using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orange.JobScheduler.Framework.Common
{
    public class JobTrigger
    {
        public int JobId { get; set; }

        public string JobName { get; set; }

        public override string ToString()
        {
            return $"jobId:{JobId}, jobName:{JobName}";
        }
    }
}
