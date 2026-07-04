using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.InspectionRules;
using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Web.EMS.MeteringEquipment.Calibrations.Commands;
using SIE.Web.EMS.MeteringEquipment.WorkFlows;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations
{
    /// <summary>
    /// 计量设备定检视图配置
    /// </summary>
    public class CalibrationViewConfig : WebViewConfig<Calibration>
    {
        /// <summary>
        /// 添加记录
        /// </summary>
        public const string InputReportView = "InputReportView";

        /// <summary>
        /// 查看记录
        /// </summary>
        public const string SeeReportView = "SeeReportView";

        /// <summary>
        /// 审核视图
        /// </summary>
        private readonly string ApprovalView = "ApprovalView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(InputReportView, SeeReportView, ApprovalView);
            if (ViewGroup == InputReportView)
            {
                ConfigInputReportView();
            }
            if (ViewGroup == SeeReportView)
            {
                ConfigSeeReportView();
            }
            if (ViewGroup == ApprovalView)
            {
                ConfigApprovalView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.MeteringEquipment.Common.Behaviors.ApprovalBehavior");
            View.AddBehavior("SIE.Web.EMS.MeteringEquipment.Calibrations.Behaviors.CalibrationListBehavior");
            View.FormEdit();
            View.UseCommands(WebCommandNames.Add, CalibrationCommands.InputReportCommand, CalibrationCommands.SeeReportCommand, CalibrationCommands.AuditCommand, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);

            using (View.OrderProperties())
            {
                View.Property(p => p.InspectionNo);
                View.Property(p => p.PlanName);
                View.Property(p => p.InspectionStatus).HasLabel("检验状态");
                View.Property(p => p.InspectionResult).HasLabel("检验结果");
                View.Property(p => p.InspectionRule).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.InspectionRuleName), nameof(e.InspectionRule.Name));
                    m.DisplayField = Calibration.InspectionRuleNameProperty.Name;
                    m.BindDisplayField = Calibration.InspectionRuleNameProperty.Name;
                    m.DicLinkField = dic;
                }).HasLabel("检验规程");
                View.Property(p => p.CheckCategory).HasLabel("检验类型");
                View.Property(p => p.BillSourceType);
                View.Property(p => p.PlanInspectionDate).UseDateEditor().HasLabel("计划检验日期");
                View.Property(p => p.ActualInspectionDate).UseDateEditor().HasLabel("实际检验日期");
                View.Property(p => p.AgencyId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.AgencyName), nameof(e.Agency.Name));
                    m.DicLinkField = dic;
                    m.DisplayField = Calibration.AgencyNameProperty.Name;
                    m.BindDisplayField = Calibration.AgencyNameProperty.Name;
                }).HasLabel("检验机构");
                View.Property(p => p.InspectorId).HasLabel("检验人");
                View.Property(p => p.ApprovalStatus);
                View.Property(p => p.PoNo);
                View.Property(p => p.Remark);

                View.ChildrenProperty(p => p.CalibrationEquipmentList).UseViewGroup(CalibrationEquipmentViewConfig.ReadonlyView).HasLabel("设备明细").Show(ChildShowInWhere.All).HasOrderNo(1);
                View.ChildrenProperty(p => p.CalibrationItemList).UseViewGroup(CalibrationItemViewConfig.ReadonlyView).HasLabel("检验规程").Show(ChildShowInWhere.All).HasOrderNo(2);
                View.ChildrenProperty(p => p.CalibrationDetailList).HasLabel("检验明细").Show(ChildShowInWhere.Hide).HasOrderNo(3);
                View.ChildrenProperty(p => p.CalibrationResumeList).HasLabel("操作记录").Show(ChildShowInWhere.All).HasOrderNo(4);
                View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
                {
                    var args = w as ChildPagingDataArgs;
                    var parent = args.Parent.CastTo<Calibration>();
                    if (parent == null)
                    {
                        return new EntityList<WorkFlowRecord>();
                    }
                    return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(Calibration).FullName, args.SortInfo, args.PagingInfo);
                }, WorkFlowRecordViewConfig.RegSeeView).HasLabel("审核记录").HasOrderNo(5);

                View.ChildrenProperty(p => p.CalibrationAttachmentList).UseViewGroup(CalibrationAttachmentViewConfig.SeeView).HasLabel("检验附件").Show(ChildShowInWhere.Hide).HasOrderNo(6);
            }
        }


        ///<summary>
        ///添加计量设备定检
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(Calibration));
            View.AddBehavior("SIE.Web.EMS.MeteringEquipment.Calibrations.Behaviors.AddCalibrationBehavior");
            View.UseCommands(CalibrationCommands.CalibrationSaveCommand);
            View.HasDetailColumnsCount(2);
            using (View.OrderProperties())
            {
                View.Property(p => p.InspectionNo).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.PlanName).Show(ShowInWhere.All);

                View.Property(p => p.InspectionRuleId).UseDataSource((e, p, s) =>
                {
                    var model = e as Calibration;
                    if (model == null)
                    {
                        return new EntityList<InspectionRule>();
                    }
                    return RT.Service.Resolve<InspectionRuleController>().GetInspectionRuleList(InspectionRuleType.Metrology, s, p);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.CheckCategory), nameof(e.CheckCategory));
                    dic.Add(nameof(e.InspectionRuleName), nameof(e.InspectionRule.Name));
                    m.DicLinkField = dic;
                    m.DisplayField = Calibration.InspectionRuleNameProperty.Name;
                    m.BindDisplayField = Calibration.InspectionRuleNameProperty.Name;

                    m.DicLinkField = dic;
                }).Cascade(p=>p.AgencyId,null).HasLabel("检验规程").Show(ShowInWhere.All);
                View.Property(p => p.CheckCategory).HasLabel("检验类型").Readonly().Show(ShowInWhere.All);

                View.Property(p => p.PlanInspectionDate).UseDateEditor(p =>
                {
                    p.MinValue = DateTime.Today.ToString();
                }).DefaultValue(DateTime.Today).HasLabel("计划检验日期").Show(ShowInWhere.All);

                View.Property(p => p.AgencyId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.AgencyName), nameof(e.Agency.Name));
                    m.DicLinkField = dic;
                    m.DisplayField = Calibration.AgencyNameProperty.Name;
                    m.BindDisplayField = Calibration.AgencyNameProperty.Name;
                }).Readonly(p=>p.CheckCategory== SIE.Core.Enums.CheckCategory.InnerCheck).HasLabel("检验机构").Show(ShowInWhere.All);

                View.Property(p => p.Remark).HasLabel("备注".L10N()+"*").UseMemoEditor().ShowInDetail(columnSpan: 4).HasLabel("备注").Show(ShowInWhere.All);

                //子页签
                View.ChildrenProperty(p => p.CalibrationEquipmentList).HasLabel("设备明细").Show(ChildShowInWhere.All).HasOrderNo(1);
                View.ChildrenProperty(p => p.CalibrationItemList).HasLabel("检验规程").Show(ChildShowInWhere.All).HasOrderNo(2);
                View.ChildrenProperty(p => p.CalibrationDetailList).HasLabel("检验明细").Show(ChildShowInWhere.Hide).HasOrderNo(3);
                View.ChildrenProperty(p => p.CalibrationResumeList).HasLabel("操作记录").Show(ChildShowInWhere.Hide).HasOrderNo(4);
                View.ChildrenProperty(p => p.CalibrationAttachmentList).HasLabel("检验附件").Show(ChildShowInWhere.Hide).HasOrderNo(5);
            }
        }


        /// <summary>
        /// 录入检验报告
        /// </summary>
        private void ConfigInputReportView()
        {
            View.AssignAuthorize(typeof(Calibration));

            //保存,提交
            View.HasDetailColumnsCount(4);
            View.UseCommands(CalibrationCommands.CalibrationDetailSaveCommand, CalibrationCommands.CalibrationDetailSubmitCommand);
            using (View.OrderProperties())
            {
                View.Property(p => p.InspectionNo).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.PlanName).Show(ShowInWhere.All);
                View.Property(p => p.ActualInspectionDate).UseDateEditor(p =>
                {
                    p.Format = "Y-m-d";
                    p.MaxValue = DateTime.Today.ToString();
                }).DefaultValue(DateTime.Today).HasLabel("检验日期".L10N() + "*").Show(ShowInWhere.All);

                View.Property(p => p.CheckCategory).HasLabel("检验类型").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.InspectionResult).HasLabel("检验结果".L10N() + "*").Show(ShowInWhere.All);
                View.Property(p => p.Inspector).HasLabel("检验人").Show(ShowInWhere.All);
                View.Property(p => p.AgencyId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.AgencyName), nameof(e.Agency.Name));
                    m.DicLinkField = dic;
                    m.DisplayField = Calibration.AgencyNameProperty.Name;
                    m.BindDisplayField = Calibration.AgencyNameProperty.Name;
                    m.DicLinkField = dic;
                }).Readonly(p => p.CheckCategory == SIE.Core.Enums.CheckCategory.InnerCheck).HasLabel("检验机构").Show(ShowInWhere.All);

                View.Property(p => p.PoNo).HasLabel("采购单号").Show(ShowInWhere.All);
                View.Property(p => p.InspectionRemark).UseMemoEditor().ShowInDetail(columnSpan: 4).Show(ShowInWhere.All);

                //子页签
                View.ChildrenProperty(p => p.CalibrationEquipmentList).UseViewGroup(CalibrationEquipmentViewConfig.InputView).HasLabel("设备明细").Show(ChildShowInWhere.All).HasOrderNo(1);
                View.ChildrenProperty(p => p.CalibrationItemList).UseViewGroup(CalibrationItemViewConfig.ReadonlyView).HasLabel("检验规程").Show(ChildShowInWhere.All).HasOrderNo(2);
                View.ChildrenProperty(p => p.CalibrationDetailList).HasLabel("检验明细").Show(ChildShowInWhere.All).HasOrderNo(3);
                View.ChildrenProperty(p => p.CalibrationResumeList).HasLabel("操作记录").Show(ChildShowInWhere.Hide).HasOrderNo(4);
                View.ChildrenProperty(p => p.CalibrationAttachmentList).HasLabel("检验附件").Show(ChildShowInWhere.All).HasOrderNo(5);
            }
        }




        /// <summary>
        /// 审核视图
        /// </summary>
        protected void ConfigApprovalView()
        {
            View.AssignAuthorize(typeof(Calibration));
            View.HasDetailColumnsCount(4);
            View.UseCommands(typeof(AuditSubmitCommand).FullName, typeof(AuditRejectCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.InspectionNo).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.PlanName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ActualInspectionDate).UseDateEditor(p =>
                {
                    p.Format = "Y-m-d";
                    p.MaxValue = DateTime.Today.ToString();
                }).DefaultValue(DateTime.Today).HasLabel("检验日期").Readonly().Show(ShowInWhere.All);

                View.Property(p => p.CheckCategory).HasLabel("检验类型").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.InspectionResult).HasLabel("检验结果").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Inspector).HasLabel("检验人").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.AgencyId).HasLabel("检验机构").Readonly().Show(ShowInWhere.All);

                View.Property(p => p.PoNo).HasLabel("采购单号").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.InspectionRemark).UseMemoEditor().ShowInDetail(columnSpan: 4).Readonly().Show(ShowInWhere.All);

                View.Property(p => p.ApprovalInfo).UseMemoEditor().ShowInDetail(columnSpan: 4).Show(ShowInWhere.All);

                View.ChildrenProperty(p => p.CalibrationEquipmentList).UseViewGroup(CalibrationEquipmentViewConfig.ReadonlyView).HasLabel("设备明细").Show(ChildShowInWhere.All).HasOrderNo(1);
                View.ChildrenProperty(p => p.CalibrationItemList).UseViewGroup(CalibrationItemViewConfig.ReadonlyView).HasLabel("检验规程").Show(ChildShowInWhere.All).HasOrderNo(2);
                View.ChildrenProperty(p => p.CalibrationDetailList).UseViewGroup(CalibrationDetailViewConfig.ReadonlyView).HasLabel("检验明细").Show(ChildShowInWhere.All).HasOrderNo(3);
                View.ChildrenProperty(p => p.CalibrationResumeList).HasLabel("操作记录").Show(ChildShowInWhere.Hide).HasOrderNo(4);
                View.ChildrenProperty(p => p.CalibrationAttachmentList).UseViewGroup(CalibrationAttachmentViewConfig.SeeView).HasLabel("检验附件").Show(ChildShowInWhere.All).HasOrderNo(5);
            }
        }


        /// <summary>
        /// 查看记录页面
        /// </summary>
        private void ConfigSeeReportView()
        {
            View.AssignAuthorize(typeof(Calibration));
            View.FormEdit();
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.InspectionNo).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.PlanName).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ActualInspectionDate).UseDateEditor(p =>
                {
                    p.Format = "Y-m-d";
                    p.MaxValue = DateTime.Today.ToString();
                }).DefaultValue(DateTime.Today).HasLabel("检验日期").Readonly().Show(ShowInWhere.All);

                View.Property(p => p.CheckCategory).HasLabel("检验类型").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.InspectionResult).HasLabel("检验结果").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Inspector).HasLabel("检验人").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.AgencyId).HasLabel("检验机构").Readonly().Show(ShowInWhere.All);

                View.Property(p => p.PoNo).HasLabel("采购单号").Readonly().Show(ShowInWhere.All);
                View.Property(p => p.InspectionRemark).UseMemoEditor().ShowInDetail(columnSpan: 4).Readonly().Show(ShowInWhere.All);

                View.Property(p => p.ApprovalInfo).UseMemoEditor().ShowInDetail(columnSpan: 4).Readonly().Show(ShowInWhere.All);

                View.ChildrenProperty(p => p.CalibrationEquipmentList).UseViewGroup(CalibrationEquipmentViewConfig.ReadonlyView).HasLabel("设备明细").Show(ChildShowInWhere.All).HasOrderNo(1);
                View.ChildrenProperty(p => p.CalibrationItemList).UseViewGroup(CalibrationItemViewConfig.ReadonlyView).HasLabel("检验规程").Show(ChildShowInWhere.All).HasOrderNo(2);
                View.ChildrenProperty(p => p.CalibrationDetailList).UseViewGroup(CalibrationDetailViewConfig.ReadonlyView).HasLabel("检验明细").Show(ChildShowInWhere.All).HasOrderNo(3);
                View.ChildrenProperty(p => p.CalibrationResumeList).HasLabel("操作记录").Show(ChildShowInWhere.Hide).HasOrderNo(4);
                View.ChildrenProperty(p => p.CalibrationAttachmentList).UseViewGroup(CalibrationAttachmentViewConfig.SeeView).HasLabel("检验附件").Show(ChildShowInWhere.All).HasOrderNo(5);
            }
        }
    }
}