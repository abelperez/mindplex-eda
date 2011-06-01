
#region Imports

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Mindplex.Core.Event
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class Registry
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private static Hashtable registry = new Hashtable();

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="eventType"></param>
        /// 
        /// <returns></returns>
        /// 
        public static object Search(string name)
        {
            return registry[name];
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// 
        public static void Add(string name, object item)
        {
            registry.Add(name, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="name"></param>
        /// 
        /// <returns></returns>
        /// 
        public static bool Contains(string name)
        {
            return registry.ContainsKey(name);
        }
    }
}
