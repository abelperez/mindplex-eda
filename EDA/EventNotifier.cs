
#region Imports

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Mindplex.Core.Event.Channel;
using Mindplex.Core.Event.Threads;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public abstract class EventNotifier : ExecutorService, IEventNotifier
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private IEventInputStream inputStream;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private string source;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public event EventHandler OnEventNotificationReceived;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public event EventHandler OnEventNotificationDelivered;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public event UnhandledExceptionEventHandler OnInternalEventNotificationException;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="source"></param>
        /// 
        public EventNotifier(string source)
            : base()
        {
            if (String.IsNullOrEmpty(source))
            {
                ArgumentException exception = new ArgumentException("invalid source specified.");
                if (Logger.IsErrorEnabled)
                {
                    LogError(exception, "invalid source specified {0}.", source);
                }
                throw exception;
            }

            this.source = source;
            inputStream = EventStreamFactory.GetEventInputStream(source); 

            if (Logger.IsDebugEnabled)
            {
                LogDebug("constructed with source: {0}", source);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="source"></param>
        /// <param name="workerCount"></param>
        /// 
        public EventNotifier(string source, int workerCount)
            : base(workerCount)
        {
            if (String.IsNullOrEmpty(source))
            {
                ArgumentException exception = new ArgumentException("invalid source specified.");
                if (Logger.IsErrorEnabled)
                {
                    LogError(exception, "invalid source specified {0}.", source);
                }
                throw exception;
            }

            this.source = source;
            inputStream = EventStreamFactory.GetEventInputStream(source); 

            if (Logger.IsDebugEnabled)
            {
                LogDebug("constructed with source: {0}", source);
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
                    CanonicalEvent @event = inputStream.Receive();
                    FireOnEventNotificationReceievedEvent(@event);
                    Notify(@event);
                    FireOnEventNotificationDeliveredEvent(@event);
                }
            }
            catch (Exception exception)
            {
                FireOnInternalEventNotificationExceptionEvent(exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        protected void FireOnEventNotificationReceievedEvent(CanonicalEvent @event)
        {
            if (OnEventNotificationReceived != null)
            {
                OnEventNotificationReceived.Invoke(this, new CanonicalEventArgs(@event));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        protected void FireOnEventNotificationDeliveredEvent(CanonicalEvent @event)
        {
            if (OnEventNotificationDelivered != null)
            {
                OnEventNotificationDelivered.Invoke(this, new CanonicalEventArgs(@event));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="exception"></param>
        /// 
        protected void FireOnInternalEventNotificationExceptionEvent(Exception exception)
        {
            if (OnInternalEventNotificationException != null)
            {
                UnhandledExceptionEventArgs e = new UnhandledExceptionEventArgs(exception, false);
                OnInternalEventNotificationException.Invoke(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        public abstract void Notify(CanonicalEvent @event);
    }
}
