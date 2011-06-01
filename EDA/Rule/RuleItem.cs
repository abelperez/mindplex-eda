
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
    public class RuleItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private string name;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private XmlDocument source;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private XmlElement rule;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private string fileName;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private string directory;

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="name"></param>
        /// <param name="source"></param>
        /// <param name="rule"></param>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        /// 
        public RuleItem(string name, XmlDocument source, XmlElement rule, string directory, string fileName)
        {
            if (String.IsNullOrEmpty(name) || source == null || rule == null || String.IsNullOrEmpty(fileName) || String.IsNullOrEmpty(directory))
            {
                throw new ArgumentException(string.Format("Invalid arguments specified: name={0}, source={1}, rule={2}, filename={3}, directory={4}.", name, source, rule, fileName, directory));
            }

            this.name = name;
            this.source = source;
            this.rule = rule;
            this.directory = directory;
            this.fileName = fileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public XmlDocument Source
        {
            get { return source; }
            set { source = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public XmlElement Rule
        {
            get { return rule; }
            set { rule = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Directory
        {
            get { return directory; }
            set { directory = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string AbsoluteFileName
        {
            get 
            { 
                return new StringBuilder().Append(Directory).Append("\\").Append(FileName).ToString(); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        public override string ToString()
        {
            return new StringBuilder()
                    .AppendFormat("Name={0}, Source={1}, Rule={2}, FileName={3}, Directory={4}.",
                    Name, Source, Rule.OuterXml, FileName, Directory).ToString();
        }

    }
}
