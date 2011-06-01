
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Core;
using NUnit.Framework;

#endregion

namespace Mindplex.Core.Event.Channel.Messaging.Test
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [TestFixture]
    public class MessagingEventStreamProviderTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Test]
        public void TestSend()
        {
            MessagingEventStreamProvider provider = new MessagingEventStreamProvider();
            provider.GetEventOutputStream("eda.test.channel.send").Send(new CanonicalEvent());
        }
    }
}
