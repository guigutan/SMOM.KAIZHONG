using SIE.Domain;
using SIE.EMS.Checks;
using SIE.EMS.Checks.Confirmations;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Checks.Projects;
using SIE.EMS.Checks.Records;
using SIE.Web.EMS.Checks.Confirmations;
using SIE.Web.EMS.Checks.Confirmations.Commands;
using SIE.Web.EMS.Checks.Plans.Commands;
using SIE.Web.EMS.Checks.Projects;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.Checks.Plans
{
    /// <summary>
    /// 点检计划维护视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class CheckPlanViewConfig : WebViewConfig<CheckPlan>
    {
        #region ViewGroup

        /// <summary>
        /// 添加窗口保养明细列表
        /// </summary>
        public const string PlanAddList = "PlanAddList";

        /// <summary>
        /// 修改窗口保养明细列表
        /// </summary>
        public const string PlanEditList = "PlanEditList";

        /// <summary>
        /// 执行点检计划
        /// </summary>
        public const string PlanExecuteViewGroup = "PlanExecuteViewGroup";

        /// <summary>
        /// 点检确认
        /// </summary>
        public const string PlanConfirmViewGroup = "PlanConfirmViewGroup";

        private const string YMD_FORMAT = "Y/m/d";

        #endregion

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(PlanAddList, PlanEditList, PlanExecuteViewGroup, PlanConfirmViewGroup);
            View.AssignAuthorize(typeof(CheckPlanViewModel));
            if (ViewGroup == PlanAddList)
            {
                PlanAddListView();
            }
            if (ViewGroup == PlanEditList)
            {
                PlanEditListView();
            }
            if (ViewGroup == PlanExecuteViewGroup)
            {
                PlanExecuteView();
            }
            if (ViewGroup == PlanConfirmViewGroup)
            {
                PlanConfirmView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.CheckPlanNo);
            View.Property(p => p.CheckDate);
            View.Property(p => p.CheckCycleType);
            View.Property(p => p.EquipAccount);
            View.Property(p => p.ExeState);
        }

        /// <summary>
        /// 配置添加的计划明细视图
        /// </summary>
        protected void PlanAddListView()
        {
            View.FormEdit();
            View.AssignAuthorize(typeof(CheckPlanViewModel));
            View.UseCommands(typeof(GenerateCheckPlanCommand).FullName).DomainName("计划明细");
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipAccount).HasLabel("设备编码").Show(ShowInWhere.All).DisableSort();
                View.Property(p => p.CheckDate).UseDateEditor(d => d.Format = YMD_FORMAT).Show(ShowInWhere.All).DisableSort();
                View.Property(p => p.WhetherAcrossDay).Show(ShowInWhere.All).DisableSort();
                View.Property(p => p.CheckBeginDate).UseDateTimeEditor().Show(ShowInWhere.All).DisableSort();
                View.Property(p => p.CheckEndDate).UseDateTimeEditor().Show(ShowInWhere.All).DisableSort();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
            View.ChildrenProperty(p => p.CheckProjectList).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 配置修改的计划明细视图
        /// </summary>
        protected void PlanEditListView()
        {
            View.UseClientOrder();
            View.FormEdit();
            View.UseCommands(typeof(DelCheckPlanCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.CheckPlanNo).HasLabel("点检单号").Show(ShowInWhere.All);
                View.Property(p => p.CheckCycleType).HasLabel("周期类型").Show(ShowInWhere.Hide);
                View.Property(p => p.CheckDate).UseDateEditor(d => d.Format = YMD_FORMAT).HasLabel("点检日期").Show(ShowInWhere.All);
                View.Property(p => p.WhetherAcrossDay).Show(ShowInWhere.Hide);
                View.Property(p => p.CheckBeginDate).Show(ShowInWhere.All).ShowInList(width: 140);
                View.Property(p => p.CheckEndDate).Show(ShowInWhere.All).ShowInList(width: 140);
                View.Property(p => p.ExeState).HasLabel("状态").Show(ShowInWhere.All);
                View.Property(p => p.Department).HasLabel("部门").Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.CheckProjectList);
            }
        }

        /// <summary>
        /// 配置执行点检计划视图
        /// </summary>
        public void PlanExecuteView()
        {
            View.AssignAuthorize(typeof(CheckPlanViewModel), typeof(CheckRecord));
            View.AddBehavior("SIE.Web.EMS.Checks.Plans.Scripts.ExeCheckPlanBehavior");
            View.UseCommands(typeof(SaveExeCheckPlanCommand).FullName, typeof(SubmitExeCheckPlanCommand).FullName,
                "SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.AddCheckEquipRepairCommand");
            View.HasDetailColumnsCount(4);

            using (View.OrderProperties())
            {
                View.Property(p => p.CheckPlanNo).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipAccountCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipAccountName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipModelCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipModelName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipTypeCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipTypeName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.DepartmentId).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckPlanType).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckBeginDate).UseDateTimeEditor().Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckEndDate).UseDateTimeEditor().Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ExeState).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ExeResult).HasLabel("执行结果".L10N()+"*").Show(ShowInWhere.All)
                    .Readonly(p => p.ExeState != SIE.EMS.Enums.CheckExeState.NotPerformed && p.ExeState != SIE.EMS.Enums.CheckExeState.Performing);
                View.Property(p => p.CheckDate).UseDateEditor(d => d.Format = YMD_FORMAT).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckEmployeeId).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckSourceType).Show(ShowInWhere.All).Readonly();

                View.Property(p => p.LastCheckSummary).UseMemoEditor().ShowInDetail(columnSpan: 2, rowSpan: 2).Readonly();
                View.Property(p => p.CheckSummary).UseMemoEditor().ShowInDetail(columnSpan: 2, rowSpan: 2)
                    .Readonly(p => p.ExeState != SIE.EMS.Enums.CheckExeState.NotPerformed && p.ExeState != SIE.EMS.Enums.CheckExeState.Performing);


                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
            View.ChildrenProperty(p => p.CheckProjectList).Show(ChildShowInWhere.All).OrderNo = 1;
            View.ChildrenProperty(p => p.CheckPlanSparePartAplList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.CheckPlanSparePartList).Show(ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(CheckPlanSparePart), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<CheckPlan>();
                if (parent == null)
                {
                    return new EntityList<CheckPlanSparePart>();
                }
                var spareParts = RT.Service.Resolve<CheckPlanController>().GetCheckPlanSpareParts(parent.Id, parent.CheckPlanNo, parent.EquipAccountId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return spareParts;
            }, directionParentPropertyName: CheckPlan.CheckPlanSparePartListProperty.Name).HasLabel("备件更换").Show(ChildShowInWhere.All).OrderNo = 2;
            View.AttachChildrenProperty(typeof(CheckPlanSparePartApl), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<CheckPlan>();
                if (parent == null)
                {
                    return new EntityList<CheckPlanSparePartApl>();
                }
                var spareParts = RT.Service.Resolve<CheckPlanController>().GetCheckPlanSparePartApls(parent.Id, parent.EquipAccountId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return spareParts;
            }, directionParentPropertyName: CheckPlan.CheckPlanSparePartAplListProperty.Name).HasLabel("备件申请").Show(ChildShowInWhere.All).OrderNo = 3;
            View.ChildrenProperty(p => p.CheckPlanAttachmentList).Show(ChildShowInWhere.All).OrderNo = 4;
        }

        /// <summary>
        /// 计划确认视图
        /// </summary>
        public void PlanConfirmView()
        {
            View.AssignAuthorize(typeof(CheckPlanViewModel));
            View.AddBehavior("SIE.Web.EMS.Checks.Confirmations.Scripts.ConfirmCheckPlanBehavior");
            View.UseCommands("SIE.Web.EMS.Checks.Confirmations.Commands.SubmitCheckConfirmationCommand");
            View.HasDetailColumnsCount(4);

            using (View.OrderProperties())
            {
                View.Property(p => p.CheckPlanNo).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipAccountCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipAccountName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipModelCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipModelName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipTypeCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipTypeName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ConfirmDeptDisplay).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckPlanType).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckBeginDate).UseDateTimeEditor().Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckEndDate).UseDateTimeEditor().Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ExeState).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ExeResult).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckDate).UseDateEditor(d => d.Format = "Y-m-d H:i:s").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckEmployeeId).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckSourceType).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CheckSummary).ShowInDetail(columnSpan: 3, rowSpan: 1).Readonly();
                View.Property(p => p.WhetherRepair).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ConfirmResult).HasLabel("点检确认结果".L10N()+"*").UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.ConfirmNote).HasLabel("备注").UseTextEditor(p => p.AllowBlank = true).ShowInDetail(columnSpan: 3, rowSpan: 1);
            }
            View.AttachChildrenProperty(typeof(CheckConfirmation), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<CheckPlan>();
                if (parent == null) { return new EntityList<CheckConfirmation>(); }

                var list = RT.Service.Resolve<CheckPlanController>().GetCheckConfirmationList(parent.Id, parent.ConfirmDeptId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, CheckConfirmationViewConfig.CheckConfirmView).HasLabel("评分项").Show(ChildShowInWhere.All).HasOrderNo(0);

            View.AttachChildrenProperty(typeof(CheckProject), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<CheckPlan>();
                if (parent == null) { return new EntityList<CheckProject>(); }

                var list = RT.Service.Resolve<CheckPlanController>().GetCheckProjectList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, CheckProjectViewConfig.CheckConfirmationListView).HasLabel("点检项目").Show(ChildShowInWhere.All).HasOrderNo(1);

            View.AttachChildrenProperty(typeof(CheckPlanSparePart), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<CheckPlan>();
                if (parent == null) { return new EntityList<CheckPlanSparePart>(); }

                var list = RT.Service.Resolve<CheckPlanController>().GetCheckPlanSpareParts(parent.Id, parent.CheckPlanNo, parent.EquipAccountId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, CheckPlanSparePartViewConfig.CheckConfirmationListView).HasLabel("备件更换").Show(ChildShowInWhere.All).HasOrderNo(2);

            View.AttachChildrenProperty(typeof(CheckPlanSparePartApl), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<CheckPlan>();
                if (parent == null) { return new EntityList<CheckPlanSparePartApl>(); }

                var list = RT.Service.Resolve<CheckPlanController>().GetCheckPlanSparePartApls(parent.Id, parent.EquipAccountId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, CheckPlanSparePartAplViewConfig.CheckConfirmationListView).HasLabel("备件申请").Show(ChildShowInWhere.All).HasOrderNo(3);

            View.AttachChildrenProperty(typeof(CheckPlanAttachment), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<CheckPlan>();
                if (parent == null) { return new EntityList<CheckPlanAttachment>(); }

                var list = RT.Service.Resolve<CheckPlanController>().GetCheckPlanAttachmentList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, CheckPlanAttachmentViewConfig.CheckConfirmationListView).HasLabel("执行图片").Show(ChildShowInWhere.All).HasOrderNo(4);
        }

    }
}
