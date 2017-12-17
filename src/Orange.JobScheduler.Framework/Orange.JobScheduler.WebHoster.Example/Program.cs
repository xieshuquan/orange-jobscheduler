using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orange.JobScheduler.WebHoster.Example.Handler;
using Orange.JobScheduler.WebHoster.OwinApplication;

namespace Orange.JobScheduler.WebHoster.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var hoster = new Hoster(9000, new Dictionary<string, IRequestHandler>() {{"task", new TaskHandler()}});
            hoster.Start();
            Console.ReadKey();
            hoster.Stop();
        }
    }
}
