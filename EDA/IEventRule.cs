
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
    public interface IEventRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="eventAction"></param>
        /// 
        void AddAction(EventAction eventAction);

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="eventAction"></param>
        /// 
        /// <returns></returns>
        /// 
        bool RemoveAction(EventAction eventAction);

        /// <summary>
        /// 
        /// </summary>
        /// 
        int ActionCount { get; }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        void Invoke(CanonicalEvent @event);

        /// <summary>
        /// 
        /// </summary>
        /// 
        string EventType { get; set; }
 
    }
}
