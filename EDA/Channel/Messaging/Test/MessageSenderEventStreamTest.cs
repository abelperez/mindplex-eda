
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
    public class MessageSenderEventStreamTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Test]
        public void TestSend()
        {
            MessageSenderEventStream stream = new MessageSenderEventStream("eda.test.channel.send");
            stream.Send(new CanonicalEvent());
        }
    }
}
