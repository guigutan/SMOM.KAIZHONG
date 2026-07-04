using SIE.Barcodes;
using SIE.Barcodes.Panels;
using SIE.Domain;
using System;

namespace SIE.MES.PanelBindings
{
    /// <summary>
    /// 条码控制器
    /// </summary>
    public partial class WoPanelRangeController : DomainController
    {
        /// <summary>
        /// 默认查询条码领用方法
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <returns>BarcodeRange列表</returns>
        public virtual EntityList<WoPanelRange> GetPanelRanges(WoPanelRangeCriteria criteria)
        {
            var q = Query<WoPanelRange>();
            if (criteria.WorkOrderId.HasValue)
                q.Where(p => p.WorkOrderId == criteria.WorkOrderId);
            if(criteria.WipResourceId.HasValue)
                q.Where(p => p.WorkOrder.ResourceId == criteria.WipResourceId);
            if (!criteria.PanelCode.IsNullOrEmpty())
            {
                q.Exists<Panel>((a, b) => b.Where(p => p.RangeId == a.Id && p.Code == criteria.PanelCode));
            }
            if (criteria.ReceiveById.HasValue)
                q.Where(p => p.ReceiveById == criteria.ReceiveById);
            if (criteria.ReceiveDate.BeginValue.HasValue)
                q.Where(p => p.ReceiveDate >= criteria.ReceiveDate.BeginValue);
            if (criteria.ReceiveDate.EndValue.HasValue)
                q.Where(p => p.ReceiveDate <= criteria.ReceiveDate.EndValue);
            return q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
