
#region Imports

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Mindplex.Core.Event.Channel;

using log4net;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public abstract class EventAction : EventResource, IEventAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private IEventOutputStream outputStream;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private string destination;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private string actionType;
                
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// 
        public EventAction(string destination)
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
            outputStream = EventStreamFactory.GetEventOutputStream(destination);

            if (Logger.IsDebugEnabled)
            {
                LogDebug("constructed with destination: {0}.", destination);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        public abstract void Execute(CanonicalEvent @event);

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string ActionType
        {
            get { return actionType; }
            set { actionType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// <param name="destination"></param>
        /// 
        protected void Deliver(CanonicalEvent @event)
        {
            if (outputStream != null)
            {
                if (Logger.IsDebugEnabled)
                {
                    LogDebug("delivering event type: {0} to destination: {1}.", @event.EventType, destination);
                }
                outputStream.Send(@event);
            }
        }
    }
}