
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
    public interface IEventChannel
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        void Send(CanonicalEvent @event);

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        CanonicalEvent Receive();
    }
}
