
#region Imports

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;

using log4net;

#endregion

namespace Mindplex.Core.Event.Rule
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class RuleReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private static ILog log = LogManager.GetLogger(typeof(RuleReader));

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="directory"></param>
        /// <param name="ruleName"></param>
        /// 
        /// <returns></returns>
        /// 
        public static List<RuleItem> Read(string directory, string ruleName)
        {
            List<RuleItem> result = new List<RuleItem>();

            foreach (string file in Directory.GetFiles(directory, "*.xml"))
            {
                XmlValidatingReader reader = null;
                try
                {
                    XmlDocument xml = new XmlDocument();
                    reader = new XmlValidatingReader(new XmlTextReader(file));
                    reader.ValidationType = ValidationType.None;
                    xml.Load(reader);
                    reader.Close();

                    XmlElement rootNode = (XmlElement) xml.SelectSingleNode("/applicationlogic");
                    XmlNodeList ruleNodes = rootNode.SelectNodes(ruleName);

                    // extract each rule.
                    foreach (XmlElement ruleSource in ruleNodes)
                    {
                        RuleItem rule = new RuleItem(ruleName, xml, ruleSource, directory, file);
                        result.Add(rule);
                    }
                    
                }
                catch (Exception exception)
                {
                    log.Warn(string.Format("Failed to read rules file: {0}.", file), exception);
                }
                finally
                {
                    if (reader != null && reader.ReadState != ReadState.Closed)
                    {
                        reader.Close();
                    }
                }
            }

            return result;
        }
    }
}