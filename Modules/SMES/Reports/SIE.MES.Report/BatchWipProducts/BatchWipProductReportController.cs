using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 批次产品版本控制器
    /// </summary>
    public class BatchWipProductReportController : DomainController
    {
        /// <summary>
        /// 获取批次通用报表集合
        /// </summary>
        /// <param name="query">标准查询实体</param>
        /// <returns>批次通用报表集合</returns>
        public virtual EntityList<BatchWipProductVersionReport> GetBatchWipProductReport(CriteriaQuery query)
        {
            return Query<BatchWipProductVersionReport>().Where(query.Criteria)
                .Join<Core.Items.ItemBatchRule>((x, y) => x.Product.Item.Id == y.ItemId && y.RetrospectType == Core.Items.RetrospectType.Batch)
                .ToList(query.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
