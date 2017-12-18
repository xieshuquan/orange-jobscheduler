using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orange.JobScheduler.Job.JobCenter.Models
{
    public class JobExecuteResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
    }
}
