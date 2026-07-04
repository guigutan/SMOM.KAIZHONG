using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Core.QmsStaticConst
{
    /// <summary>
    /// MSA常用参数 MsaConstK2 DAO
    /// </summary>
    public class StaticConstK2Dao : BaseDao<StaticConstK2>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msaConstId"></param>
        /// <returns></returns>
        public virtual EntityList<StaticConstK2> GetMsaConstK2s(double msaConstId)
        {
            return Query().Where(c => c.MsaConstId == msaConstId).OrderBy(c => c.EvaluateQty).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
