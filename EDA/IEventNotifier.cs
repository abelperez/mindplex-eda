
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

using Mindplex.Core.Event.Threads;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public interface IEventNotifier : ILifeCycle
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        void Notify(CanonicalEvent @event);

        /// <summary>
        /// 
        /// </summary>
        /// 
        string Source { get; }

        /// <summary>
        /// 
        /// </summary>
        /// 
        event EventHandler OnEventNotificationReceived;

        /// <summary>
        /// 
        /// </summary>
        /// 
        event EventHandler OnEventNotificationDelivered;

        /// <summary>
        /// 
        /// </summary>
        /// 
        event UnhandledExceptionEventHandler OnInternalEventNotificationException;
    }
}
