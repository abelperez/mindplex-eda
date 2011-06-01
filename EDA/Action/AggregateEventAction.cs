
#region Imports

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using log4net;

#endregion

namespace Mindplex.Core.Event.Action
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class AggregateEventAction : EventAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private static readonly string DataStore = "EventAggregatorDataStore";

        /// <summary>
        /// 
        /// </summary>
        /// 
        private int max;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// <param name="max"></param>
        /// 
        public AggregateEventAction(string destination, int max)
            : base(destination)
        {
            this.max = max;
            if (Logger.IsDebugEnabled)
            {
                LogDebug("constructed with buffer size: {0}.", max);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        public override void Execute(CanonicalEvent @event)
        {
            LocalDataStoreSlot slot = Thread.GetNamedDataSlot(DataStore);
            List<CanonicalEvent> events = GetEventAggregate(slot);

            if (events == null)
            {
                events = new List<CanonicalEvent>();
                events.Add(@event);
                SetEventAggregate(slot, events);

                if (Logger.IsDebugEnabled)
                {
                    LogDebug("initializing buffer size: {0}.", max);
                }
            }
            else if (events.Count <= max)
            {
                events.Add(@event);
                if (Logger.IsDebugEnabled)
                {
                    LogDebug("buffering event: {0}", events.Count);
                }
            }
            else
            {
                if (Logger.IsInfoEnabled)
                {
                    LogInfo("flushing {0} events from buffer...", events.Count);
                }
                List<CanonicalEvent> clone = new List<CanonicalEvent>(events);
                Deliver(ProcessAggregateResult(clone));
                events.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        protected List<CanonicalEvent> GetEventAggregate(LocalDataStoreSlot slot)
        {
            return Thread.GetData(slot) as List<CanonicalEvent>;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="slot"></param>
        /// <param name="buffer"></param>
        /// 
        protected void SetEventAggregate(LocalDataStoreSlot slot, List<CanonicalEvent> buffer)
        {
            Thread.SetData(slot, buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="events"></param>
        /// 
        /// <returns></returns>
        /// 
        protected virtual CanonicalEvent ProcessAggregateResult(List<CanonicalEvent> events)
        {
            CanonicalEvent result = new CanonicalEvent();
            result.Payload = events;
            result.EventType = Event.AggregateType;
            return result;
        }
    }
}