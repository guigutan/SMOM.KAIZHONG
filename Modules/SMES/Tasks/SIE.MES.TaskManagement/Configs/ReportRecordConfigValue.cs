using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.TaskManagement.Configs
{
    /// <summary>
    /// 报工记录配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("报工记录配置值")]
    public class ReportRecordConfigValue : ConfigValue 
    {
        #region 报工批次号编码规则 ReportBatchNoRule
        /// <summary>
        /// 报工批次号编码规则Id
        /// </summary>
        [Label("报工批次号编码规则")]
        public static readonly IRefIdProperty ReportBatchNoRuleIdProperty =
            P<ReportRecordConfigValue>.RegisterRefId(e => e.ReportBatchNoRuleId, ReferenceType.Normal);

        /// <summary>
        /// 报工批次号编码规则Id
        /// </summary>
        public double? ReportBatchNoRuleId
        {
            get { return (double?)this.GetRefNullableId(ReportBatchNoRuleIdProperty); }
            set { this.SetRefNullableId(ReportBatchNoRuleIdProperty, value); }
        }

        /// <summary>
        /// 报工批次号编码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> ReportBatchNoRuleProperty =
            P<ReportRecordConfigValue>.RegisterRef(e => e.ReportBatchNoRule, ReportBatchNoRuleIdProperty);

        /// <summary>
        /// 报工批次号编码规则
        /// </summary>
        public NumberRule ReportBatchNoRule
        {
            get { return this.GetRefEntity(ReportBatchNoRuleProperty); }
            set { this.SetRefEntity(ReportBatchNoRuleProperty, value); }
        }
        #endregion

        #region 报工打印模板 ReportPrintTemplate
        /// <summary>
        /// 报工打印模板Id
        /// </summary>
        [Label("报工打印模板")]
        public static readonly IRefIdProperty ReportPrintTemplateIdProperty =
            P<ReportRecordConfigValue>.RegisterRefId(e => e.ReportPrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 报工打印模板Id
        /// </summary>
        public double? ReportPrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(ReportPrintTemplateIdProperty); }
            set { this.SetRefNullableId(ReportPrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 报工打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> ReportPrintTemplateProperty =
            P<ReportRecordConfigValue>.RegisterRef(e => e.ReportPrintTemplate, ReportPrintTemplateIdProperty);

        /// <summary>
        /// 报工打印模板
        /// </summary>
        public PrintTemplate ReportPrintTemplate
        {
            get { return this.GetRefEntity(ReportPrintTemplateProperty); }
            set { this.SetRefEntity(ReportPrintTemplateProperty, value); }
        }
        #endregion

        #region 是否校验只允许一个任务报工 IsValidateReportSingleTask
        /// <summary>
        /// 是否校验只允许一个任务报工
        /// </summary>
        [Label("是否校验只允许一个任务报工")]
        public static readonly Property<bool> IsValidateReportSingleTaskProperty = P<ReportRecordConfigValue>.Register(e => e.IsValidateReportSingleTask);

        /// <summary>
        /// 是否校验只允许一个任务报工
        /// </summary>
        public bool IsValidateReportSingleTask
        {
            get { return this.GetProperty(IsValidateReportSingleTaskProperty); }
            set { this.SetProperty(IsValidateReportSingleTaskProperty, value); }
        }
        #endregion

        #region 允许工序多任务同时报工 AllowMultiTaskReportProcess
        /// <summary>
        /// 允许工序多任务同时报工
        /// </summary>
        [Label("允许工序多任务同时报工")]
        public static readonly Property<string> AllowMultiTaskReportProcessProperty = P<ReportRecordConfigValue>.Register(e => e.AllowMultiTaskReportProcess);

        /// <summary>
        /// 允许工序多任务同时报工
        /// </summary>
        public string AllowMultiTaskReportProcess
        {
            get { return this.GetProperty(AllowMultiTaskReportProcessProperty); }
            set { this.SetProperty(AllowMultiTaskReportProcessProperty, value); }
        }
        #endregion


        //#region 报工数量允许大于前工序报工数量（合格） ProcessReportingRelationship
        ///// <summary>
        ///// 报工数量允许大于前工序报工数量（合格）
        ///// </summary>
        //[Label("报工数量允许大于前工序报工数量（合格）")]
        //public static readonly Property<bool> ProcessReportingRelationshipProperty = P<ReportRecordConfigValue>.Register(e => e.ProcessReportingRelationship);

        ///// <summary>
        ///// 报工数量允许大于前工序报工数量（合格）
        ///// </summary>
        //public bool ProcessReportingRelationship
        //{
        //    get { return this.GetProperty(ProcessReportingRelationshipProperty); }
        //    set { this.SetProperty(ProcessReportingRelationshipProperty, value); }
        //}
        //#endregion

        //#region 是否启用报工确认 NeedCheck
        ///// <summary>
        ///// 是否启用报工确认
        ///// </summary>
        //[Label("是否启用报工确认")]
        //public static readonly Property<bool> NeedCheckProperty = P<ReportRecordConfigValue>.Register(e => e.NeedCheck);

        ///// <summary>
        ///// 是否启用报工确认
        ///// </summary>
        //public bool NeedCheck
        //{
        //    get { return this.GetProperty(NeedCheckProperty); }
        //    set { this.SetProperty(NeedCheckProperty, value); }
        //}
        //#endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>编码规则名称|打印模板名称</returns>
        public override string Display()
        {

            //return $"报工批次号编码规则：{ReportBatchNoRule?.Name} | 报工打印模板：{ReportPrintTemplate?.FileName}";
            return "报工批次号编码规则:{0} | 报工打印模板：{1} "
                .L10nFormat(ReportBatchNoRule==null?"":ReportBatchNoRule.Name, ReportPrintTemplate == null ? "":ReportPrintTemplate.FileName);
        }
    }
}
