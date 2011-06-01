
#region Imports

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

namespace Mindplex.Core.Event.EPL
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class Scanner
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private char next;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private int pos;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private int linenr;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private string line;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private StreamReader reader;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private StringBuilder builder;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="target"></param>
        /// 
        public Scanner(string target)
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(target));
            reader = new StreamReader(stream);
            builder = new StringBuilder();
            pos = 0;
            linenr = 0;
            line = "";
            Advance();
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        private void Advance()
        {
            if (pos + 1 < line.Length)
                next = line[++pos];
            else
            {
                if (reader.EndOfStream)
                    next = (char)0;
                else
                {
                    line = reader.ReadLine() + "\n";
                    linenr++;
                    pos = 0;
                    next = line[pos];
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        public GrammarToken Next()
        {
            switch (next)
            {
                case ' ':
                case '\t':
                case '\n':
                    Advance();
                    return Next();
                case '*':
                    Advance();
                    return new GrammarToken(Token.ALL, "*", linenr, pos);
                case '=':
                    Advance();
                    return new GrammarToken(Token.Equal, "=", linenr, pos);
                case ',':
                    Advance();
                    return new GrammarToken(Token.Comma, ",", linenr, pos);
                case '\'':
                    Advance();
                    return new GrammarToken(Token.Quote, "'", linenr, pos);
            }

            if (Char.IsDigit(next))
            {
                builder.Length = 0;
                builder.Append(next);
                Advance();
                return new GrammarToken(Token.Numeric, builder.ToString(), linenr, pos);
            }

            if (Char.IsLetter(next))
            {
                builder.Length = 0;
                while (Char.IsLetter(next))
                {
                    builder.Append(next);
                    Advance();
                }

                string keyword = builder.ToString();
                switch (keyword)
                {
                    case "select":
                        {
                            return new GrammarToken(Token.Select, "Select", linenr, pos);
                        }
                    case "from":
                        {
                            return new GrammarToken(Token.From, "From", linenr, pos);
                        }
                    case "where":
                        {
                            return new GrammarToken(Token.Where, "Where", linenr, pos);
                        }
                    case "and":
                        {
                            return new GrammarToken(Token.And, "And", linenr, pos);
                        }
                    case "retain":
                        {
                            return new GrammarToken(Token.Retain, "Retain", linenr, pos);
                        }
                    case "events":
                        {
                            return new GrammarToken(Token.Events, "Events", linenr, pos);
                        }
                    default:
                        return new GrammarToken(Token.Action, keyword, linenr, pos);
                }
            }
            return new GrammarToken(Token.Eof, null, linenr, pos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="backslash"></param>
        /// <param name="ch"></param>
        /// 
        /// <returns></returns>
        /// 
        private char Escape(bool backslash, char ch)
        {
            if (!backslash)
                return ch;
            else
                switch (ch)
                {
                    case 'a': return '\a';
                    case 'b': return '\b';
                    case 'f': return '\f';
                    case 'n': return '\n';
                    case 'r': return '\r';
                    case 't': return '\t';
                    case 'v': return '\v';
                    case '0': return '\0';
                    default:
                        throw new ApplicationException(string.Format("Unexpected escape character '\\{0}'", ch));
                        return ch;
                }
        }
    }
}
