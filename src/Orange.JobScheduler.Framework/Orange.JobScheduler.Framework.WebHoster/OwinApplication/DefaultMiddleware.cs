using System;
using System.IO;
using System.Threading.Tasks;
using Common.Logging;
using Microsoft.Owin;
using Newtonsoft.Json;

namespace Orange.JobScheduler.Framework.WebHoster.OwinApplication
{
    public class DefaultMiddleware : OwinMiddleware
    {
        public IRequestHandler _handler;
        public ILog _logger;

        public DefaultMiddleware(OwinMiddleware next, IRequestHandler requestHandler, ILog logger) : base(next)
        {
            this._handler = requestHandler;
            this._logger = logger;
        }

        public override async Task Invoke(IOwinContext context)
        {
            HandleResult handleResult = new HandleResult();
            try
            {
                BaseRequest request = new BaseRequest(context);

                handleResult = _handler.HandleRequest(request);
            }
            catch (Exception ex)
            {
                handleResult = new HandleResult()
                {
                    Code = "webhoster.error.handlererror",
                    Message = $"handler发生异常, uri: {context.Request.Uri.AbsolutePath}",
                    Exception = ex
                };
                _logger?.Error($"request handler出错, context:{JsonConvert.SerializeObject(context)}", ex);
            }

            string result = JsonConvert.SerializeObject(handleResult);
            using (var writer = new StreamWriter(context.Response.Body))
            {
                await writer.WriteAsync(result);
            }
        }
    }
}
