using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 批次生产通用报表查询数据提供者
    /// </summary>
    public class BatchReportCriteriaProvider : ICriteriaQueryProvider
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="query">标准查询实体</param>
        /// <returns>实体列表</returns>
        public EntityList GetList(CriteriaQuery query)
        {
            return RT.Service.Resolve<BatchWipProductReportController>().GetBatchWipProductReport(query);
        }
    }
}
