using SIE.Domain;
using SIE.EMS.InspectionRules;
using SIE.EMS.SpecialEquipment.RegularInspections;
using SIE.EMS.SpecialEquipment.SpecialEquipmentAcounts;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands;
using SIE.Web.EMS.SpecialEquipment.RegularInspections.WorkFlows;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.SpecialEquipment.RegularInspections
{
    /// <summary>
    /// 特种设备定检视图配置
    /// </summary>
    public class RegularInspectionViewConfig : WebViewConfig<RegularInspection>
    {
        /// <summary>
        /// 录入报告视图
        /// </summary>
        private readonly string WritingReportView = "WriteReportView";

        /// <summary>
        /// 查看报告视图
        /// </summary>
        private readonly string ReadonlyView = "ReadonlyView";

        /// <summary>
        /// 审核视图
        /// </summary>
        private readonly string AuditView = "AuditView";


        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.DeclareExtendViewGroup(WritingReportView, ReadonlyView, AuditView);
            View.RequierModels(typeof(RegularInspectionValue));
            if (ViewGroup == WritingReportView)
            {
                ConfigWritingReportView();
            }
            if (ViewGroup == ReadonlyView)
            {
                ConfigReadonlyView();
            }
            if (ViewGroup == AuditView)
            {
                ConfigAuditView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.SpecialEquipment.Common.Behaviors.ApprovalBehavior");
            View.AddBehavior("SIE.Web.EMS.SpecialEquipment.RegularInspections.RegularInspectionListBehavior");
            View.AssignAuthorize(typeof(RegularInspection));
            View.FormEdit();
            View.UseCommands(WebCommandNames.Add, RegularInspectionCommands.WritingReportCommand, RegularInspectionCommands.ViewReportCommand, RegularInspectionCommands.AuditCommand, "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.AddRegularInspectionRepairCommand", WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);

            using (View.OrderProperties())
            {
                View.Property(p => p.InspectionNo);
                View.Property(p => p.SpecialEquipmentAccountId).HasLabel("设备编码");
                View.Property(p => p.SpecialEquipmentAccountName);
                View.Property(p => p.PlanInspectionDate).UseDateEditor();
                View.Property(p => p.ActualInspectionDate).UseDateEditor();
                View.Property(p => p.InspectionRemark);
                View.Property(p => p.Inspector).HasLabel("检验人");
                View.Property(p => p.InspectionResult).HasLabel("检验结果");
                View.Property(p => p.InspectionStatus).HasLabel("检验状态");
                View.Property(p => p.BillSourceType);
                View.Property(p => p.AgencyId).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.AgencyName), nameof(e.Agency.Name));
                    m.DicLinkField = dic;
                    m.DisplayField = RegularInspection.AgencyNameProperty.Name;
                    m.BindDisplayField = RegularInspection.AgencyNameProperty.Name;
                }).HasLabel("检验机构");
                View.Property(p => p.ApprovalStatus);
                View.Property(p => p.InspectionRule).HasLabel("检验规程");
                View.Property(p => p.PoNo);
                View.Property(p => p.EquipTypeName);
                View.Property(p => p.ResPerson);
                View.Property(p => p.UseDepartment).HasLabel("使用部门");
                View.Property(p => p.Manufacturer);
                View.Property(p => p.CheckCategory).HasLabel("检验类型");
                View.Property(p => p.EquipModelName);
                View.Property(p => p.EnterDate).UseDateEditor();
                View.Property(p => p.Remark);
                View.ChildrenProperty(p => p.RegularInspectionDetailList).HasLabel("检验明细").Show(ChildShowInWhere.Hide).HasOrderNo(1);
                View.ChildrenProperty(p => p.RegularInspectionResumeList).HasLabel("操作记录").Show(ChildShowInWhere.All).HasOrderNo(2);
                View.ChildrenProperty(p => p.RegularInspectionAttachmentList).HasLabel("检验附件").Show(ChildShowInWhere.Hide).HasOrderNo(3);
                View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
                {
                    var args = w as ChildPagingDataArgs;
                    var parent = args.Parent.CastTo<RegularInspection>();
                    if (parent == null)
                    {
                        return new EntityList<WorkFlowRecord>();
                    }
                    return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(RegularInspection).FullName, args.SortInfo, args.PagingInfo);
                }, WorkFlowRecordViewConfig.RegSeeView).HasLabel("审核记录").HasOrderNo(4);
            }
        }

        ///<summary>
        /// 配置明细视图-添加
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(RegularInspection));
            View.AddBehavior("SIE.Web.EMS.SpecialEquipment.RegularInspections.Behaviors.RegularInspectionClearBehavior");
            View.HasDetailColumnsCount(2);
            View.UseCommands("SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands.SaveRegularInspectionCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.SpecialEquipmentAccount).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var item = source as RegularInspection;
                    if (item != null)
                    {
                        return RT.Service.Resolve<SpecialEquipAccountController>().GetSpecialEquipAccountList(pagingInfo, keyword);
                    }
                    else
                    {
                        return new EntityList<SpecialEquipmentAccount>();
                    }
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.SpecialEquipmentAccountName), nameof(e.SpecialEquipmentAccount.Name));
                    m.DicLinkField = dic;
                }).Cascade(p => p.InspectionRuleId, null).Cascade(p => p.CheckCategory, null).HasLabel("设备编码");
                View.Property(p => p.SpecialEquipmentAccountName).HasLabel("设备名称").Readonly();
                View.Property(p => p.InspectionRuleId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var item = source as RegularInspection;
                    if (item != null)
                    {
                        return RT.Service.Resolve<RegularInspectionController>().GetInspectionRuleList(item.SpecialEquipmentAccountId, pagingInfo, keyword);
                    }
                    else
                    {
                        return new EntityList<InspectionRule>();
                    }
                }).UsePagingLookUpEditor((m, e) =>
               {
                   Dictionary<string, string> dic = new Dictionary<string, string>();
                   dic.Add(nameof(e.CheckCategory), nameof(e.InspectionRule.CheckCategory));
                   m.DicLinkField = dic;
               }).HasLabel("检验规程");
                View.Property(p => p.CheckCategory).HasLabel("检验类型").Readonly();
                View.Property(p => p.PlanInspectionDate).UseDateEditor(p =>
                {
                    p.Format = "Y-m-d";
                    p.MinValue = DateTime.Today.ToString();
                }).DefaultValue(DateTime.Today);
                View.Property(p => p.Remark).HasLabel("备注".L10N() + "*");
                View.ChildrenProperty(p => p.RegularInspectionDetailList).HasLabel("检验明细").Show(ChildShowInWhere.Detail).HasOrderNo(1);
                View.ChildrenProperty(p => p.RegularInspectionResumeList).HasLabel("操作记录").Show(ChildShowInWhere.Hide).HasOrderNo(2);
                View.ChildrenProperty(p => p.RegularInspectionAttachmentList).HasLabel("检验附件").Show(ChildShowInWhere.Hide).HasOrderNo(3);
            }
        }


        /// <summary>
        /// 录入检验报告视图
        /// </summary>
        protected void ConfigWritingReportView()
        {
            View.AssignAuthorize(typeof(RegularInspection));
            View.AddBehavior("SIE.Web.EMS.SpecialEquipment.RegularInspections.RegularInspectionBehavior");
            View.UseCommands(typeof(InputSaveCommand).FullName, typeof(InputSubmitCommand).FullName);
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.InspectionNo).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SpecialEquipmentAccount).HasLabel("设备编号").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SpecialEquipmentAccountName).HasLabel("设备名称").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipModelName).HasLabel("设备型号").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckCategory).HasLabel("检验类型").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ResPerson).HasLabel("资产责任人").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.UseDepartment).HasLabel("使用部门").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ActualInspectionDate).UseDateEditor(p =>
                {
                    p.Format = "Y-m-d";
                    p.MaxValue = DateTime.Today.ToString();
                }).DefaultValue(DateTime.Today).HasLabel("检验日期".L10N() + "*").Show(ShowInWhere.All);
                View.Property(p => p.Inspector).HasLabel("检验人").Show(ShowInWhere.All);
                View.Property(p => p.InspectionResult).HasLabel("检验结果".L10N() + "*").Show(ShowInWhere.All);
                View.Property(p => p.Agency).HasLabel("检验机构").ShowInDetail(width: "35%", columnSpan: 4).Show(ShowInWhere.All);
                View.Property(p => p.InspectionRemark).UseMemoEditor().ShowInDetail(columnSpan: 4);

                View.ChildrenProperty(p => p.RegularInspectionDetailList)
                    .UseViewGroup(RegularInspectionDetailViewConfig.InputRegularInspection)
                    .HasLabel("检验明细").Show(ChildShowInWhere.Detail).HasOrderNo(1);

                View.ChildrenProperty(p => p.RegularInspectionResumeList)
                    .HasLabel("操作记录").Show(ChildShowInWhere.Hide).HasOrderNo(2);
                View.ChildrenProperty(p => p.RegularInspectionAttachmentList).HasLabel("检验附件").Show(ChildShowInWhere.Detail).HasOrderNo(3);
            }
        }


        ///<summary>
        /// 查看检验报告视图
        /// </summary>
        protected void ConfigReadonlyView()
        {
            View.AssignAuthorize(typeof(RegularInspection));
            View.DisableEditing();
            View.AddBehavior("SIE.Web.EMS.SpecialEquipment.RegularInspections.RegularInspectionBehavior");
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.InspectionNo).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SpecialEquipmentAccount).HasLabel("设备编号").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SpecialEquipmentAccountName).HasLabel("设备名称").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipModelName).HasLabel("设备型号").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckCategory).HasLabel("检验类型").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ResPerson).HasLabel("资产责任人").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.UseDepartment).HasLabel("使用部门").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ActualInspectionDate).HasLabel("检验日期").UseDateEditor().Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Inspector).HasLabel("检验人").Show(ShowInWhere.All);
                View.Property(p => p.InspectionResult).HasLabel("检验结果").Show(ShowInWhere.All);
                View.Property(p => p.Agency).HasLabel("检验机构").ShowInDetail(width: "35%", columnSpan: 4).Show(ShowInWhere.All);
                View.Property(p => p.InspectionRemark).UseMemoEditor().ShowInDetail(columnSpan: 4).Readonly();
                View.Property(p => p.ApprovalInfo).UseMemoEditor().ShowInDetail(columnSpan: 4).Readonly();
                View.ChildrenProperty(p => p.RegularInspectionDetailList).UseViewGroup(RegularInspectionDetailViewConfig.ReadonlyView).Show(ChildShowInWhere.All).HasLabel("检验明细").HasOrderNo(1);
                View.ChildrenProperty(p => p.RegularInspectionResumeList).HasLabel("操作记录").Show(ChildShowInWhere.Hide).HasOrderNo(2);
                View.ChildrenProperty(p => p.RegularInspectionAttachmentList).Show(ChildShowInWhere.All).UseViewGroup("Readonly").HasLabel("检验附件").HasOrderNo(3);
            }
        }

        /// <summary>
        /// 审核视图
        /// </summary>
        protected void ConfigAuditView()
        {
            View.AssignAuthorize(typeof(RegularInspection));
            View.AddBehavior("SIE.Web.EMS.SpecialEquipment.RegularInspections.RegularInspectionBehavior");
            View.HasDetailColumnsCount(4);
            View.UseCommands(typeof(AuditSubmitCommand).FullName, typeof(AuditRejectCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.InspectionNo).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SpecialEquipmentAccount).HasLabel("设备编号").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SpecialEquipmentAccountName).HasLabel("设备名称").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipModelName).HasLabel("设备型号").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckCategory).HasLabel("检验类型").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ResPerson).HasLabel("资产责任人").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.UseDepartment).HasLabel("使用部门").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ActualInspectionDate).HasLabel("检验日期").Show(ShowInWhere.All).UseDateEditor().Readonly();
                View.Property(p => p.Inspector).HasLabel("检验人").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.InspectionResult).HasLabel("检验结果").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Agency).HasLabel("检验机构").ShowInDetail(width: "35%", columnSpan: 4).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.InspectionRemark).UseMemoEditor().ShowInDetail(columnSpan: 4).Readonly();

                View.Property(p => p.ApprovalInfo).UseMemoEditor().ShowInDetail(columnSpan: 4);

                View.ChildrenProperty(p => p.RegularInspectionDetailList).UseViewGroup(RegularInspectionDetailViewConfig.ReadonlyView).Show(ChildShowInWhere.All).HasLabel("检验明细").HasOrderNo(1);
                View.ChildrenProperty(p => p.RegularInspectionResumeList).HasLabel("操作记录").Show(ChildShowInWhere.Hide).HasOrderNo(2);
                View.ChildrenProperty(p => p.RegularInspectionAttachmentList).Show(ChildShowInWhere.All).UseViewGroup("Readonly").HasLabel("检验附件").HasOrderNo(3);

            }

        }

    }
}