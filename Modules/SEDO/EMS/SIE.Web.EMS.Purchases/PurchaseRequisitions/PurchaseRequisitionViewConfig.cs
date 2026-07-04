using SIE.Domain;
using SIE.EMS.Projects;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Web.EMS.Extensions;
using SIE.Web.EMS.Purchases._Extensions_;
using SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands;
using SIE.Web.EMS.Purchases.WorkFlows;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 采购申请视图配置
    /// </summary>
    internal class PurchaseRequisitionViewConfig : WebViewConfig<PurchaseRequisition>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.Purchases.Common.ApprovalBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.PurchaseRequisitions.Commands.AddPurRequireCommand", "SIE.Web.EMS.Purchases.Common.Commands.ApprovalStatusEditCommand",
                typeof(DeletePurRequireCommand).FullName, typeof(SubmitPurRequireCommand).FullName, typeof(CancelPurRequireCommand).FullName,
                typeof(ExaminePurRequireCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.FactoryId).ShowInList(120);
            View.Property(p => p.DepartmentId).ShowInList(120);
            View.Property(p => p.No).ShowInList(130);
            View.Property(p => p.PurchaseType).ShowInList(100);
            View.Property(p => p.ApprovalStatus).ShowInList(80);
            View.Property(p => p.ApprovalTime).ShowInList(150);
            View.Property(p => p.PurchaseObjectType).ShowInList(80);
            View.Property(p => p.ProjectId).HasLabel("项目编码").ShowInList(130);
            View.Property(p => p.ProjectName).ShowInList(200);
            View.Property(p => p.VarietyQuantity).ShowInList(60);
            View.Property(p => p.TotalAmount).ShowInList(70);
            View.Property(p => p.PurchaseBudget).UseSpinEditor(p => p.DecimalPrecision = 3).ShowInList(150);
            View.Property(p => p.Currency).ShowInList(60);
            View.Property(p => p.AmountUnit).ShowInList(80);
            View.Property(p => p.BidAmount).ShowInList(150);
            View.Property(p => p.Remark).ShowInList(200);
            View.Property(p => p.CreateByName).Show(ShowInWhere.All);
            View.Property(p => p.CreateDate).Show(ShowInWhere.All);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.All);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.All);
            View.Property(p => p.WorkflowStartResult).ShowInList(200);
            View.ChildrenProperty(p => p.DetailList).HasLabel("采购明细").HasOrderNo(1);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件").HasOrderNo(2);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<PurchaseRequisition>();
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(PurchaseRequisition).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.PurSeeView).HasLabel("审核记录").HasOrderNo(3);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(8);
            View.AddBehavior("SIE.Web.EMS.Purchases.PurchaseRequisitions.PurRequireBehavior");
            View.ClearCommands();
            View.UseCommand(typeof(SavePurRequireCommand).FullName);
            View.Property(p => p.No).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.FactoryId).ShowInDetail(columnSpan: 2).UseFactoryEditor().Cascade(p => p.DepartmentId, null).Cascade(p => p.ProjectId, null);
            View.Property(p => p.DepartmentId).ShowInDetail(columnSpan: 2).UseUserBudgetDepartmentEditor().Cascade(p => p.ProjectId, null);
            View.Property(p => p.PurchaseType).ShowInDetail(columnSpan: 2);
            View.Property(p => p.ProjectId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var pur = source as PurchaseRequisition;
                if (pur == null)
                {
                    return new EntityList<Project>();
                }
                return RT.Service.Resolve<ProjectController>().GetAuditedProjects(pur.FactoryId, pur.DepartmentId, pagingInfo, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ProjectName), nameof(e.Project.Name));
                m.DicLinkField = keyValues;
            }).ShowInDetail(columnSpan: 2).HasLabel("项目编号").Readonly(p => p.PurchaseType == PurchaseType.NoneProject);
            View.Property(p => p.ProjectName).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.PurchaseObjectType).UsePurchaseObjectEditor().ShowInDetail(columnSpan: 2);
            View.Property(p => p.VarietyQuantity).ShowInDetail(columnSpan: 2).HasLabel("品种数量").Readonly();
            View.Property(p => p.TotalAmount).ShowInDetail(columnSpan: 2).Readonly();
            View.Property(p => p.PurchaseBudget).ShowInDetail(columnSpan: 2).UseSpinEditor(p =>
            {
                p.DecimalPrecision = 2;
                p.MinValue = 0.01;
            });
            View.Property(p => p.Currency).ShowInDetail(columnSpan: 2);
            View.Property(p => p.AmountUnit).ShowInDetail(columnSpan: 2);
            View.Property(p => p.Remark).ShowInDetail(columnSpan: 4);
            View.ChildrenProperty(p => p.DetailList).UseViewGroup(PurchaseRequisitionItemViewConfig.EditView).HasLabel("采购明细").HasOrderNo(1);
            View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件").HasOrderNo(2);
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.No);
        }
    }
}