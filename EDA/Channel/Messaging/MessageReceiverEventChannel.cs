
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
    public class MessageReceiverEventChannel : EventResource, IEventChannel
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
        private MessageReceiverGateway receiver;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// 
        public MessageReceiverEventChannel(string destination)
        {
            if (String.IsNullOrEmpty(destination))
            {
                throw new EventChannelException(string.Format("Invalid destination: {0}", destination));    
            }

            this.destination = destination;
            receiver = new MessageReceiverGateway(destination, destination, new Type[] { typeof(CanonicalEvent) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        public void Send(CanonicalEvent @event)
        {
            if (Logger.IsErrorEnabled)
            {
                LogError("receiver channels do not send events.");
            }

            throw new NotSupportedException("receiver channels do not send events.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        public CanonicalEvent Receive()
        {
            Object result = receiver.ReceiveObject();
            if (result == null)
            {
                if (Logger.IsWarnEnabled)
                {
                    LogWarn("channel received null event.");
                }
            }

            return result as CanonicalEvent;
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
