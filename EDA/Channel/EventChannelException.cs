
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
    public class EventChannelException : Exception
    {
        public EventChannelException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="message"></param>
        /// 
        public EventChannelException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="message"></param>
        /// 
        public EventChannelException(string message, Exception exception)
            : base(message, exception)
        {
        }
    }
}
