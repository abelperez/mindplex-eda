
#region Imports

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Mindplex.Core.Event.Channel.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class EventStreamProviderSection : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        public const string Providers = "providers";

        /// <summary>
        /// 
        /// </summary>
        /// 
        public const string DefaultProvider = "defaultProvider";

        /// <summary>
        /// 
        /// </summary>
        /// 
        public static readonly string ConfigurationSection = "eventStream";

        /// <summary>
        /// 
        /// </summary>
        /// 
        [ConfigurationProperty(EventStreamProviderSection.Providers)]
        public ProviderSettingsCollection EventStreamProviders
        {
            get 
            {
                return base[Providers] as ProviderSettingsCollection;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        [ConfigurationProperty(EventStreamProviderSection.DefaultProvider, DefaultValue = "MemoryEventStreamProvider")]
        public string EventStreamProvider
        {
            get
            {
                return base[DefaultProvider] as string;
            }
        }
    }
}
