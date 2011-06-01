
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
    public interface IEventStreamProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// 
        /// <returns></returns>
        /// 
        IEventOutputStream GetEventOutputStream(string destination);

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="source"></param>
        /// 
        /// <returns></returns>
        /// 
        IEventInputStream GetEventInputStream(string source);
    }
}
