
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
    public class EventRuleContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private NameTable soapNameTable;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private XmlNamespaceManager soapNamespaceManager;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private XmlDocument soapMessage;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public XmlDocument SoapMessage
        {
            get { return soapMessage; }
            set { soapMessage = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        private string label;
        
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Label
        {
            get { return label; }
            set { label = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public XmlNamespaceManager SoapNamespaceManager
        {
            get { return soapNamespaceManager; }
            set { soapNamespaceManager = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public NameTable SoapNameTable
        {
            get { return soapNameTable; }
            set { soapNameTable = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        private CanonicalEvent genericEvent;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private StringBuilder buffer = new StringBuilder();

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="genericEvent"></param>
        /// 
        public EventRuleContext(CanonicalEvent genericEvent)
        {
            if (genericEvent == null)
            {
                //throw new ArgumentNullException("genericEvent");
            }

            this.genericEvent = genericEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="data"></param>
        /// 
        public void Append(string data)
        {
            buffer.Append(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        public string Flush()
        {
            return buffer.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public CanonicalEvent GenericEvent
        {
            get { return genericEvent; }
        }
    }
}
