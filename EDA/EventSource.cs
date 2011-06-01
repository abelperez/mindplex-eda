
#region Imports

using System;
using System.Threading;

using log4net;

using Mindplex.Core.Event.Channel;
using Mindplex.Core.Event.Threads;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public abstract class EventSource : ExecutorService, IEventSource
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private string destination;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private IEventOutputStream outputStream;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private volatile bool initialized = false;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private object syncRoot = new object();

        /// <summary>
        /// 
        /// </summary>
        /// 
        public event EventHandler OnEventSourceReceived;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public event EventHandler OnEventSourceDelivered;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public event UnhandledExceptionEventHandler OnInternalEventSourceException;
                
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// 
        public EventSource(string destination)
            : base(DefaultWorkerCount)
        {
            if (String.IsNullOrEmpty(destination))
            {
                ArgumentException exception = new ArgumentException("invalid destination specified.");
                if (Logger.IsErrorEnabled)
                {
                    LogError(exception, "invalid destination specified: {0}.", destination);
                }
                throw exception;
            }

            this.destination = destination;

            if (Logger.IsDebugEnabled)
            {
                LogDebug("constructed with destination: {0}.", destination);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// <param name="workerCount"></param>
        /// 
        public EventSource(string destination, int workerCount)
            : base(workerCount)
        {
            if (String.IsNullOrEmpty(destination))
            {
                ArgumentException exception = new ArgumentException("invalid destination specified.");
                if (Logger.IsErrorEnabled)
                {
                    LogError(exception, "invalid destination specified: {0}.", destination);
                }
                throw exception;
            }

            this.destination = destination;

            if (Logger.IsDebugEnabled)
            {
                LogDebug("constructed with destination: {0}.", destination);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// <param name="workerCount"></param>
        /// <param name="executionCountPerSecond"></param>
        /// 
        public EventSource(string destination, int workerCount, int executionCountPerSecond)
            : base(workerCount, executionCountPerSecond)
        {
            this.destination = destination;
            if (Logger.IsDebugEnabled)
            {
                LogDebug("constructed with destination: {0}", destination);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public override void Start()
        {
            if (String.IsNullOrEmpty(destination))
            {
                if (Logger.IsErrorEnabled)
                {
                    LogError("destination is invalid: {0}.", destination);
                }
                throw new EventSourceException(string.Format("destination is invalid: {0}.", destination));
            }

            lock (syncRoot)
            {
                if (outputStream == null)
                {
                    outputStream = EventStreamFactory.GetEventOutputStream(destination);
                    initialized = true;
                }
            }

            base.Start();
            if (Logger.IsDebugEnabled)
            {
                LogDebug("initialized with destination: {0}", destination);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Destination
        {
            get { return destination; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        protected virtual void Deliver(CanonicalEvent @event)
        {
            try
            {
                if (!initialized)
                {
                    if (Logger.IsErrorEnabled)
                    {
                        LogError("source has not initialized for destination: {0}.", destination);
                    }
                    throw new EventSourceException("source has not initialized for destination: {0}.", destination);
                }

                outputStream.Send(@event);
                FireOnEventSourceDeliveredEvent(@event);
            }
            catch(Exception exception)
            {
                if (Logger.IsErrorEnabled)
                {
                    LogError("failed to deliver event.", exception);
                }
                FireOnInternalEventSourceExceptionEvent(exception);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// 
        protected override void Execute()
        {
            try
            {
                while (Active)
                {
                    Throttle();
                    Listen();
                }
            }
            catch (Exception exception)
            {
                if (Logger.IsErrorEnabled)
                {
                    LogError("failed to execute event source.", exception);
                }
                FireOnInternalEventSourceExceptionEvent(exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        protected void FireOnEventSourceReceievedEvent(CanonicalEvent @event)
        {
            if (OnEventSourceReceived != null)
            {
                OnEventSourceReceived.Invoke(this, new CanonicalEventArgs(@event));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        protected void FireOnEventSourceDeliveredEvent(CanonicalEvent @event)
        {
            if (OnEventSourceDelivered != null)
            {
                OnEventSourceDelivered.Invoke(this, new CanonicalEventArgs(@event));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="exception"></param>
        /// 
        protected void FireOnInternalEventSourceExceptionEvent(Exception exception)
        {
            if (OnInternalEventSourceException != null)
            {
                UnhandledExceptionEventArgs e = new UnhandledExceptionEventArgs(exception, false);
                OnInternalEventSourceException.Invoke(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public abstract void Listen();
    }
}