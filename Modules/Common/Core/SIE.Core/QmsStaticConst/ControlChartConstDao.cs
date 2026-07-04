using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Core.QmsStaticConst
{
    /// <summary>
    /// MSA常用参数 ControlChartConst DAO
    /// </summary>
    public class ControlChartConstDao : BaseDao<ControlChartConst>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msaConstId"></param>
        /// <returns></returns>
        public virtual EntityList<ControlChartConst> GetControlChartConsts(double msaConstId)
        {
            return Query().Where(c => c.MsaConstId == msaConstId).OrderBy(c => c.SampleQty).ToList(null,new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 稳定性导出时获取常用参数值
        /// </summary>
        /// <param name="msaPlanId"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual ControlChartConst GetConst(double constId, int sampleQty)
        {
            var query = Query().Where(c => c.MsaConstId == constId && c.SampleQty == sampleQty).FirstOrDefault();
            
            //var list = query.OrderBy(c => c.PlanTime).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            //var result = list.AsEntityList<MsaAnalysisBillBase>();
            //result.SetTotalCount(list.TotalCount);
            //return result;
            return query;
        }
    }
}
