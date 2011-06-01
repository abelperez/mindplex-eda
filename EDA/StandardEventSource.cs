
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class StandardEventSource : EventSource
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// 
        public StandardEventSource(string destination)
            : base(destination)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// <param name="workerCount"></param>
        /// 
        public StandardEventSource(string destination, int workerCount)
            : base(destination, workerCount)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// <param name="workerCount"></param>
        /// <param name="executionCountPerSecond"></param>
        /// 
        public StandardEventSource(string destination, int workerCount, int executionCountPerSecond)
            : base(destination, workerCount, executionCountPerSecond)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public override void Listen()
        {
            CanonicalEvent @event = new CanonicalEvent();
            @event.EventType = "StandardEvent";
            @event.Payload = "<message description=\"standard event\" />";

            FireOnEventSourceReceievedEvent(@event);
            Deliver(@event);
        }     
    }
}
