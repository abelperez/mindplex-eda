
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
    public class Retention
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private int count;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="count"></param>
        /// 
        public Retention(int count)
        {
            this.count = count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        public override string ToString()
        {
            return new StringBuilder().AppendFormat("retain {0} events", Count).ToString();
        }
    }
}
