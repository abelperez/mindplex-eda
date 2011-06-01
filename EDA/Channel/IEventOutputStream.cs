
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Mindplex.Core.Event.Channel
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public interface IEventOutputStream
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        void Send(CanonicalEvent @event);
    }
}
