
#region Imports

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Mindplex.Core.Event.Rule
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class EventRuleRegistry
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="eventType"></param>
        /// <param name="eventRule"></param>
        /// 
        public static void AddRule(string eventType, IEventRule eventRule)
        {
            if (Registry.Contains(eventType))
            {
                ((List<IEventRule>)Registry.Search(eventType)).Add(eventRule);
            }
            else
            {
                List<IEventRule> target = new List<IEventRule>();
                target.Add(eventRule);
                Registry.Add(eventType, target);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="eventType"></param>
        /// 
        /// <returns></returns>
        /// 
        public static List<IEventRule> GetRules(string eventType)
        {
            return Registry.Search(eventType) as List<IEventRule>;
        }
    }
}