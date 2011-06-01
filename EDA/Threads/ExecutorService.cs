
#region Imports

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

#endregion

namespace Mindplex.Core.Event.Threads
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public abstract class ExecutorService : EventResource
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        public static readonly int DefaultWorkerCount = 1;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public static readonly int DefaultExecutionCountPerSecond = 500;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private int workerCount = DefaultWorkerCount;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private int executionCountPerSecond = DefaultExecutionCountPerSecond;

        /// 
        /// </summary>
        /// 
        private volatile bool throttleExecution = false;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private List<Thread> executors;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private bool active = false;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private object syncRoot = new object();
        
        /// <summary>
        /// 
        /// </summary>
        /// 
        public ExecutorService()
        {
            executors = new List<Thread>(workerCount);
            if (Logger.IsDebugEnabled)
            {
                LogDebug("constructed with default worker count: {0}.", workerCount);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="workerCount"></param>
        /// 
        public ExecutorService(int workerCount)
        {
            if (workerCount <= 0)
            {
                if (Logger.IsErrorEnabled)
                {
                    LogError("worker count cannot be less than or equal to zero: {0}.", workerCount);
                }
                throw new ArgumentException(string.Format("worker count cannot be less than or equal to zero: {0}.", workerCount));
            }

            this.workerCount = workerCount;
            executors = new List<Thread>(workerCount);

            if (Logger.IsDebugEnabled)
            {
                LogDebug("constructed with worker count: {0}.", workerCount);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="workerCount"></param>
        /// <param name="executionCountPerSecond"></param>
        /// 
        public ExecutorService(int workerCount, int executionCountPerSecond)
        {
            if (workerCount <= 0)
            {
                if (Logger.IsErrorEnabled)
                {
                    LogError("worker count cannot be less than or equal to zero: {0}.", workerCount);
                }
                throw new ArgumentException(string.Format("worker count cannot be less than or equal to zero: {0}.", workerCount));
            }

            if (executionCountPerSecond <= 0)
            {
                if (Logger.IsErrorEnabled)
                {
                    LogError("execution count per second cannot be less than or equal to zero: {0}.", executionCountPerSecond);
                }
                throw new ArgumentException(string.Format("execution count per second cannot be less than or equal to zero: {0}.", executionCountPerSecond));
            }

            this.workerCount = workerCount;
            if (executionCountPerSecond > 0 && executionCountPerSecond < 1000)
            {
                this.executionCountPerSecond = (1000 / executionCountPerSecond);
                throttleExecution = true;
            }

            executors = new List<Thread>(workerCount);

            if (Logger.IsDebugEnabled)
            {
                LogDebug("constructed with worker count: {0} and execution count per second: {1}.", workerCount, executionCountPerSecond);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public virtual void Start()
        {
            lock (syncRoot)
            {
                if (active == true)
                {
                    if (Logger.IsWarnEnabled)
                    {
                        LogWarn("already started.");
                    }
                    return;
                }
                active = true;

                for (int x = 0; x < workerCount; x++)
                {
                    ThreadStart workStarter = new ThreadStart(Execute);
                    Thread worker = new Thread(workStarter);
                    worker.Name = string.Format("{0}-{1}", GetType().Name, x);
                    if (Logger.IsDebugEnabled)
                    {
                        LogDebug("spawning worker thread: {0}.", worker.Name);
                    }
                    worker.Start();
                    executors.Add(worker);
                }
            }

            if (Logger.IsInfoEnabled)
            {
                LogInfo("successfuly started!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public virtual void Stop()
        {
            lock (syncRoot)
            {
                if (!active)
                {
                    if (Logger.IsWarnEnabled)
                    {
                        LogWarn("already stopped.");
                    }
                    return;
                }
                active = false;

                if (Logger.IsInfoEnabled)
                {
                    LogInfo("successfuly stopped!");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public void Throttle()
        {
            if (ThrottleExecution)
            {
                Thread.Sleep(ExecutionCountPerSecond);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public bool Active
        {
            get { return active; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public bool ThrottleExecution
        {
            get { return throttleExecution; }
            set { throttleExecution = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public int ExecutionCountPerSecond
        {
            get { return executionCountPerSecond; }
            set { executionCountPerSecond = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        protected abstract void Execute();
    }
}
