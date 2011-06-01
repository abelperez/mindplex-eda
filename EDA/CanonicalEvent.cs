
#region Imports

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [Serializable]
    public class CanonicalEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private string eventType;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private object payload;
        
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string EventType
        {
            get { return eventType; }
            set { eventType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public object Payload
        {
            get { return payload; }
            set { payload = value; }
        }
    }
}
