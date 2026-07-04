using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.StockDeducRecords
{
    public class StockDeducRecordsController : DomainController
    {
        /// <summary>
        /// 扣料记录查询方法
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<StockDeducRecord> CriteriaStockDeducRecords(StockDeducRecordCriteria criteria)
        {
            var q = Query<StockDeducRecord>();
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.WorkOrder.No.Contains(criteria.WorkOrderNo));
            if (!criteria.TaskNo.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.No.Contains(criteria.TaskNo));
            if (!criteria.ProductCode.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.Product.Code.Contains(criteria.ProductCode));
            if (!criteria.ProductName.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.Product.Name.Contains(criteria.ProductName));
            if (!criteria.ShortDescription.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.Product.ShortDescription.Contains(criteria.ShortDescription));
            if (!criteria.BatchNo.IsNullOrEmpty())
                q.Where(p => p.BatchNo.Contains(criteria.BatchNo));
            if (!criteria.Resource.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.Resource.Name.Contains(criteria.Resource));
            if (!criteria.Process.IsNullOrEmpty())
                q.Where(p => p.DispatchTask.Process.Code.Contains(criteria.Process) || p.DispatchTask.Process.Name.Contains(criteria.Process));
            if (!criteria.ItemCode.IsNullOrEmpty())
                q.Where(p => p.Item.Code.Contains(criteria.ItemCode));
            if (!criteria.ItemName.IsNullOrEmpty())
                q.Where(p => p.Item.Name.Contains(criteria.ItemName));
            if (!criteria.ItemShortDescription.IsNullOrEmpty())
                q.Where(p => p.Item.ShortDescription.Contains(criteria.ItemShortDescription));
            if (criteria.CreateDate.BeginValue != null)
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            if (criteria.CreateDate.EndValue != null)
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return list;
        }
    }
}
