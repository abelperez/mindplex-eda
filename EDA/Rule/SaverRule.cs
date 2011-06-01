
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
    [Listener(ToolName = "Saver")]
    public class SaverRule : EventLanguageCompiler
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="config"></param>
        /// 
        public SaverRule(XmlElement config) 
            : base(config) 
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="SoapNamespaceManager"></param>
        /// <param name="MessageTitle"></param>
        /// <param name="ActionNode"></param>
        /// <param name="Actions"></param>
        /// 
        /// <returns></returns>
        /// 
        public override XmlElement ProcessApplication(EventRuleContext context, XmlElement ActionNode, ref string Actions)
        {
            if (ActionNode.Name == "saveaction")
            {
                try
                {
                    string Filename = GetParameter("file", ActionNode, context.SoapMessage, context.SoapNamespaceManager).Trim();
                    string Data = GetParameter("binary", ActionNode, context.SoapMessage, context.SoapNamespaceManager);

                    if (Data == "")
                    {
                        Data = GetParameter("data", ActionNode, context.SoapMessage, context.SoapNamespaceManager);

                        string NewFolder = Filename.Substring(0, Filename.LastIndexOf("\\"));
                        System.IO.Directory.CreateDirectory(NewFolder);

                        System.IO.FileStream fs = new System.IO.FileStream(Filename, System.IO.FileMode.Create);
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);

                        try
                        {
                            sw.Write(Data);

                            Actions = "Saved data to " + Filename;
                        }
                        catch (Exception exx)
                        {
                            //ReportError(SoapMessage, exx);
                            Console.WriteLine(exx);
                        }
                        finally
                        {
                            sw.Close();
                        }
                    }
                    else
                    {
                        byte[] Binary = System.Convert.FromBase64String(Data);

                        System.IO.FileStream fs = new System.IO.FileStream(Filename, System.IO.FileMode.Create);

                        try
                        {
                            fs.Write(Binary, 0, Binary.Length);

                            Actions = "Saved data to " + Filename;
                        }
                        catch (Exception exx)
                        {
                            //ReportError(SoapMessage, exx);
                            Console.WriteLine(exx);
                        }
                        finally
                        {
                            fs.Close();
                        }
                    }
                }
                catch (_1800Communications.AggregationSystem._CoreLibrary.MessageQueueException exxx)
                {
                    throw exxx;
                }
                catch (Exception exx)
                {
                    //ReportError(SoapMessage, exx);
                    Console.WriteLine(exx);
                }
            }

            return null;
        }
    }
}
