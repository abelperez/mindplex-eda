
#region Imports

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

#endregion

namespace Mindplex.Core.Event.Rule
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [Listener(ToolName="Blanker")]
    public class EmptyRule : EventLanguageCompiler
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="config"></param>
        /// 
        public EmptyRule(XmlElement config) 
            : base(config)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="context"></param>
        /// <param name="ActionNode"></param>
        /// <param name="Actions"></param>
        /// 
        /// <returns></returns>
        /// 
        public override System.Xml.XmlElement ProcessApplication(EventRuleContext context, XmlElement ActionNode, ref string Actions)
        {
            Console.WriteLine("processing blanker...");
            string name = GetParameter("name", ActionNode, context.SoapMessage, context.SoapNamespaceManager).Trim();
            Console.WriteLine("found name: {0}.", name);
            XmlElement result = context.SoapMessage.CreateElement("results");
            result.SetAttribute("name", name);
            return result;
        }
    }
}
