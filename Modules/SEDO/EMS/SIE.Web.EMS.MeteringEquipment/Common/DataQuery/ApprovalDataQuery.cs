using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.Equipments.Common.Controller;
using SIE.Web.Data;
using System;

namespace SIE.Web.EMS.MeteringEquipment.Common.DataQuery
{
    /// <summary>
    /// 审批
    /// </summary>
    public class ApprovalDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取是否启用审批流程
        /// </summary>
        /// <param name="typeQty">界面类型（1：计量设备定检）</param>
        /// <returns>是否启用审批流程</returns>
        public bool GetEnableApproval(int typeQty)
        {
            Type type = typeof(Calibration);
            if (typeQty == 1)
            {
                type = typeof(Calibration);
            }
            return RT.Service.Resolve<ApprovalController>().GetApprovalConfigValue(type).EnableAudit;
        }
    }
}