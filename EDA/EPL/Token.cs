
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
    public enum Token
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        ALL, 
        Select, 
        From, 
        Where, 
        Equal, 
        Comma, 
        And, 
        Quote, 
        Numeric, 
        Retain, 
        Events,
        Eof, 
        Symbol, 
        Literal, 
        Action, 
        Divider, 
        Colon, 
        SemiColon, 
        EndOfSection, 
        Union, 
        Type,
        Token, 
        Left, 
        Right, 
        Prolog, 
        Epilog, 
        LeftCurly, 
        RightCurly, 
        Start,
        Namespace, 
        Using, 
        Visibility 
    };
}
