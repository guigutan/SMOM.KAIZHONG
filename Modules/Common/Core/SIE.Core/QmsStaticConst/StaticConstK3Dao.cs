using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Core.QmsStaticConst
{
    /// <summary>
    /// MSA常用参数 MsaConstK3 DAO
    /// </summary>
    public class StaticConstK3Dao : BaseDao<StaticConstK3>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msaConstId"></param>
        /// <returns></returns>
        public virtual EntityList<StaticConstK3> GetMsaConstK3s(double msaConstId)
        {
            return Query().Where(c => c.MsaConstId == msaConstId).OrderBy(c => c.SampleQty).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
