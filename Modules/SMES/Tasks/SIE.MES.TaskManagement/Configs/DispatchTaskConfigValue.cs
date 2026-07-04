using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Reports;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 派工任务单配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("派工任务单配置值")]
    public class DispatchTaskConfigValue : ConfigValue
    {
        #region 是否生成工单任务单 IsGenerate
        /// <summary>
        /// 是否生成工单任务单
        /// </summary>
        [Label("是否生成工单任务单")]
        public static readonly Property<bool> IsGenerateProperty = P<DispatchTaskConfigValue>.Register(e => e.IsGenerate);

        /// <summary>
        /// 是否生成工单任务单
        /// </summary>
        public bool IsGenerate
        {
            get { return this.GetProperty(IsGenerateProperty); }
            set { this.SetProperty(IsGenerateProperty, value); }
        }
        #endregion

        #region 任务单生成方式 GenerateMode
        /// <summary>
        /// 任务单生成方式
        /// </summary>
        [Label("任务单生成方式")]
        public static readonly Property<ReportMode> GenerateModeProperty = P<DispatchTaskConfigValue>.Register(e => e.GenerateMode);

        /// <summary>
        /// 任务单生成方式
        /// </summary>
        public ReportMode GenerateMode
        {
            get { return this.GetProperty(GenerateModeProperty); }
            set { this.SetProperty(GenerateModeProperty, value); }
        }
        #endregion

        #region 任务单号生成规则 NumberRule
        /// <summary>
        /// 任务单号生成规则Id
        /// </summary>
        [Label("任务单号生成规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
            P<DispatchTaskConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 任务单号生成规则Id
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 任务单号生成规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<DispatchTaskConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 任务单号生成规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 报工顺序 ReportOrder
        /// <summary>
        /// 报工顺序
        /// </summary>
        [Label("报工顺序")]
        public static readonly Property<ReportOrder?> ReportOrderProperty = P<DispatchTaskConfigValue>.Register(e => e.ReportOrder);

        /// <summary>
        /// 报工顺序
        /// </summary>
        public ReportOrder? ReportOrder
        {
            get { return this.GetProperty(ReportOrderProperty); }
            set { this.SetProperty(ReportOrderProperty, value); }
        }
        #endregion 

        #region 单据模板 PrintBillRule
        /// <summary>
        /// 单据模板ID
        /// </summary>
        [Label("单据模板")]
        public static readonly IRefIdProperty PrintBillRuleIdProperty =
              P<DispatchTaskConfigValue>.RegisterRefId(e => e.PrintBillRuleId, ReferenceType.Normal);

        /// <summary>
        /// 单据模板ID
        /// </summary>
        public double? PrintBillRuleId
        {
            get { return (double?)this.GetRefNullableId(PrintBillRuleIdProperty); }
            set { this.SetRefNullableId(PrintBillRuleIdProperty, value); }
        }

        /// <summary>
        /// 单据模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> DispatchTaskPrintBillRuleProperty =
            P<DispatchTaskConfigValue>.RegisterRef(e => e.PrintBillRule, PrintBillRuleIdProperty);

        /// <summary>
        /// 单据模板
        /// </summary>
        public PrintTemplate PrintBillRule
        {
            get { return this.GetRefEntity(DispatchTaskPrintBillRuleProperty); }
            set { this.SetRefEntity(DispatchTaskPrintBillRuleProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            return "是否生成工单任务单:{0} | 生成任务单方式：{1} | 任务单号生成规则：{2}  | 报工顺序：{3}".L10nFormat(IsGenerate, GenerateMode.ToLabel().L10N(), NumberRule?.Name, ReportOrder?.ToLabel().L10N());
        }
    }
}
