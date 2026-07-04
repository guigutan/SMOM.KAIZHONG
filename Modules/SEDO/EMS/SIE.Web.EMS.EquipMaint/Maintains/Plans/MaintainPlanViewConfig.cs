using SIE.Domain;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Maintains.Controller;
using SIE.EMS.Equipments;
using SIE.EMS.Maintains.Confirmations;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.EMS.Maintains.Projects;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web.EMS.EquipMaint.Equipments.Accounts;
using SIE.Web.EMS.EquipMaint.Maintains.Confirmations.Commands;
using SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands;
using SIE.Web.EMS.EquipMaint.Maintains.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Web.EMS.EquipMaint.Maintains.Confirmations;
using SIE.Core.Enums;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans
{
    /// <summary>
    /// 保养计划维护视图配置
    /// </summary>
    public class MaintainPlanViewConfig : WebViewConfig<MaintainPlan>
    {
        /// <summary>
        /// 添加窗口保养明细列表
        /// </summary>
        public readonly static string PlanAddList = "PlanAddList";

        /// <summary>
        /// 修改窗口保养明细列表
        /// </summary>
        public readonly static string PlanEditList = "PlanEditList";

        /// <summary>
        /// 批量添加窗口保养明细列表
        /// </summary>
        public readonly static string BatchAddMaintainPlan = "BatchAddMaintainPlanView";

        /// <summary>
        /// 添加保养计划窗口
        /// </summary>
        public readonly static string AddMaintainPlan = "AddMaintainPlan";

        /// <summary>
        /// 修改保养计划
        /// </summary>
        public readonly static string EditMaintainPlan = "EditMaintainPlan";

        /// <summary>
        /// 执行保养计划
        /// </summary>
        public readonly static string PlanExecuteViewGroup = "PlanExecuteViewGroup";

        /// <summary>
        /// 保养确认
        /// </summary>
        public readonly static string PlanConfirmViewGroup = "PlanConfirmViewGroup";

        /// <summary>
        /// 计划开始日期
        /// </summary>
        private const string PlanStartDateLabel = "计划开始日期";

        /// <summary>
        /// 计划结束日期
        /// </summary>
        private const string PlanEndDateLabel = "计划结束日期";
        /// <summary>
        /// Y/m/d
        /// </summary>
        private const string ymdFormat = "Y/m/d";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(PlanAddList, PlanEditList, BatchAddMaintainPlan, AddMaintainPlan, EditMaintainPlan, PlanExecuteViewGroup, PlanConfirmViewGroup);
            View.AssignAuthorize(typeof(MaintainPlanViewModel));
            if (ViewGroup == PlanAddList)
            {
                PlanAddListView();
            }
            if (ViewGroup == PlanEditList)
            {
                PlanEditListView();
            }
            if (ViewGroup == BatchAddMaintainPlan)
            {
                BatchAddMaintainPlanView();
            }
            if (ViewGroup == AddMaintainPlan)
            {
                AddMaintainPlanView();
            }
            if (ViewGroup == EditMaintainPlan)
            {
                EditMaintainPlanView();
            }
            if (ViewGroup == PlanExecuteViewGroup)
            {
                PlanExecuteView();
            }
            if (ViewGroup == PlanConfirmViewGroup)
            {
                ConfigPlanConfirmView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands("SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.OpenMaintainsCommand");
            View.FormEdit();
            View.Property(p => p.EquipAccount);
            View.Property(p => p.PlanBeginDate).UseDateEditor(d => d.Format = ymdFormat);
            View.Property(p => p.PlanEndDate).UseDateEditor(d => d.Format = ymdFormat);
            View.Property(p => p.BeginDay);
            View.Property(p => p.EndDay);
        }
        /// <summary>
        /// 添加保养计划窗口-明细子列表
        /// </summary>
        protected void PlanAddListView()
        {
            bool IsMaintainForPrecisePlan = RT.Service.Resolve<MaintainController>().IsMaintainForPrecisePlan();
            View.UseCommands(GenerateMaintainPlanCommand.CommandName, WebCommandNames.Delete)
                .DomainName("计划明细");
            View.UseClientOrder();
            using (View.OrderProperties())
            {
                View.Property(p => p.YearAndMonth).Show(ShowInWhere.All).UseDateEditor(d => d.Format = "y/m").Readonly();
                View.Property(p => p.Cycle).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipAccount).HasLabel("设备编码").Show(ShowInWhere.Hide);
                View.Property(p => p.PlanBeginDate).UseDateEditor(d => d.Format = "m/d").HasLabel(PlanStartDateLabel).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PlanEndDate).UseDateEditor(d => d.Format = "m/d").HasLabel(PlanEndDateLabel).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.MaintainTypeInfoId)
                    .UseDataSource((source, pagingInfo, keyword) =>
                    {
                        var sourceList = new EntityList<MaintainTypeInfo>();
                        MaintainPlan maintainPlan = source as MaintainPlan;

                        if (maintainPlan != null)
                        {
                            sourceList = RT.Service.Resolve<MaintainController>().GetMaintainTypeInfoList(maintainPlan.MaintainCycleType, pagingInfo, keyword);
                        }
                        return sourceList;
                    }).ShowInList();
                View.Property(p => p.MaintainType).Show(ShowInWhere.Hide);
                View.Property(p => p.PrecisePlanBeginDate).Show().ShowInList(width: 143).Readonly(!IsMaintainForPrecisePlan);
                View.Property(p => p.PrecisePlanEndDate).Show().ShowInList(width: 143).Readonly(!IsMaintainForPrecisePlan);
                View.Property(p => p.MaintainTime).UseSpinEditor(s => { s.MinValue = 0; }).Show().ShowInList(width: 120);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.ProjectList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MaintainPlanAttachmentList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MaintainPlanSparePartList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.WorkHoursRegisterList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.ProjectList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MaintainConfirmationList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MaintainPlanSparePartAplList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 修改窗口保养明细列表视图
        /// </summary>
        protected void PlanEditListView()
        {

            bool IsMaintainForPrecisePlan = RT.Service.Resolve<MaintainController>().IsMaintainForPrecisePlan();
            View.DomainName("保养计划");
            View.UseClientOrder();
            using (View.OrderProperties())
            {
                View.Property(p => p.YearAndMonth).Show(ShowInWhere.All).UseDateEditor(d => d.Format = "y/m").Readonly();
                View.Property(p => p.Cycle).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipAccount).HasLabel("设备编码").Show(ShowInWhere.Hide);
                View.Property(p => p.PlanBeginDate).UseDateEditor(d => d.Format = "m/d").HasLabel(PlanStartDateLabel).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PlanEndDate).UseDateEditor(d => d.Format = "m/d").HasLabel(PlanEndDateLabel).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.MaintainTypeInfoId)
                    .UseDataSource((source, pagingInfo, keyword) =>
                 {
                     var sourceList = new EntityList<MaintainTypeInfo>();
                     MaintainPlan maintainPlan = source as MaintainPlan;

                     if (maintainPlan != null)
                     {
                         sourceList = RT.Service.Resolve<MaintainController>().GetMaintainTypeInfoList(maintainPlan.MaintainCycleType, pagingInfo, keyword);
                     }
                     return sourceList;
                 });

                View.Property(p => p.MaintainType).Show(ShowInWhere.All);
                View.Property(p => p.PrecisePlanBeginDate).Show().ShowInList(width: 143).Readonly(!IsMaintainForPrecisePlan);
                View.Property(p => p.PrecisePlanEndDate).Show().ShowInList(width: 143).Readonly(!IsMaintainForPrecisePlan);
                View.Property(p => p.MaintainTime).UseSpinEditor(s => { s.MinValue = 0; }).Show().ShowInList(width: 120);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.ProjectList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MaintainPlanAttachmentList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MaintainPlanSparePartList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.WorkHoursRegisterList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.ProjectList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MaintainConfirmationList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MaintainPlanSparePartAplList).Show(ChildShowInWhere.Hide);
            }
        }
        /// <summary>
        /// 批量添加保养计划窗口
        /// </summary>
        protected void BatchAddMaintainPlanView()
        {
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(3);
                View.Property(p => p.EquipMaintainType).Show().Cascade(p => p.ResourceId, null);

                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var resourcesList = RT.Service.Resolve<EnterpriseController>().GetLines(c, r, null);
                    resourcesList.ForEach(p => p.TreePId = null);
                    return resourcesList;
                }).UsePagingLookUpEditor((m, e) =>
                {
                    var keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ResourceName), nameof(e.Resource.Name));
                    m.DicLinkField = keyValues;
                }).Readonly(p => p.EquipMaintainType != EquipCheckType.Line).HasLabel("产线编码").Show(ShowInWhere.All);
                View.Property(p => p.ResourceName).Readonly().HasLabel("产线名称").Show(ShowInWhere.All);
                View.Property(p => p.PlanBeginDate).UseDateEditor(d => d.Format = ymdFormat).Show(ShowInWhere.All).HasLabel("计划区间".L10N()+"*");
                View.Property(p => p.PlanEndDate).UseDateEditor(d => d.Format = ymdFormat).Show(ShowInWhere.All).HasLabel("至".L10N()+"*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                View.Property(p => p.MaintainTime).UseSpinEditor(s => { s.MinValue = 0; s.AllowDecimals = true; s.AllowNegative = false; }).Show();
                View.Property(p => p.BeginDay).UseSpinEditor(s =>
                {
                    s.MinValue = 1;
                    s.MaxValue = 31;
                }).Show(ShowInWhere.Hide);
                View.Property(p => p.EndDay).UseSpinEditor(s =>
                {
                    s.MinValue = 1;
                    s.MaxValue = 31;
                }).Show(ShowInWhere.Hide);
                View.Property(p => p.MaintainCycleType).ShowInDetail().DefaultValue(MaintainCycleType.Week);

                View.AttachChildrenProperty(typeof(MaintainPlan), (e) =>
                {
                    var maintainPlan = e.Parent as MaintainPlan;
                    return maintainPlan;
                }, PlanAddList).HasLabel("保养计划").Show(ChildShowInWhere.All);

                View.AttachChildrenProperty(typeof(EquipAccountSelect), (e) =>
                {
                    return new EntityList<EquipAccountSelect>();
                }, EquipAccountSelectExtensionViewConfig.MaintainPlanBatchAddList)
                    .Show(ChildShowInWhere.All)
                    .HasLabel("设备台账")
                    .LazyLoad(false);
            }
        }
        /// <summary>
        /// 添加保养计划窗口
        /// </summary>
        protected void AddMaintainPlanView()
        {
            View.AddBehavior("SIE.Web.EMS.EquipMaint.Maintains.Plans.Scripts.AddMaintainPlanBehavior");
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(3);
                View.Property(p => p.EquipMaintainType).DefaultValue(0).Readonly().Show();
                View.Property(p => p.EquipAccount).UseDataSource((e, p, k) =>
                {
                    List<AccountUseState> useStates = new List<AccountUseState> { AccountUseState.Using, AccountUseState.Repair, AccountUseState.ToAccepted };
                    EntityList<EquipAccountSelect> list = RT.Service.Resolve<EquipAccountSelectController>().GetCheckPlanEquipAccounts(useStates, k, p);
                    return list;
                }).UsePagingLookUpEditor((m, e) =>
                {
                    var keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.MachineNo), nameof(e.EquipAccount.Name));
                    m.DicLinkField = keyValues;
                }).Show().UseFormSetting(x=>x.HelpInfo="只能选择【管理状态】为【使用中】的设备。");

                View.Property(p => p.MachineNo).Readonly().Show(ShowInWhere.All).HasLabel("设备名称");

                View.Property(p => p.PlanBeginDate).UseDateEditor(d => d.Format = ymdFormat).Show(ShowInWhere.All).HasLabel("计划区间*");

                View.Property(p => p.PlanEndDate).UseDateEditor(d => d.Format = ymdFormat).Show(ShowInWhere.All).HasLabel("至*&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");

                View.Property(p => p.MaintainTime).UseSpinEditor(s => { s.MinValue = 0; s.AllowDecimals = true; s.AllowNegative = false; }).Show();

                View.Property(p => p.BeginDay).UseSpinEditor(s =>
                {
                    s.MinValue = 1;
                    s.MaxValue = 31;
                }).Show(ShowInWhere.Hide);

                View.Property(p => p.EndDay).UseSpinEditor(s =>
                {
                    s.MinValue = 1;
                    s.MaxValue = 31;
                }).Show(ShowInWhere.Hide);

                View.Property(p => p.MaintainCycleType).ShowInDetail().DefaultValue(MaintainCycleType.Week).HasLabel("生成周期");

                View.AttachChildrenProperty(typeof(MaintainPlan), (e) =>
                {
                    var maintainPlan = e.Parent as MaintainPlan;
                    return maintainPlan;
                }, PlanAddList).HasLabel("保养计划").Show(ChildShowInWhere.All);
            }
        }

        /// <summary>
        /// 修改保养计划窗口
        /// </summary>
        protected void EditMaintainPlanView()
        {
            using (View.OrderProperties())
            {
                View.HasDetailColumnsCount(3);
                View.Property(p => p.EquipMaintainType).DefaultValue(0).Readonly().Show();
                View.Property(p => p.EquipAccountCode).Readonly().Show();
                View.Property(p => p.MachineNo).Readonly().Show(ShowInWhere.All).HasLabel("设备名称");
                View.Property(p => p.PlanBeginDate).Readonly().UseDateEditor(d =>
                {
                    d.Format = ymdFormat;
                    d.MinValue = DateTime.Now.ToString();
                }).Show(ShowInWhere.All).HasLabel("计划区间");
                View.Property(p => p.PlanEndDate).Readonly().UseDateEditor(d => d.Format = ymdFormat).Show(ShowInWhere.All).HasLabel("至&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                View.Property(p => p.MaintainTime).Readonly().Show();
                View.AttachChildrenProperty(typeof(MaintainPlan), (e) =>
                {
                    var maintainPlan = e.Parent as MaintainPlan;
                    return maintainPlan;
                }, PlanEditList).HasLabel("保养计划").Show(ChildShowInWhere.All);
            }
        }

        /// <summary>
        /// 配置执行保养计划视图
        /// </summary>
        public void PlanExecuteView()
        {
            View.AssignAuthorize(typeof(MaintainPlanViewModel));
            View.AddBehavior("SIE.Web.EMS.EquipMaint.Maintains.Plans.Scripts.ExeMaintainPlanBehavior");
            View.UseCommands(typeof(SaveExeMaintainPlanCommand).FullName, typeof(SubmitExeMaintainPlanCommand).FullName, typeof(AddEquipRepairCommand).FullName, typeof(BeginMaintainCommand).FullName);
            View.HasDetailColumnsCount(4);

            using (View.OrderProperties())
            {
                View.Property(p => p.MaintainNo).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipAccountCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipAccountName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipModelCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipModelName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipTypeCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipTypeName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.DepartmentId).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.ExeState).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PlanBeginDate).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.PlanEndDate).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.MaintainType).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SumWorkHours).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ActBeginDate).UseDateTimeEditor().Readonly(p => p.ExeState != SIE.EMS.Enums.MaintExeState.NotPerformed && p.ExeState != SIE.EMS.Enums.MaintExeState.Performing).Show(ShowInWhere.All).HasLabel("保养开始时间".L10N()+"*");
                View.Property(p => p.ActEndDate).UseDateTimeEditor().Readonly(p => p.ExeState != SIE.EMS.Enums.MaintExeState.NotPerformed && p.ExeState != SIE.EMS.Enums.MaintExeState.Performing).Show(ShowInWhere.All).HasLabel("保养结束时间".L10N() + "*");
                View.Property(p => p.ExecuteById).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.UpMaintainSummary).Show(ShowInWhere.All).ShowInDetail(columnSpan: 2, rowSpan: 1).Readonly();
                View.Property(p => p.ExeResult).Show(ShowInWhere.All).Readonly(p => p.ExeState != SIE.EMS.Enums.MaintExeState.NotPerformed && p.ExeState != SIE.EMS.Enums.MaintExeState.Performing).HasLabel("执行结果".L10N()+"*");
                View.Property(p => p.MaintainSummary).Show(ShowInWhere.All).ShowInDetail(columnSpan: 4, rowSpan: 1).Readonly(p => p.ExeState != SIE.EMS.Enums.MaintExeState.NotPerformed && p.ExeState != SIE.EMS.Enums.MaintExeState.Performing);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
            View.ChildrenProperty(p => p.ProjectList).Show(ChildShowInWhere.All).OrderNo = 1;
            View.AttachChildrenProperty(typeof(MaintainPlanSparePart), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainPlan>();
                if (parent == null)
                {
                    return new EntityList<MaintainPlanSparePart>();
                }
                var spareParts = RT.Service.Resolve<MaintainController>().GetMaintainPlanSpareParts(parent.Id, parent.MaintainNo, parent.EquipAccountId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return spareParts;
            }, directionParentPropertyName: MaintainPlan.MaintainPlanSparePartListProperty.Name).HasLabel("备件更换").Show(ChildShowInWhere.All).OrderNo = 2;
            View.AttachChildrenProperty(typeof(MaintainPlanSparePartApl), (w) =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainPlan>();
                if (parent == null)
                {
                    return new EntityList<MaintainPlanSparePartApl>();
                }
                var spareParts = RT.Service.Resolve<MaintainController>().GetMaintainPlanSparePartApls(parent.Id, parent.EquipAccountId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return spareParts;
            }, directionParentPropertyName: MaintainPlan.MaintainPlanSparePartAplListProperty.Name).HasLabel("备件申请").Show(ChildShowInWhere.All).OrderNo = 3;
            View.ChildrenProperty(p => p.MaintainPlanAttachmentList).Show(ChildShowInWhere.All).OrderNo = 4;
            View.ChildrenProperty(p => p.WorkHoursRegisterList).Show(ChildShowInWhere.All).OrderNo = 5;
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(4);

            using (View.OrderProperties())
            {
                View.Property(p => p.MaintainNo).Show(ShowInWhere.All);
                View.Property(p => p.EquipAccountCode).Show(ShowInWhere.All);
                View.Property(p => p.EquipAccountName).Show(ShowInWhere.All);
                View.Property(p => p.EquipModelCode).Show(ShowInWhere.All);
                View.Property(p => p.EquipModelName).Show(ShowInWhere.All);
                View.Property(p => p.EquipTypeCode).Show(ShowInWhere.All);
                View.Property(p => p.EquipTypeName).Show(ShowInWhere.All);
                View.Property(p => p.DepartmentId).Show(ShowInWhere.All);
                View.Property(p => p.PlanBeginDate).UseDateEditor(d => d.Format = ymdFormat).Show(ShowInWhere.All);
                View.Property(p => p.PlanEndDate).UseDateEditor(d => d.Format = ymdFormat).Show(ShowInWhere.All);
                View.Property(p => p.MaintainType).Show(ShowInWhere.All);
                View.Property(p => p.ExeState).Show(ShowInWhere.All);
                View.Property(p => p.ActBeginDate).UseDateTimeEditor().Show(ShowInWhere.All);
                View.Property(p => p.ActEndDate).UseDateTimeEditor().Show(ShowInWhere.All);
                View.Property(p => p.SumWorkHours).Show(ShowInWhere.All);
                View.Property(p => p.ExecuteById).Show(ShowInWhere.All);
                View.Property(p => p.ExeResult).Show(ShowInWhere.All);
                View.Property(p => p.UpMaintainSummary).Show(ShowInWhere.All).ShowInDetail(columnSpan: 2, rowSpan: 1);
                View.Property(p => p.MaintainSummary).Show(ShowInWhere.All).ShowInDetail(columnSpan: 4, rowSpan: 1);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
            View.ChildrenProperty(p => p.MaintainConfirmationList).Show(ChildShowInWhere.Hide);

            View.ChildrenProperty(p => p.ProjectList).Show(ChildShowInWhere.All).OrderNo = 1;
            View.ChildrenProperty(p => p.MaintainPlanSparePartList).Show(ChildShowInWhere.All).OrderNo = 2;
            View.ChildrenProperty(p => p.MaintainPlanAttachmentList).Show(ChildShowInWhere.All).OrderNo = 3;
            View.ChildrenProperty(p => p.WorkHoursRegisterList).Show(ChildShowInWhere.All).OrderNo = 4;
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.ExeState);
            View.Property(p => p.MaintainNo);
            View.Property(p => p.EquipAccount);
            View.Property(p => p.MachineNo);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.MaintainNo);
        }

        /// <summary>
        /// 配置保养视图
        /// </summary>
        public void ConfigPlanConfirmView()
        {
            View.AssignAuthorize(typeof(MaintainPlanViewModel));
            View.AddBehavior("SIE.Web.EMS.EquipMaint.Maintains.Confirmations.Scripts.ConfirmMaintainPlanBehavior");
            View.UseCommands("SIE.Web.EMS.EquipMaint.Maintains.Confirmations.Commands.SubmitMaintainConfirmationCommand");
            View.HasDetailColumnsCount(4);

            using (View.OrderProperties())
            {
                View.Property(p => p.MaintainNo).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipAccountCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipAccountName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipModelCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipModelName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipTypeCode).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.EquipTypeName).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ConfirmDeptDisplay).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SelectBeginTime).Show(ShowInWhere.All).Readonly().HasLabel(PlanStartDateLabel);
                View.Property(p => p.SelectEndTime).Show(ShowInWhere.All).Readonly().HasLabel(PlanEndDateLabel);
                View.Property(p => p.MaintainType).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ExeState).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ActBeginDate).UseDateTimeEditor().Readonly().Show(ShowInWhere.All).HasLabel("保养开始时间");
                View.Property(p => p.ActEndDate).UseDateTimeEditor().Readonly().Show(ShowInWhere.All).HasLabel("保养结束时间");
                View.Property(p => p.ExecuteById).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SumWorkHours).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.ExeResult).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.WhetherRepair).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.UpMaintainSummary).Show(ShowInWhere.All).ShowInDetail(columnSpan: 2, rowSpan: 1).Readonly().HasLabel("上次保养小结");
                View.Property(p => p.MaintainSummary).Show(ShowInWhere.All).ShowInDetail(columnSpan: 2, rowSpan: 1).Readonly().HasLabel("本次保养小结");

                View.Property(p => p.ConfirmResult).HasLabel("保养确认结果").UseEnumEditor().Show(ShowInWhere.All);
                View.Property(p => p.ConfirmNote).HasLabel("备注").UseTextEditor(p => p.AllowBlank = true).ShowInDetail(columnSpan: 3, rowSpan: 1);
            }
            View.AttachChildrenProperty(typeof(MaintainConfirmation), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainPlan>();
                if (parent == null)
                {
                    return new EntityList<MaintainConfirmation>();
                }

                var list = RT.Service.Resolve<MaintainController>().GetMaintainConfirmationList(parent.Id, parent.ConfirmDeptId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, MaintainConfirmationViewConfig.MaintainConfirmView).HasLabel("评分项").Show(ChildShowInWhere.All).HasOrderNo(0);

            View.AttachChildrenProperty(typeof(MaintainProject), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainPlan>();
                if (parent == null)
                {
                    return new EntityList<MaintainProject>();
                }

                var list = RT.Service.Resolve<MaintainController>().GetMaintainProjectList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, MaintainProjectViewConfig.MaintainConfirmationListView).HasLabel("保养项目").Show(ChildShowInWhere.All).HasOrderNo(1);

            View.AttachChildrenProperty(typeof(MaintainPlanSparePart), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainPlan>();
                if (parent == null)
                {
                    return new EntityList<MaintainPlanSparePart>();
                }

                var list = RT.Service.Resolve<MaintainController>().GetMaintainPlanSparePartList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, MaintainPlanSparePartViewConfig.MaintainConfirmationListView).HasLabel("备件更换").Show(ChildShowInWhere.All).HasOrderNo(2);

            View.AttachChildrenProperty(typeof(MaintainPlanSparePartApl), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainPlan>();
                if (parent == null)
                {
                    return new EntityList<MaintainPlanSparePartApl>();
                }
                var list = RT.Service.Resolve<MaintainController>().GetMaintainPlanSparePartAplList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, MaintainPlanSparePartAplViewConfig.MaintainConfirmationListView).HasLabel("备件申请").Show(ChildShowInWhere.All).HasOrderNo(3);

            View.AttachChildrenProperty(typeof(MaintainPlanAttachment), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainPlan>();
                if (parent == null)
                {
                    return new EntityList<MaintainPlanAttachment>();
                }

                var list = RT.Service.Resolve<MaintainController>().GetMaintainPlanAttachmentList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, MaintainPlanAttachmentViewConfig.MaintainConfirmationListView).HasLabel("执行图片").Show(ChildShowInWhere.All).HasOrderNo(4);

            View.AttachChildrenProperty(typeof(WorkHoursRegister), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainPlan>();
                if (parent == null)
                {
                    return new EntityList<WorkHoursRegister>();
                }
                var list = RT.Service.Resolve<MaintainController>().GetWorkHoursRegisterList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, WorkHoursRegisterViewConfig.MaintainConfirmationListView).HasLabel("工时登记").Show(ChildShowInWhere.All).HasOrderNo(5);

            
        }
    }
}
