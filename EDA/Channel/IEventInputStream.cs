
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
    public interface IEventInputStream
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        CanonicalEvent Receive();
    }
}
