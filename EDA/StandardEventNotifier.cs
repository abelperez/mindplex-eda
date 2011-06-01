
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
    public class StandardEventNotifier : EventNotifier
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="source"></param>
        /// 
        public StandardEventNotifier(string source)
            : base(source)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="source"></param>
        /// <param name="workerCount"></param>
        /// 
        public StandardEventNotifier(string source, int workerCount)
            : base(source, workerCount)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="@event"></param>
        /// 
        public override void Notify(CanonicalEvent @event)
        {
            Console.WriteLine("firing event notification...");
        }
    }
}
