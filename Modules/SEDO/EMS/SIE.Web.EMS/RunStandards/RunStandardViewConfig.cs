using SIE.Domain;
using SIE.EMS.RunStandards;
using SIE.Equipments.EquipModels;
using SIE.Equipments.WorkFlows;
using SIE.Web.EMS.Common.Commands;
using SIE.Web.EMS.RunStandards.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.RunStandards
{
    /// <summary>
    /// 设备运行定标
    /// </summary>
    public class RunStandardViewConfig:WebViewConfig<RunStandard>
    {

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.FormEdit();
            View.AddBehavior("SIE.Web.EMS.RunStandards.RunStandardsListBehavior");
            View.UseCommands("SIE.Web.EMS.RunStandards.Commands.AddRunStandardsCommand",
                "SIE.Web.EMS.RunStandards.Commands.EditRunStandardsCommand",
                 typeof(PromptlyDeleteCommand).FullName,
                typeof(SubmitCommand).FullName,
                typeof(ApprovalCommand).FullName,typeof(CancelCommand).FullName);
            View.Property(p => p.No).ShowInList().Readonly();
            View.Property(p => p.Name).ShowInList().Readonly();
            View.Property(p => p.EquipModelId).ShowInList().Readonly();
            View.Property(p => p.ApprovalStatus).ShowInList().Readonly();
            View.Property(p => p.RepairHours).ShowInList().Readonly();
            View.Property(p => p.Remark).ShowInList().Readonly();
            View.Property(p => p.CreateByName).HasLabel("制定人").ShowInList().Readonly();
            View.ChildrenProperty(p => p.RunStandardEquipmentList).HasLabel("设备清单").HasOrderNo(1);
            View.ChildrenProperty(p => p.RunStandardProjectList).HasLabel("维修标准").HasOrderNo(2);
            View.ChildrenProperty(p => p.RunStandardValueList).HasLabel("定标量").HasOrderNo(3);
            View.ChildrenProperty(p => p.RunStandardLogList).HasLabel("操作记录").HasOrderNo(4);
            View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as RunStandard;
                if (parent == null)
                {
                    return new EntityList<WorkFlowRecord>();
                }

                return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id,
                    typeof(RunStandard).FullName, args.SortInfo, args.PagingInfo);

            }, ListView).HasLabel("审核记录").HasOrderNo(5);

        }

        /// <summary>
        /// 配置明细
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(3);
            View.AddBehavior("SIE.Web.EMS.RunStandards.RunStandardsBehavior");
            View.UseCommands(typeof(SaveRunStandardsCommand).FullName);
            View.Property(p => p.No).ShowInDetail().Readonly();
            View.Property(p => p.Name).ShowInDetail();
            View.Property(p => p.EquipModelId).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EquipModelController>().GetEquipModels(pagingInfo, keyword);
            }).ShowInDetail();
            View.Property(p => p.CreateBy).ShowInDetail().Readonly().HasLabel("制定人");
            View.Property(p => p.RepairHours).ShowInDetail(width: "30%",columnSpan:3);
            View.Property(p => p.Remark).ShowInDetail().ShowInDetail(columnSpan: 2);

            View.ChildrenProperty(p => p.RunStandardEquipmentList).UseViewGroup(RunStandardEquipmentViewConfig.EditView).HasLabel("设备清单");
            View.ChildrenProperty(p => p.RunStandardProjectList).UseViewGroup(RunStandardProjectViewConfig.EditView).HasLabel("维修标准");
            View.ChildrenProperty(p => p.RunStandardValueList).UseViewGroup(RunStandardValueViewConfig.EditView).HasLabel("定标量");
        }
    }
}
