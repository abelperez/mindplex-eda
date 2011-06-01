
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

using Mindplex.Core.Event.Channel;

using log4net;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class EventStreamFactory 
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private static ILog logger = LogManager.GetLogger(typeof(EventStreamFactory));

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="destination"></param>
        /// 
        /// <returns></returns>
        /// 
        public static IEventOutputStream GetEventOutputStream(string destination)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug(string.Format("getting event output stream for destiantion: {0}.", destination));
            }
            return EventStreamService.Instance.DefaultProvider.GetEventOutputStream(destination);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="source"></param>
        /// 
        /// <returns></returns>
        /// 
        public static IEventInputStream GetEventInputStream(string source)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug(string.Format("getting event input stream for source: {0}.", source));
            }
            return EventStreamService.Instance.DefaultProvider.GetEventInputStream(source);
        }
    }
}
