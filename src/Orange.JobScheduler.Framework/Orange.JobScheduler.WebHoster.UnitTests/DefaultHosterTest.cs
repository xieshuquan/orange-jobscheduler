using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NSubstitute;
using Orange.JobScheduler.WebHoster.OwinApplication;

namespace Orange.JobScheduler.WebHoster.UnitTests
{
    [TestClass]
    public class DefaultHosterTest
    {
        private int port = 9000;
        private IRequestHandler taskHandler;

        private Hoster webHoster;

        [TestCleanup]
        public void TestCleanup()
        {
            taskHandler = Substitute.For<IRequestHandler>();
            taskHandler.HandleRequest(Arg.Is<BaseRequest>(t => t.Uri.AbsolutePath.StartsWith("/task")))
                .Returns(new HandleResult() { Code = "1" });
            taskHandler.HandleRequest(Arg.Is<BaseRequest>(t => !t.Uri.AbsolutePath.StartsWith("/task")))
                .Returns(new HandleResult() { Code = "0" });
            webHoster?.Stop();
        }

        [TestMethod]
        public void TestTask()
        {
            webHoster = new Hoster(port, new Dictionary<string, IRequestHandler>() {{"task", taskHandler}});
            Thread t = new Thread(webHoster.Start);
            t.Start();
        }
    }
}
