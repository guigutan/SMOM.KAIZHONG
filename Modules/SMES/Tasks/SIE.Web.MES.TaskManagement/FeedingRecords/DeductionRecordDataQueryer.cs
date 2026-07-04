using AngleSharp.Dom;
using SIE.Domain;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.FeedingRecords
{

    /// <summary>
    /// 
    /// </summary>
    public class DeductionRecordDataQueryer : DataQueryer
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public EntityList<DeductionRecord> GetDeductionRecordsByIds(List<double> ids)
        {
            var list = RT.Service.Resolve<FeedingRecordController>().GetDeductionRecordsByIds(ids);
            return list;
        }

        /// <summary>
        /// 根据报工记录获取扣料记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityList<DeductionRecord> GetDeductionRecordsByReportId(double id)
        {
            var list = RT.Service.Resolve<FeedingRecordController>().GetDeductionRecordsByReportId(id);
            return list;
        }
    }
}
