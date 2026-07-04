using SIE.Domain;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试工作计划保存后事件
    /// </summary>
    [System.ComponentModel.DisplayName("安装调试工作计划保存后事件")]
    [System.ComponentModel.Description("安装调试工作计划保存后事件")]
    public class EquipmentSetupPlanSubmmited : OnSubmitted<EquipmentSetupPlan>
    {
        /// <summary>
        /// 安装调试工作计划保存后事件
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">提交参数</param>
        protected override void Invoke(EquipmentSetupPlan entity, EntitySubmittedEventArgs e)
        {
            if (e != null && entity != null && e.Action == SubmitAction.Delete)
            {
                RT.Service.Resolve<EquipmentSetupController>().DeleteSetupAttachment(entity.EquipmentSetupId, entity.Id, null);
            }
        }
    }

    /// <summary>
    /// 安装调试设备明细保存后事件
    /// </summary>
    [System.ComponentModel.DisplayName("安装调试设备明细保存后事件")]
    [System.ComponentModel.Description("安装调试设备明细保存后事件")]
    public class EquipmentDetailSubmmited : OnSubmitted<EquipmentDetail>
    {
        /// <summary>
        /// 安装调试设备明细保存后事件
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">提交参数</param>
        protected override void Invoke(EquipmentDetail entity, EntitySubmittedEventArgs e)
        {
            if (e != null && entity != null && e.Action == SubmitAction.Delete)
            {
                RT.Service.Resolve<EquipmentSetupController>().DeleteSetupAttachment(entity.EquipmentSetupId, null, entity.EquipAccountId);
            }
        }
    }
}
