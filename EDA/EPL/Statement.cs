
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
    public class Statement
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        private string type;

        /// <summary>
        /// 
        /// </summary>
        /// 
        private Projection projection;
        
        /// <summary>
        /// 
        /// </summary>
        /// 
        private List<Predicate> predicates = new List<Predicate>();

        /// <summary>
        /// 
        /// </summary>
        /// 
        private Retention retention;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public Statement()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public Projection Projection
        {
            get { return projection; }
            set { projection = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public Retention Retention
        {
            get { return retention; }
            set { retention = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="predicate"></param>
        /// 
        public void AddPredicate(Predicate predicate)
        {
            predicates.Add(predicate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns></returns>
        /// 
        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
            b.Append("select ");
            b.Append(projection);
            b.Append(" from ");
            b.Append(type);
            if (predicates.Count > 0)
            {
                b.Append(" where ");
                foreach (Predicate p in predicates)
                {
                    b.Append(p);
                    b.Append(" and ");
                }
            }
            b.Append(retention);

            return b.ToString();
        }
    }
}
