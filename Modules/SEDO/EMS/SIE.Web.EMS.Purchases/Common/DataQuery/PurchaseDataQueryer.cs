using SIE.EMS.Purchases.Common.Controller;
using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.EMS.Purchases.PaymentPlans;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.Web.Data;


namespace SIE.Web.EMS.Purchases.Common.DataQuery
{
    /// <summary>
    /// 采购模块查询器
    /// </summary>
    public class PurchaseDataQueryer : DataQueryer
    {
        /// <summary>
        /// 获取是否启用审批流程
        /// </summary>
        /// <param name="typeQty">界面类型（0：采购申请，1：采购订单，2：付款计划，3：设备开箱验收，4：备件验收,5:工治具验收,6:安装调试）</param>
        /// <returns>是否启用审批流程</returns>
        public bool GetEnableApproval(int typeQty)
        {
            var type = typeof(PurchaseRequisition);
            switch (typeQty)
            {
                case 1:
                    type = typeof(PurchaseOrder);
                    break;
                case 2:
                    type = typeof(PaymentPlan);
                    break;
                case 3:
                    type = typeof(EquipmentAcceptance);
                    break;
                case 4:
                    type = typeof(SparePartAcceptance);
                    break;
                case 5:
                    type = typeof(FixtureAcceptance);
                    break;
                case 6:
                    type = typeof(EquipmentSetup);
                    break;
                default:
                    break;
            }
            return RT.Service.Resolve<PurchasesApprovalController>().GetApprovalConfigValue(type).EnableAudit;
        }
    }
}