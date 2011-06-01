
#region Imports

using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Core;
using NUnit.Framework;

using Mindplex.Core.Event.Rule;

#endregion

namespace Mindplex.Core.Event.Test
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [TestFixture]
    public class RulesEngineTest
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Test]
        public void SaverRuleTest()
        {
            List<RuleItem> rules = RuleReader.Read("C:\\Rules", "save");
            foreach (RuleItem rule in rules) 
            {
                if (rule.Name.Equals("save"))
                {
                    Console.WriteLine(rule);
                    SaverRule saver = new SaverRule(rule.Rule);
                    saver.Invoke(Simulator.SoapMessage, "EventSaver");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        [Test]
        public void StandardSimulatorTest()
        {
            StandardSimulator.Start();
        }

    }
}
