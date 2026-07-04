using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Core.QmsStaticConst
{
    /// <summary>
    /// MSA常用参数 MsaConstK1 DAO
    /// </summary>
    public class StaticConstK1Dao : BaseDao<StaticConstK1>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msaConstId"></param>
        /// <returns></returns>
        public virtual EntityList<StaticConstK1> GetMsaConstK1s(double msaConstId)
        {
            return Query().Where(c => c.MsaConstId == msaConstId).OrderBy(c => c.TestQty).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
