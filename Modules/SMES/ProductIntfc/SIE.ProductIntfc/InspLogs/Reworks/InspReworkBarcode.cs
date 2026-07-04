using SIE.Domain;
using SIE.MES.WorkOrders.Reworks;
using System;

namespace SIE.ProductIntfc.InspLogs.Reworks
{
    /// <summary>
    /// 返修条码获取类
    /// </summary>
    public partial class InspReworkBarcode : DomainController, IReworkBarcode
    {
        /// <summary>
        /// 获取返工关联条码列表
        /// </summary>
        /// <param name="criteria">返工关联条码查询实体</param>
        /// <returns>关联条码列表</returns>
        public EntityList GetUnionBarcodeViews(UnionBarcodeViewCriteria criteria)
        {
            var query = DB.Query<InspUnionBarcodeView>().Where(p => p.InvOrgId == RT.InvOrg);
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                query.Where(p => p.WorkOrderNo.Contains(criteria.WorkOrderNo));
            }
            if (!criteria.InspetNo.IsNullOrEmpty())
            {
                query.Where(p => p.InspetNo.Contains(criteria.InspetNo));
            }
            if (!criteria.Barcode.IsNullOrEmpty())
            {
                query.Where(p => p.Barcode.Contains(criteria.Barcode));
            }

            var result = query.Distinct().ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return result;
        }
    }
}