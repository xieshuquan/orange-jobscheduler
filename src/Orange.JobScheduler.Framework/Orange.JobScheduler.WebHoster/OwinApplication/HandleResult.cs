using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orange.JobScheduler.WebHoster.OwinApplication
{
    public class HandleResult
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public Object Data { get; set; }

        public Exception Exception { get; set; }
    }
}
