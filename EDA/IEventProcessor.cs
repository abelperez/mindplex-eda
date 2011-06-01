
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

using Mindplex.Core.Event.EPL;
using Mindplex.Core.Event.Threads;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public delegate void OnPatternMatched();

    /// <summary>
    /// 
    /// </summary>
    /// 
    public interface IEventProcessor : ILifeCycle
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        void Process(CanonicalEvent @event);

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="query"></param>
        /// 
        /// <returns></returns>
        /// 
        Statement CreateStatement(string query);

        /// <summary>
        /// 
        /// </summary>
        /// 
        event EventHandler OnEventProcessBegin;

        /// <summary>
        /// 
        /// </summary>
        /// 
        event EventHandler OnEventProcessEnd;

        /// <summary>
        /// 
        /// </summary>
        /// 
        event UnhandledExceptionEventHandler OnInternalEventProcessException;
    }
}
