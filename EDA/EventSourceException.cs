
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
    public class EventSourceException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private string destination;
                
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="message"></param>
        /// 
        public EventSourceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="message"></param>
        /// <param name="destination"></param>
        /// 
        public EventSourceException(string message, string destination)
            : base(message)
        {
            this.destination = destination;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="message"></param>
        /// <param name="exception"></param>
        /// 
        public EventSourceException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="message"></param>
        /// <param name="destination"></param>
        /// <param name="exception"></param>
        /// 
        public EventSourceException(string message, string destination, Exception exception)
            : base(message, exception)
        {
            this.destination = destination;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }
    }
}
