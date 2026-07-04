using SIE.Domain;
using SIE.EMS.EquipRepair.PlanRepairs;
using SIE.EMS.Projects;
using SIE.Equipments.WorkFlows;
using SIE.Web.EMS.EquipRepair.PlanRepairs.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipRepair.PlanRepairs
{
    /// <summary>
    /// 计划维修视图配置
    /// </summary>
    public class PlanRepairViewConfig : WebViewConfig<PlanRepair>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(PlanRepair.NoProperty);
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.EquipRepairs.PlanRepairs.PlanRepairsBehavior");
            View.UseCommands("SIE.Web.EMS.EquipRepair.Commands.AddPlanRepairsCommand",
                "SIE.Web.EMS.EquipRepair.PlanRepairs.Commands.EditRunStandardsCommand",
               typeof(PromptlyDeleteCommand).FullName,
               typeof(SubmitCommand).FullName,
               typeof(ForcedShutdownCommand).FullName,
               typeof(ApprovalCommand).FullName,
               typeof(CancelCommand).FullName
              );
            View.DisableEditing();
            View.FormEdit();
            View.Property(p => p.No).ShowInList(120);
            View.Property(p => p.Name).ShowInList(120);
            View.Property(p => p.RunStandardNo).ShowInList(120);
            View.Property(p => p.EquipAccount).HasLabel("设备编码").ShowInList(150);
            View.Property(p => p.EquipName).HasLabel("设备名称").ShowInList(100);
            View.Property(p => p.Project).HasLabel("项目编码").ShowInList(100);
            View.Property(p => p.ProjectKeyItem).HasLabel("项目事项").ShowInList(100);
            View.Property(p => p.BillSourceType).ShowInList(80);
            View.Property(p => p.ApprovalStatus);
            View.Property(p => p.StandardType);
            View.Property(p => p.Amount);
            View.Property(p => p.StandardUnit);
            View.Property(p => p.RepairHours);

            View.Property(p => p.RoundAmount);
            View.Property(p => p.LastExecuteDate);
            View.Property(p => p.LeadTime);
            View.Property(p => p.EquipRepairBill).HasLabel("维修单号");
            View.Property(p => p.RepairState).HasLabel("维修状态");
            View.Property(p => p.Close).HasLabel("已关闭");
            View.Property(p => p.Remark);
            //View.Property(p => p.EquipAccountRepairStandard).HasLabel("维修定标");
            View.ChildrenProperty(p => p.PlanRepairProjectList).HasLabel("维修规程");
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as PlanRepair;
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }

                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id,
                    typeof(PlanRepair).FullName, args.SortInfo, args.PagingInfo);

            }, ListView).HasLabel("审核记录").HasOrderNo(5);

        }

        /// <summary>
        /// 配置明细页面
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(3);
            View.UseCommands(typeof(SavePlanRepairsCommand).FullName);
            View.AddBehavior("SIE.Web.EMS.EquipRepairs.PlanRepairs.PlanRepairsBehavior");
            View.Property(p => p.No).Readonly();
            View.Property(p => p.Name);
            View.Property(p => p.EquipAccount).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.EquipName), nameof(e.EquipAccount.Name));
                dic.Add(nameof(e.EquipAccountModelId), nameof(e.EquipAccount.EquipModelId));
                m.DicLinkField = dic;
            }).HasLabel("设备编码");
            View.Property(p => p.EquipName).HasLabel("设备名称").Readonly();
            View.Property(p => p.Project).HasLabel("项目编码").Cascade(p=>p.ProjectKeyItem,null);
            View.Property(p => p.ProjectKeyItem).UseDataSource((source, pagingInfo, keyword) =>
            {
                var planRepair = source as PlanRepair;
                if (planRepair == null || planRepair.ProjectId == null)
                {
                    return new EntityList<ProjectKeyItem>();
                }

                return RT.Service.Resolve<ProjectController>().GetProjectKeyItemsOfProject(planRepair.ProjectId.Value, pagingInfo, keyword);
            }).ShowInDetail().HasLabel("项目事项");

            View.Property(p => p.RepairHours).UseSpinEditor(m=>m.MinValue=0);
            View.Property(p => p.Remark).ShowInDetail( columnSpan:2);
            View.ChildrenProperty(p => p.PlanRepairProjectList).UseViewGroup(PlanRepairProjectViewConfig.EditView).HasLabel("维修项目");
        }
    }
}