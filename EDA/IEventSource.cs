
#region Imports

using System;

using Mindplex.Core.Event.Threads;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public interface IEventSource : ILifeCycle
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        void Listen();

        /// <summary>
        /// 
        /// </summary>
        /// 
        string Destination { get; }

        /// <summary>
        /// 
        /// </summary>
        /// 
        event EventHandler OnEventSourceReceived;

        /// <summary>
        /// 
        /// </summary>
        /// 
        event EventHandler OnEventSourceDelivered;

        /// <summary>
        /// 
        /// </summary>
        /// 
        event UnhandledExceptionEventHandler OnInternalEventSourceException;

    }
}
