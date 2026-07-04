using SIE.Equipments.Common.Controller;
using SIE.Equipments.EquipmentCards;
using SIE.Web.Data;

namespace SIE.Web.Equipments.Common.DataQuerys
{
    /// <summary>
    /// 数据查询帮助类
    /// </summary>
    public class EquipmentDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取是否启用审批流程
        /// </summary>
        /// <param name="typeQty">界面类型（1:设备立卡）</param>
        /// <returns>是否启用审批流程</returns>
        public bool GetEnableApproval(int typeQty)
        {
            return RT.Service.Resolve<ApprovalController>().GetApprovalConfigValue(typeof(EquipmentCard)).EnableAudit;
        }
    }
}
