
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

using Mindplex.Core.Event.Rule;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class StandardEventProcessor : EventProcessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="source"></param>
        /// 
        public StandardEventProcessor(string source)
            : base(source)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="source"></param>
        /// <param name="workerCount"></param>
        /// 
        public StandardEventProcessor(string source, int workerCount)
            : base(source, workerCount)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        public override void Process(CanonicalEvent @event)
        {
            if (@event == null)
            {
                LogWarn("received null event!");
                return;
            }

            List<IEventRule> rules = EventRuleRegistry.GetRules(@event.EventType);
            if (rules == null)
            {
                return;
            }

            foreach (EventRule rule in rules)
            {
                rule.Invoke(@event);
            }
        }
    }
}
