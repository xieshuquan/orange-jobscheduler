using Common.Logging;
using Orange.JobScheduler.Framework.Common;
using Orange.JobScheduler.Framework.Common.Enums;
using Orange.JobScheduler.Framework.Common.Events;
using Orange.JobScheduler.Framework.Common.Notifier;
using Orange.JobScheduler.Job.Common.Notifier;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orange.JobScheduler.Job.JobCenter
{
    public class JobExectuor : IJobEventWatcher
    {
        private readonly int _maxThreads;
        private readonly ConcurrentQueue<JobTrigger> _triggers;
        private readonly ConcurrentDictionary<int, JobThread> _runningJobs;
        private readonly ILog _logger;
        private readonly JobFinder _jobFinder;

        public JobExectuor(int maxThreads, ILog logger)
        {
            _maxThreads = maxThreads;
            _triggers = new ConcurrentQueue<JobTrigger>();
            _runningJobs = new ConcurrentDictionary<int, JobThread>();
            _logger = logger;
            _jobFinder = new JobFinder();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="jobname"></param>
        /// <returns></returns>
        public bool AddJobTrigger(JobTrigger trigger)
        {
            bool result = false;
            if (trigger != null)
            {
                _triggers.Enqueue(trigger);
                result = true;
                JobEventNotifier.Notify(new JobEvent()
                {
                    EventType = JobEventType.InQueue,
                    JobId = trigger.JobId,
                    JobName = trigger.JobName
                });
            }

            return result;
        }

        /// <summary>
        /// 执行job事件对应的操作
        /// </summary>
        /// <param name="jobEvent"></param>
        public void Do(JobEvent jobEvent)
        {
            switch (jobEvent.EventType)
            {
                case JobEventType.RunComplete:
                case JobEventType.RunError:
                    SetJobToComplete(jobEvent.JobId);
                    break;
            }
        }

        /// <summary>
        /// 循环执行job
        /// </summary>
        public void Run()
        {
            while (true)
            {
                while(_runningJobs.Count < _maxThreads && _triggers.Any())
                {
                    JobTrigger trigger = null;
                    if (_triggers.TryDequeue(out trigger))
                    {
                        var job = _jobFinder.FindJob(trigger.JobName);
                        if(job!= null)
                        {
                            var jobThread = new JobThread(trigger, job, _logger);
                            jobThread.Start();
                            _runningJobs[trigger.JobId] = jobThread;
                        }
                        else
                        {
                            _logger.Error($"无法定位到{trigger.ToString()}对应的job");
                        }                        
                    }
                }

                Thread.Sleep(50);
            }
        }

        public bool StopJob(JobTrigger trigger)
        {
            bool result = true;
            JobThread jobThread = null;
            
            if (_runningJobs.TryGetValue(trigger.JobId, out jobThread) && jobThread != null)
            {
                jobThread.Stop();
                JobEventNotifier.Notify(new JobEvent()
                {
                    EventType = JobEventType.Stopped,
                    JobId = trigger.JobId,
                    JobName = trigger.JobName
                });
                result = true;
            }
            else
            {
                JobEventNotifier.Notify(new JobEvent()
                {
                    EventType = JobEventType.StopFailed,
                    JobId = trigger.JobId,
                    JobName = trigger.JobName,
                    Message = "Job已经停止了"
                });
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 设置job为完成
        /// </summary>
        /// <param name="jobId"></param>
        public void SetJobToComplete(int jobId)
        {
            JobThread jobThread = null;
            if (!_runningJobs.TryRemove(jobId, out jobThread))
            {
                if (_runningJobs.ContainsKey(jobId))
                {
                    if (!_runningJobs.TryRemove(jobId, out jobThread))
                    {
                        _logger.Error("无法移除正在跑的job, 原因未知！");
                    }
                }
            }
        }

        public bool IsBusy()
        {
            return _runningJobs.Count >= _maxThreads;
        }
    }
}
