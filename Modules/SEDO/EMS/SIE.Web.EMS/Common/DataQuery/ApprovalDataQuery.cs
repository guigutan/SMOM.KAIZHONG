using SIE.EMS.AssetIssues;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.AssetReturns;
using SIE.EMS.AssetTransfers;
using SIE.EMS.Common.Controller;
using SIE.EMS.FixedAssets.Accounts;
using SIE.EMS.InventoryPlans;
using SIE.EMS.Lubrications;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.ViceTransfers;
using SIE.Web.Data;
using System;

namespace SIE.Web.EMS.Common.DataQuery
{
    /// <summary>
    /// 审批按钮查询器
    /// </summary>
    public class ApprovalDataQuery : DataQueryer
    {

        /// <summary>
        /// 获取是否启用审批流程
        /// </summary>
        /// <param name="typeQty">界面类型（1:润滑记录 2:资源调拨 3:副资源调拨 4:固定资产台账 5:资产领用 6:资产发放 7:备件申请单）</param>
        /// <returns>是否启用审批流程</returns>
        public bool GetEnableApproval(int typeQty)
        {
            Type type;
            switch (typeQty)
            {
                case 1:
                    type = typeof(Lubrication);
                    break;
                case 2:
                    type = typeof(AssetTransfer);
                    break;
                case 3:
                    type = typeof(ViceTransfer);
                    break;
                case 4:
                    type = typeof(FixedAssetsAccount);
                    break;
                case 5:
                    type = typeof(AssetRequisition);
                    break;
                case 6:
                    type = typeof(AssetIssue);
                    break;
                case 7:
                    type = typeof(SparePartApp);
                    break;
                default:
                    type = typeof(Lubrication);
                    break;
            }
            return RT.Service.Resolve<EmsApprovalController>().GetApprovalConfigValue(type).EnableAudit;
        }
    }
}
