
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
    public class Grammar
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private int section = 0;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private Scanner scanner;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private GrammarToken token;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="scanner"></param>
        /// 
        public Grammar(Scanner scanner)
        {
            if (scanner == null)
            {
                throw new ArgumentNullException("scanner");
            }

            this.scanner = scanner;
        }

        #region Parse 

        /// <summary>
        /// 
        /// </summary>
        /// 
        public Statement Parse()
        {
            Statement result = new Statement();
            Advance();

            while (token.Token != Token.Eof)
            {
                switch (token.Token)
                {
                    case Token.Select:
                        {
                            if (section == 1)
                            {
                                throw new GrammarException("incorrect syntax near the keyword 'select'.", token.Line, token.Position);
                            }

                            if (section == 0)
                            {
                                section = 1;
                            }

                            if (section != 1)
                            {
                                throw new GrammarException("incorrect syntax near the keyword 'select'.", token.Line, token.Position);
                            }

                            //Console.WriteLine("Processing SELECT");
                            Advance();

                            Projection projection = new Projection();

                            if (token.Token.Equals(Token.ALL))
                            {
                                Console.WriteLine("TOKEN SELECT " + token.Value);
                                projection.AddProperty(token.Value);
                                result.Projection = projection;
                                Advance();
                                break;
                            }

                            while (token.Token.Equals(Token.Comma) || token.Token.Equals(Token.Action))
                            {
                                Console.WriteLine("TOKEN SELECT " + token.Value);
                                projection.AddProperty(token.Value);
                                Advance();
                            }
                            result.Projection = projection;
                            break;
                        }
                    case Token.From:
                        {
                            if (section == 1)
                            {
                                section = 2;
                            }

                            if (section != 2)
                            {
                                throw new GrammarException("incorrect syntax near the keyword 'from'.", token.Line, token.Position);
                            }

                            //Console.WriteLine("processing FROM");
                            Advance();

                            if (token.Token.Equals(Token.Action))
                            {
                                Console.WriteLine("TOKEN FROM " + token.Value);
                                result.Type = token.Value;
                            }

                            Advance();
                            break;
                        }
                    case Token.Where:
                        {
                            if (section == 3)
                            {
                                throw new GrammarException("incorrect syntax near the keyword 'where'.", token.Line, token.Position);
                            }

                            if (section == 2)
                            {
                                section = 3;
                            }

                            if (section != 3)
                            {
                                throw new GrammarException("incorrect syntax near the keyword 'where'.", token.Line, token.Position);
                            }

                            //Console.WriteLine("processing WHERE");
                            Advance();

                            while (token.Token.Equals(Token.Action) ||
                                    token.Token.Equals(Token.And) ||
                                    token.Token.Equals(Token.Equal) ||
                                    token.Token.Equals(Token.Quote) ||
                                    token.Token.Equals(Token.Numeric))
                            {
                                Predicate predicate = new Predicate();

                                if (token.Token.Equals(Token.And))
                                {
                                    Console.WriteLine("TOKEN AND ");
                                    Advance();
                                }

                                if (token.Token.Equals(Token.Action))
                                {
                                    Console.WriteLine("TOKEN WHERE " + token.Value);
                                    predicate.Property = token.Value;
                                    Advance();

                                    if (token.Token.Equals(Token.Equal))
                                    {
                                        Console.WriteLine("TOKEN WHERE " + token.Value);
                                        predicate.Operator = token.Value;
                                        Advance();
                                    }

                                    if (token.Token.Equals(Token.Quote))
                                    {
                                        Console.WriteLine("TOKEN WHERE " + token.Value);
                                        Advance();
                                    }

                                    if (token.Token.Equals(Token.Action))
                                    {
                                        Console.WriteLine("TOKEN WHERE " + token.Value);
                                        predicate.Value = token.Value;
                                        Advance();
                                    }

                                    if (token.Token.Equals(Token.Quote))
                                    {
                                        Console.WriteLine("TOKEN WHERE " + token.Value);
                                        Advance();
                                    }

                                    if (token.Token.Equals(Token.Numeric))
                                    {
                                        string buffer = String.Empty;
                                        while (token.Token.Equals(Token.Numeric))
                                        {
                                            buffer += token.Value;
                                            Advance();
                                        }
                                        Console.WriteLine("TOKEN WHERE " + buffer);
                                        predicate.Value = buffer;
                                    }
                                }
                                result.AddPredicate(predicate);
                            }

                            break;
                        }
                    case Token.Retain:
                        {
                            if (section == 4)
                            {
                                throw new GrammarException("incorrect syntax near the keyword 'retain'.", token.Line, token.Position);
                            }

                            if (section == 3)
                            {
                                section = 4;
                            }

                            if (section != 4)
                            {
                                throw new GrammarException("incorrect syntax near the keyword 'retain'.", token.Line, token.Position);
                            }

                            Console.WriteLine("TOKEN RETAIN ");
                            Advance();

                            if (token.Token.Equals(Token.Numeric))
                            {
                                Console.WriteLine("TOKEN RETAIN " + token.Value);
                                try
                                {
                                    Retention retention = new Retention(Convert.ToInt32(token.Value));
                                    result.Retention = retention;
                                }
                                catch (Exception exception)
                                {
                                    throw new GrammarException("Invalid retention value.", token.Line, token.Position);
                                }
                                Advance();
                            }
                            else
                            {
                                throw new GrammarException("Invalid retention value.", token.Line, token.Position);
                            }

                            if (!token.Token.Equals(Token.Events))
                            {
                                throw new GrammarException("incorrect syntax near: " + token.Value, token.Line, token.Position);
                            }
                            Advance();
                                                        
                        }
                        break;
                    default:
                        throw new GrammarException("incorrect syntax.", token.Line, token.Position);
                }
            }

            return result;
        }

        #endregion

        #region Advance

        /// <summary>
        /// 
        /// </summary>
        /// 
        private void Advance()
        {
            token = scanner.Next();
        }

        #endregion
    }
}
