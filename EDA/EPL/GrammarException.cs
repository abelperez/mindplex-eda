
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
    public class GrammarException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private int line;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private int position;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="message"></param>
        /// 
        public GrammarException(string message, int line, int position)
            : base(message)
        {
            this.line = line;
            this.position = position;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public int Line
        {
            get { return line; }
            set { line = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public int Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}
