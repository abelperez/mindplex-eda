
#region Imports

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Text;
using System.Web.Configuration;

using Mindplex.Core.Event.Channel.Configuration;

using log4net;

#endregion

namespace Mindplex.Core.Event.Channel
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class EventStreamService
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private static ILog logger = LogManager.GetLogger(typeof(EventStreamService));

        /// <summary>
        /// 
        /// </summary>
        /// 
        private EventStreamProvider defaultProvider;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private EventStreamProviderCollection providers;

        /// <summary>
        /// 
        /// </summary>
        /// 
        static EventStreamService()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        private EventStreamService()
        {
            Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        private void Initialize()
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("initializing event stream service...");
            }

            EventStreamProviderSection section =
                ConfigurationManager.GetSection(EventStreamProviderSection.ConfigurationSection) as EventStreamProviderSection;

            if (section == null)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(string.Format("could not find an event stream configuration sevction for: {0}.", EventStreamProviderSection.ConfigurationSection));
                }
                throw new ProviderException("Please define event stream configuration section.");
            }

            providers = new EventStreamProviderCollection();
            ProvidersHelper.InstantiateProviders(section.EventStreamProviders, providers, typeof(EventStreamProvider));
            providers.SetReadOnly();

            defaultProvider = providers[section.EventStreamProvider];
            if (defaultProvider == null)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error("could not find a definition for default event stream provider.");
                }
                throw new ProviderException("Please define a default event stream provider.");
            }
            else
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug(string.Format("default event stream provider: {0}", section.EventStreamProvider));
                }
            }

            if (providers != null)
            {
                foreach (IEventStreamProvider provider in providers)
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug(string.Format("configured provider: {0}", provider));
                    }
                }
            }

            if (logger.IsDebugEnabled)
            {
                logger.Debug("event stream service initialized.");
            }
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// 
        public static EventStreamService Instance
        {
            get
            {
                return EventStreamProviderManagerSingleton.soleInstance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        internal class EventStreamProviderManagerSingleton
        {
            /// <summary>
            /// 
            /// </summary>
            /// 
            static EventStreamProviderManagerSingleton()
            {
            }

            static internal readonly EventStreamService soleInstance = new EventStreamService();
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public EventStreamProvider DefaultProvider
        {
            get
            {
                return defaultProvider;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public EventStreamProviderCollection Providers
        {
            get
            {
                return providers;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="name"></param>
        /// 
        /// <returns></returns>
        /// 
        public EventStreamProvider GetProvider(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error("specified provider name is null or empty.");
                }
                throw new ArgumentException("Specified provider name is null.");
            }
            
            return Providers[name];
        }
    }
}