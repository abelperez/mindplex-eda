
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
    public class EventStreamProviderCollection : ProviderCollection
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="name"></param>
        /// 
        /// <returns></returns>
        /// 
        public new EventStreamProvider this[string name]
        {
            get
            {
                return base[name] as EventStreamProvider;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="provider"></param>
        /// 
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentException("Specified provider is null.");
            }

            if (!(provider is EventStreamProvider))
            {
                throw new ArgumentException("Specified provider is not of type EventStreamProvider.");
            }
            base.Add(provider);
        }
    }
}