using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    #region 工作计划
    /// <summary>
    /// 安装调试工作计划验证规则
    /// </summary>
    [DisplayName("安装调试工作计划验证规则")]
    [Description("安装调试工作计划验证规则")]
    public class EquipmentSetupPlanRule : EntityRule<EquipmentSetupPlan>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }
            var plan = entity as EquipmentSetupPlan;
            if (plan.PlanEndDateTime < plan.PlanStartDateTime)
            {
                e.BrokenDescription = "【计划结束时间】不能小于【计划开始时间】".L10N();
            }
        }
    }
    #endregion

    #region 设备明细
    /// <summary>
    /// 设备明细非重复验证规则
    /// </summary>
    [DisplayName("设备明细非重复验证规则")]
    [Description("设备明细非重复验证规则")]
    public class EquipmentDetailNotDuplicateRule : NotDuplicateRule<EquipmentDetail>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EquipmentDetailNotDuplicateRule()
        {
            Properties.Add(EquipmentDetail.EquipmentSetupIdProperty);
            Properties.Add(EquipmentDetail.EquipAccountIdProperty);
            MessageBuilder = (e) => { return "设备明细不能重复".L10N(); };
        }
    }
    #endregion

    #region 工时
    /// <summary>
    /// 安装调试工时登记验证规则
    /// </summary>
    [DisplayName("安装调试工时登记验证规则")]
    [Description("安装调试工时登记验证规则")]
    public class SetupWorkHourRule : EntityRule<SetupWorkHour>
    {
        /// <summary>
        /// 验证规则
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="e">参数</param>
        protected override void Validate(IEntity entity, RuleArgs e)
        {
            if (e == null)
            {
                return;
            }
            var workHour = entity as SetupWorkHour;
            if (workHour.EndDateTime < workHour.StartDateTime)
            {
                e.BrokenDescription = "【结束时间】不能小于【开始时间】".L10N();
            }
        }
    }
    #endregion
}
