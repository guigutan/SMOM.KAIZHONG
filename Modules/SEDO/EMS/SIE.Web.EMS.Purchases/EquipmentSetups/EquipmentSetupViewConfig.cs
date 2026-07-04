using SIE.Domain;
using SIE.EMS.Projects;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.EMS.Extensions;
using SIE.Web.EMS.Purchases.EquipmentSetups.Commands;
using SIE.Web.EMS.Purchases.WorkFlows;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试视图配置
    /// </summary>
    public class EquipmentSetupViewConfig : WebViewConfig<EquipmentSetup>
    {
        /// <summary>
        /// 安装说明界面
        /// </summary>
        public const string ExplainView = "ExplainView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ExplainView);
            if (ViewGroup == ExplainView)
            {
                ConfigExplainView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.Purchases.Common.ApprovalBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.AddSetupCommand", "SIE.Web.EMS.Purchases.Common.Commands.ApprovalStatusEditCommand",
                typeof(DeleteSetupCommand).FullName, typeof(SaveEquipSetupCommand).FullName, typeof(ReassignmentCommand).FullName,
                typeof(SubmitEquipmentSetupCommand).FullName, typeof(CancelEquipmentSetupCommand).FullName, typeof(ExamineEquipmentSetupCommand).FullName,
                typeof(HandoverConfirmCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.FactoryId).ShowInList(120);
            View.Property(p => p.SetupNo).ShowInList(130);
            View.Property(p => p.SetupStatus).HasLabel("状态").ShowInList(80);
            View.Property(p => p.ApprovalStatus).ShowInList(80);
            View.Property(p => p.DepartmentId).ShowInList(120);
            View.Property(p => p.WorkshopId).ShowInList(120);
            View.Property(p => p.Location).ShowInList(200);
            View.Property(p => p.PlanStartDate).UseDateEditor().ShowInList(150);
            View.Property(p => p.PlanEndDate).UseDateEditor().ShowInList(150);
            View.Property(p => p.PrincipalShow).ShowInList(200);
            View.Property(p => p.ProjectId).HasLabel("项目编码").ShowInList(130);
            View.Property(p => p.ProjectName).ShowInList(200);
            View.Property(p => p.OutSource).Readonly().ShowInList(50);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.EquipmentSetupPlanList).HasLabel("工作计划").HasOrderNo(1);
            View.ChildrenProperty(p => p.EquipmentDetailList).HasLabel("设备明细").HasOrderNo(2);

            //备件申请页签
            View.ChildrenProperty(p => p.SetupSparePartApplyList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(EquipmentSetup.SetupSparePartApplyListProperty, e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<EquipmentSetup>();
                if (parent == null)
                {
                    return new EntityList<SetupSparePartApply>();
                }
                else
                {
                    return RT.Service.Resolve<EquipmentSetupController>().GetSetupSparePartApplys(parent, arg.PagingInfo, arg.SortInfo);
                }
            }).HasLabel("备件申请").HasOrderNo(3);

            //备件使用页签
            View.ChildrenProperty(p => p.SetupSparePartList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(EquipmentSetup.SetupSparePartListProperty, e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<EquipmentSetup>();
                if (parent == null)
                {
                    return new EntityList<SetupSparePart>();
                }
                else
                {
                    return RT.Service.Resolve<EquipmentSetupController>().GetSetupSpareParts(parent);
                }
            }).HasLabel("备件使用").HasOrderNo(4);

            View.ChildrenProperty(p => p.SetupWorkHourList).HasLabel("工时").HasOrderNo(5);
            View.ChildrenProperty(p => p.SetupAttachmentList).HasLabel("附件").HasOrderNo(6);
            View.AttachDetailChildrenProperty(typeof(EquipmentSetup), (c) =>
            {
                var model = c.Parent as EquipmentSetup;
                if (model == null)
                {
                    return model;
                }
                return RF.GetById<EquipmentSetup>(model.Id);
            }, ExplainView).Show(ChildShowInWhere.All).HasLabel("安装说明").HasOrderNo(7);
            View.ChildrenProperty(p => p.SetupLogList).HasLabel("操作记录").HasOrderNo(8);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<EquipmentSetup>();
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }
                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(EquipmentSetup).FullName, args.SortInfo, args.PagingInfo);
            }, WorkFlowRecordViewConfig.PurSeeView).HasLabel("审核记录").HasOrderNo(9);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.UseDetail(4);
            View.AddBehavior("SIE.Web.EMS.Purchases.EquipmentSetups.EquipmentSetupBehavior");
            View.ClearCommands();
            View.UseCommand(typeof(FormSaveEquipSetupCommand).FullName);
            View.Property(p => p.SetupNo).Readonly();
            View.Property(p => p.FactoryId).UseFactoryEditor().Cascade(p => p.DepartmentId, null).Cascade(p => p.WorkshopId, null);
            View.Property(p => p.DepartmentId).UseUserBudgetDepartmentEditor();
            View.Property(p => p.WorkshopId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var entity = source as EquipmentSetup;
                if (entity == null)
                {
                    return new EntityList<Enterprise>();
                }
                var workshop = RT.Service.Resolve<EnterpriseController>().GetWorkShops(pagingInfo, keyword, entity.FactoryId);
                if (workshop == null || workshop.Count <= 0)
                {
                    return new EntityList<Enterprise>();
                }
                workshop.ForEach(p => p.TreePId = null);
                return workshop;
            });
            View.Property(p => p.PlanStartDate).UseDateEditor();
            View.Property(p => p.PlanEndDate).UseDateEditor();
            View.Property(p => p.PrincipalId);
            View.Property(p => p.Location);
            View.Property(p => p.ProjectId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<ProjectController>().GetAuditedProjects(pagingInfo, keyword);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ProjectName), nameof(e.Project.Name));
                m.DicLinkField = keyValues;
            }).HasLabel("项目编码");
            View.Property(p => p.ProjectName).Readonly();
            View.Property(p => p.OutSource);
            View.ChildrenProperty(p => p.EquipmentSetupPlanList).UseViewGroup(EquipmentSetupPlanViewConfig.EditView).HasLabel("工作计划").HasOrderNo(1);
            View.ChildrenProperty(p => p.EquipmentDetailList).UseViewGroup(EquipmentDetailViewConfig.EditView).HasLabel("设备明细").HasOrderNo(2);
            View.ChildrenProperty(p => p.SetupSparePartApplyList).HasLabel("备件申请").Show(ChildShowInWhere.Hide).HasOrderNo(3);
            View.ChildrenProperty(p => p.SetupAttachmentList).HasLabel("附件").HasOrderNo(4);
            View.AttachDetailChildrenProperty(typeof(EquipmentSetup), (c) =>
            {
                var model = c.Parent as EquipmentSetup;
                if (model == null)
                {
                    return model;
                }
                return RF.GetById<EquipmentSetup>(model.Id);
            }, ExplainView).Show(ChildShowInWhere.All).HasLabel("安装说明").HasOrderNo(5);
        }

        /// <summary>
        /// 安装说明界面
        /// </summary>
        protected void ConfigExplainView()
        {
            View.AssignAuthorize(typeof(EquipmentSetup));
            View.ClearCommands();
            View.UseDetail(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.SetupNote).UseMemoEditor().ShowInDetail(columnSpan: 4)
                    .Readonly(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject);
            }
        }
    }
}