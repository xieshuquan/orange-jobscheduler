using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Orange.JobScheduler.WebHoster.OwinApplication
{
    public class BaseRequest
    {
        private readonly IOwinContext _context;

        public BaseRequest(IOwinContext context)
        {
            _context = context;
        }

        public Uri Uri => _context.Request.Uri;

        public IReadableStringCollection Query => _context.Request.Query;

        public Task<IFormCollection> Forms => _context.Request.ReadFormAsync();
    }
}
