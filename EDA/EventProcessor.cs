
#region Imports

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Mindplex.Core.Event.Channel;
using Mindplex.Core.Event.EPL;
using Mindplex.Core.Event.Threads;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public abstract class EventProcessor : ExecutorService, IEventProcessor
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
        public event EventHandler OnEventProcessBegin;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public event EventHandler OnEventProcessEnd;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public event UnhandledExceptionEventHandler OnInternalEventProcessException;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="source"></param>
        /// 
        public EventProcessor(string source)
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
        public EventProcessor(string source, int workerCount)
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
                    FireOnEventProcessBeginEvent(@event);
                    Process(@event);
                    FireOnEventProcessEndEvent(@event);
                 }
            }
            catch (Exception exception)
            {
                if (Logger.IsErrorEnabled)
                {
                    LogError("failed to execute event processor.", exception);
                }
                FireOnInternalEventProcessExceptionEvent(exception);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        protected void FireOnEventProcessBeginEvent(CanonicalEvent @event)
        {
            if (OnEventProcessBegin != null)
            {
                OnEventProcessBegin.Invoke(this, new CanonicalEventArgs(@event));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        protected void FireOnEventProcessEndEvent(CanonicalEvent @event)
        {
            if (OnEventProcessEnd != null)
            {
                OnEventProcessEnd.Invoke(this, new CanonicalEventArgs(@event));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="exception"></param>
        /// 
        protected void FireOnInternalEventProcessExceptionEvent(Exception exception)
        {
            if (OnInternalEventProcessException != null)
            {
                UnhandledExceptionEventArgs e = new UnhandledExceptionEventArgs(exception, false);
                OnInternalEventProcessException.Invoke(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="query"></param>
        /// 
        /// <returns></returns>
        /// 
        public Statement CreateStatement(string query)
        {
            Grammar grammar = new Grammar(new Scanner(query));
            Statement epl = grammar.Parse();
            return epl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        public abstract void Process(CanonicalEvent @event);
    }
}
