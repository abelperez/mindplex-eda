
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

using Mindplex.Core.Event.Channel;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    ///
    /// </summary>
    /// 
    public interface IEventStream : IEventOutputStream, IEventInputStream
    {
    }
}
