
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

using Mindplex.Core.Event.Channel.Configuration;

#endregion

namespace Mindplex.Core.Event.Channel.Memory
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class MemoryEventStreamProvider : EventStreamProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="name"></param>
        /// <param name="eventStream"></param>
        /// 
        public void AddEventStream(string name, IEventStream eventStream)
        {
            if (!Registry.Contains(name))
            {
                Registry.Add(name, eventStream);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="name"></param>
        /// 
        /// <returns></returns>
        /// 
        public IEventStream GetEventStream(string name)
        {
            IEventStream stream = Registry.Search(name) as MemoryEventStream;
            if (stream == null)
            {
                stream = new MemoryEventStream(name);
                AddEventStream(name, stream);
            }

            return stream;
        }

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
            return GetEventStream(destination);
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
            return GetEventStream(source);
        }
    }
}