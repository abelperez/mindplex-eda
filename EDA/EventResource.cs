
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

using log4net;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class EventResource
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private ILog logger;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private object syncRoot = new object();

        /// <summary>
        /// 
        /// </summary>
        /// 
        protected ILog Logger
        {
            get
            {
                if (logger == null)
                {
                    lock (syncRoot)
                    {
                        if (logger == null)
                        {
                            logger = LogManager.GetLogger(GetType());
                        }
                    }
                }

                return logger;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="message"></param>
        /// 
        protected void LogDebug(string message, params object[] args)
        {
            Logger.Debug(string.Format(message, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="message"></param>
        /// 
        protected void LogInfo(string message, params object[] args)
        {
            Logger.Info(string.Format(message, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="message"></param>
        /// 
        protected void LogWarn(string message, params object[] args)
        {
            Logger.Warn(string.Format(message, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="message"></param>
        /// 
        protected void LogError(string message, params object[] args)
        {
            Logger.Error(string.Format(message, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="exception"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// 
        protected void LogError(Exception exception, string message, params object[] args)
        {
            Logger.Error(string.Format(message, args), exception);
        }

    }
}
