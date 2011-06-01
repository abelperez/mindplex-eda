
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Mindplex.Core.Event.Channel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class MessageReceiverEventStream : EventResource, IEventStream
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private IEventChannel channel;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private string name;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="name"></param>
        /// 
        public MessageReceiverEventStream(string name)
        {
            this.name = name;
            channel = new MessageReceiverEventChannel(name);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(string.Format("message receiver event stream initialized for: {0}.", name));
            }
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
                LogError("receiver event streams do not send events.");
            }

            throw new NotSupportedException("receiver event streams do not send events.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        public CanonicalEvent Receive()
        {
            return channel.Receive();
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        public override string ToString()
        {
            return name;
        }
    }
}