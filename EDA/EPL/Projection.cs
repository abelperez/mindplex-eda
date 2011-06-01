
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Mindplex.Core.Event.EPL
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class Projection
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private List<string> properties = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="property"></param>
        /// 
        public void AddProperty(string property)
        {
            properties.Add(property);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            
            foreach (string s in properties)
            {
                b.AppendFormat(" {0} ", s);
            }
            return b.ToString();
        }
    }
}
