
#region Imports

using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Text;

#endregion

namespace Mindplex.Core.Event.Channel.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public abstract class EventStreamProvider : ProviderBase, IEventStreamProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        public EventStreamProvider()
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// 
        /// <returns></returns>
        /// 
        public abstract IEventOutputStream GetEventOutputStream(string destination);
        
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="source"></param>
        /// 
        /// <returns></returns>
        /// 
        public abstract IEventInputStream GetEventInputStream(string source);       
    }
}
