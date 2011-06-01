
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
    public class GrammarToken
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
        private Token token;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private string value;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="token"></param>
        /// <param name="value"></param>
        /// 
        public GrammarToken(Token token, string value)
        {
            this.token = token;
            this.value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="token"></param>
        /// <param name="value"></param>
        /// <param name="line"></param>
        /// <param name="position"></param>
        /// 
        public GrammarToken(Token token, string value, int line, int position)
        {
            this.token = token;
            this.value = value;
            this.line = line;
            this.position = position;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public Token Token
        {
            get { return token; }
            set { token = value; }
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

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        public override string ToString()
        {
            return new StringBuilder(string.Format("Token={0}, Value={1}, Line={2}, Position={3}.", token.ToString(), Value, Line, Position)).ToString();
        }
    }
}
