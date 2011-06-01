
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
    class EventItem<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        internal T item;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="item"></param>
        /// 
        internal EventItem(T item)
        {
            this.item = item;
        }
    }
}