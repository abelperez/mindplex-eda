
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Mindplex.Core.Event.Test
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class StandardSimulator
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        public static void Start()
        {
            IEventSource source = new StandardEventSource("specialfinance.event.source");
            source.Start();

            IEventProcessor processor = new StandardEventProcessor("specialfinance.event.source");
            processor.Start();

            IEventNotifier notifier = new StandardEventNotifier("specialfinance.event.destination");
            notifier.Start();
        }
    }
}
