
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Mindplex.Core.Event.Rule
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ListenerAttribute : Attribute
    {
        public string ToolName = null;
        public bool Contributes = true;
        
        public ListenerAttribute()
        {
        }

        public ListenerAttribute(string ToolName)
        {
            this.ToolName = ToolName;
        }

        public ListenerAttribute(bool Contributes)
        {
            this.Contributes = Contributes;
        }

        public ListenerAttribute(string ToolName, bool Contributes)
        {
            this.ToolName = ToolName;
            this.Contributes = Contributes;
        }

        public ListenerAttribute(bool Contributes, string ToolName)
        {
            this.ToolName = ToolName;
            this.Contributes = Contributes;
        }
    }
}
