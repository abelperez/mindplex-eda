
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
    public class CanonicalEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private CanonicalEvent canonicalEvent;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="canonicalEvent"></param>
        /// 
        public CanonicalEventArgs(CanonicalEvent canonicalEvent)
        {
            if (canonicalEvent == null)
            {
                throw new ArgumentNullException("canonicalEvent");
            }
            this.canonicalEvent = canonicalEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public CanonicalEvent CanonicalEvent
        {
            get { return canonicalEvent; }
            set { canonicalEvent = value; }
        }
    }
}
