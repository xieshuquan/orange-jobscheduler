using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Microsoft.Owin.Hosting;
using Orange.JobScheduler.WebHoster.OwinApplication;
using Owin;

namespace Orange.JobScheduler.WebHoster
{
    public class Hoster
    {
        private int _port;
        private Dictionary<string, IRequestHandler> _routes;
        private ILog _logger;
        private IDisposable _app;

        public Hoster(int port, Dictionary<string, IRequestHandler> routes) : this(port, routes, null)
        {
        }

        public Hoster(int port, Dictionary<string, IRequestHandler> routes, ILog logger)
        {
            this._port = port;
            this._routes = routes;
            this._logger = logger;
        }

        public void Start()
        {
            _app = WebApp.Start($"http://*:{_port}/", app =>
            {
                foreach (var route in _routes)
                {
                    string path = route.Key;
                    if (!path.StartsWith("/"))
                    {
                        path = $"/{path}";
                    }
                    app.Map(path, privateApp =>
                    {
                        privateApp.Use(typeof(DefaultMiddleware), route.Value, null);
                    });
                }
                app.UseErrorPage();
            });
        }

        public void Stop()
        {
            _app?.Dispose();
        }
    }
}
