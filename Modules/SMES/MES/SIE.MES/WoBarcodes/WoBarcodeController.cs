using SIE.Barcodes;
using SIE.Domain;
using System;

namespace SIE.MES.WoBarcodes
{
    /// <summary>
    /// 条码控制器
    /// </summary>
    public partial class WoBarcodeController : DomainController
    {
        /// <summary>
        /// 默认查询条码领用方法
        /// </summary>
        /// <param name="criteria">查询条件</param>
        /// <exception cref="ArgumentNullException">参数为空</exception>
        /// <returns>BarcodeRange列表</returns>
        public virtual EntityList<WoBarcodeRange> GetBarcodeRanges(WoBarcodeRangeCriteria criteria)
        {
            Check.NotNull(criteria, nameof(criteria));
            var q = Query<WoBarcodeRange>();
            if (criteria.Barcode.IsNotEmpty())
                q.Exists<Barcode>((x, y) => y.Where(e => e.Sn.Contains(criteria.Barcode) && e.RangeId == x.Id));
            if (criteria.ResourceId.HasValue)
                q.Where(p => p.WorkOrder.ResourceId == criteria.ResourceId);
            if (criteria.WorkOrderId.HasValue)
                q.Where(p => p.WorkOrderId == criteria.WorkOrderId);
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
