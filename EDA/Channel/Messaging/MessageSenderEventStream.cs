
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
    public class MessageSenderEventStream : EventResource, IEventStream
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
        public MessageSenderEventStream(string name)
        {
            this.name = name;
            channel = new MessageSenderEventChannel(name);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(string.Format("message sender event stream initialized for: {0}.", name));
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
            channel.Send(@event);
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
                LogError("sender event streams do not receive events.");
            }

            throw new NotSupportedException("sender event streams do not receive events.");
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