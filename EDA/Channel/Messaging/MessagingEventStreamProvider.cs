
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

using Mindplex.Core.Event.Channel.Configuration;

#endregion

namespace Mindplex.Core.Event.Channel.Messaging
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class MessagingEventStreamProvider : EventStreamProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// 
        /// <returns></returns>
        /// 
        public override IEventOutputStream GetEventOutputStream(string destination)
        {
            return new MessageSenderEventStream(destination);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="source"></param>
        /// 
        /// <returns></returns>
        /// 
        public override IEventInputStream GetEventInputStream(string source)
        {
            return new MessageReceiverEventStream(source);
        }
    }
}
