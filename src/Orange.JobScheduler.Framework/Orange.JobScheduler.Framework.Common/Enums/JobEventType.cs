using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orange.JobScheduler.Framework.Common.Enums
{
    public enum JobEventType
    {
        PingSuccess,
        PingFailed,
        Dispatched,
        DispatchFailed,
        InQueue,
        Triggered,
        BeginExecute,
        RunComplete,
        RunError,
        NotExist,
        Stopped,
        StopFailed
    }
}
