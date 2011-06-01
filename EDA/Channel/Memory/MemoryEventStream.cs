
#region Imports

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

#endregion

namespace Mindplex.Core.Event.Channel.Memory
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class MemoryEventStream : IEventStream
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private MemoryEventChannel channel;

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
        public MemoryEventStream(string name)
        {
            this.name = name;
            channel = new MemoryEventChannel(name);
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
            CanonicalEvent @event = channel.Receive();
            return @event;
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
