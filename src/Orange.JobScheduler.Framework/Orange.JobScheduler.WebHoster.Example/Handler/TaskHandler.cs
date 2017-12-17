using Orange.JobScheduler.WebHoster.OwinApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orange.JobScheduler.WebHoster.Example.Handler
{
    public class TaskHandler:IRequestHandler
    {
        public HandleResult HandleRequest(BaseRequest request)
        {
            if (request.Uri.AbsolutePath.StartsWith("/task/200"))
            {
                return new HandleResult() {Code = "1", Data = 200};
            }
            else
            {
                return new HandleResult() {Code = "0"};
            }
        }
    }
}
