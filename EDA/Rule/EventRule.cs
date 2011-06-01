
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Mindplex.Core.Event.Rule
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class EventRule : IEventRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private string eventType;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private List<EventAction> actions = new List<EventAction>();

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="eventAction"></param>
        /// 
        public void AddAction(EventAction eventAction)
        {
            actions.Add(eventAction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="eventAction"></param>
        /// 
        /// <returns></returns>
        /// 
        public bool RemoveAction(EventAction eventAction)
        {
            return actions.Remove(eventAction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public int ActionCount
        {
            get { return actions.Count; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        public void Invoke(CanonicalEvent @event)
        {
            foreach (EventAction action in actions)
            {
                action.Execute(@event);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string EventType
        {
            get { return eventType; }
            set { eventType = value; }
        }
    }
}
