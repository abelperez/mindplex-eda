
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
    public class Predicate
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private string property;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private string value;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private string oper;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public Predicate()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// 
        public Predicate(string property, string value, int oper)
        {
            this.property = property;
            this.value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Property
        {
            get { return property; }
            set { property = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Operator
        {
            get { return oper; }
            set { oper = value; }
        }

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            return b.AppendFormat("{0} {1} {2}", property, oper, value).ToString();
        }

    }
}
