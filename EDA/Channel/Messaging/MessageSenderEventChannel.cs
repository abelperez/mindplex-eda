
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

using Mindplex.Messaging;

#endregion

namespace Mindplex.Core.Event.Channel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class MessageSenderEventChannel : EventResource, IEventChannel
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
        private MessageSenderGateway sender;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// 
        public MessageSenderEventChannel(string destination)
        {
            if (String.IsNullOrEmpty(destination))
            {
                throw new EventChannelException(string.Format("Invalid destination: {0}", destination));    
            }

            this.destination = destination;
            sender = new MessageSenderGateway(destination, new Type[] { typeof(CanonicalEvent) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        public void Send(CanonicalEvent @event)
        {
            if (@event == null)
            {
                if (Logger.IsWarnEnabled)
                {
                    LogWarn("channel received null event.");
                }
            }

            sender.Send(@event);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        public CanonicalEvent Receive()
        {
            if (Logger.IsErrorEnabled)
            {
                LogError("sender channels do not receive events.");
            }

            throw new NotSupportedException("sender channels do not receive events.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Destination
        {
            get { return destination; }
        }
    }
}
