using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Web.Data;

namespace SIE.Web.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 采购申请查询器
    /// </summary>
    public class PurRequireDataQueryer : DataQueryer
    {
        /// <summary>
        /// 创建一个新的采购申请
        /// </summary>
        /// <returns>新的采购申请</returns>
        public PurchaseRequisition GetNewPurchaseRequisition()
        {
            return RT.Service.Resolve<PurchaseRequisitionController>().GetNewPurchaseRequisition();
        }
    }
}
