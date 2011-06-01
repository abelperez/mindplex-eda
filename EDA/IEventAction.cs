
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
    public interface IEventAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        void Execute(CanonicalEvent @event);
    }
}
