using SIE.EMS.SpecialEquipment.RegularInspections;
using SIE.Equipments.Common.Controller;
using SIE.Web.Data;
using System;

namespace SIE.Web.EMS.SpecialEquipment.Common.DataQuery
{
    /// <summary>
    /// 审核帮助类
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
            Type type = typeof(RegularInspection);
            if (typeQty == 1)
            {
                type = typeof(RegularInspection);
            }
            return RT.Service.Resolve<ApprovalController>().GetApprovalConfigValue(type).EnableAudit;
        }
    }
}
