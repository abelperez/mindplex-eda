
#region Imports

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using log4net;

using Mindplex.Core.Event;

#endregion

namespace Mindplex.Core.Event.Channel.Memory
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    /// <typeparam name="T"></typeparam>
    /// 
    public class MemoryEventChannel : EventResource, IEventChannel
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private Queue<EventItem<CanonicalEvent>> queue = new Queue<EventItem<CanonicalEvent>>();

        /// <summary>
        /// 
        /// </summary>
        /// 
        private string id; 

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="id"></param>
        /// 
        public MemoryEventChannel(string id)
        {
            this.id = id;
            if (Logger.IsDebugEnabled)
            {
                LogDebug("constructed with id: {0}.", id);
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
            EventItem<CanonicalEvent> item = new EventItem<CanonicalEvent>(@event);
            lock (queue)
            {
                if (Logger.IsDebugEnabled)
                {
                    LogDebug("channel size: {0}.", queue.Count);
                }
                
                queue.Enqueue(item);
                Monitor.Pulse(queue);

                // disable the following line to make this a blocking channel.
                // Monitor.Wait(queue);  
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        public CanonicalEvent Receive()
        {
            EventItem<CanonicalEvent> item;
            lock (queue)
            {
                while (queue.Count == 0)
                {
                    Monitor.Wait(queue);
                }
                item = queue.Dequeue();
                Monitor.Pulse(queue);
            }
            return item.item;
        }
    }
}