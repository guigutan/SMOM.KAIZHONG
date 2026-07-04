using SIE.Core.Equipments;
using SIE.Domain;
using SIE.Items.Items;
using SIE.ManagedProperty;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.Dispatchs.ViewModels;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.Reports;
using SIE.MetaModel.View;
using SIE.Utils;
using SIE.Web.MES.TaskManagement.Dispatchs.Commands;
using System;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 派工任务视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class DispatchTaskViewConfig : WebViewConfig<DispatchTask>
    {
        //private static EntityList<ParentItem> parentItems = null;
        /// <summary>
        /// 工单列表任务单视图
        /// </summary>
        public const string workOrderTaskView = "WorkOrderTaskView";

        #region 可派工数量 CanSendQty
        /// <summary>
        /// 可派工数量
        /// </summary>
        public static readonly Property<decimal> CanSendQtyProperty = P<DispatchTask>.RegisterExtensionReadOnly("CanSendQty", typeof(DispatchTaskViewConfig),
            GetCanSendQty, DispatchTask.DispatchQtyProperty);

        /// <summary>
        /// 可派工数量
        /// </summary>
        /// <param name="me">DispatchTask</param>
        /// <returns>decimal</returns>
        public static decimal GetCanSendQty(DispatchTask me)
        {
            return me.DispatchQty - me.SendQty;
        }
        #endregion

        #region 显示优先级 DisplayPriority
        /// <summary>
        /// 显示优先级
        /// </summary>
        public static readonly Property<string> DisplayPriorityProperty = P<DispatchTask>.RegisterExtensionReadOnly("DisplayPriority", typeof(DispatchTaskViewConfig),
            GetDisplayPriority, DispatchTask.PriorityProperty, DispatchTask.PriorityProperty);

        /// <summary>
        /// 显示优先级
        /// </summary>
        /// <param name="me">任务单</param>
        /// <returns>优先级</returns>
        public static string GetDisplayPriority(DispatchTask me)
        {
            return EnumViewModel.EnumToLabel(me.Priority).L10N();
        }
        #endregion

        #region 显示状态 DisplayState
        /// <summary>
        /// 显示状态  用一个字段显示工单状态和排产状态，工单状态有值时显示工单状态，排产状态有值时显示排产状态
        /// </summary>
        public static readonly Property<string> DisplayStateProperty = P<DispatchTask>.RegisterExtensionReadOnly("DisplayState", typeof(DispatchTaskViewConfig),
            GetDisplayState, DispatchTask.WorkOrderProperty);

        /// <summary>
        /// 显示状态
        /// </summary>
        /// <param name="me">工单</param>
        /// <returns>工单状态/排产状态</returns>
        public static string GetDisplayState(DispatchTask me)
        {
            var wo = me.WorkOrder;
            if (wo != null)
            {
                var state = wo.State;
                if (me.IsPause == YesNo.Yes && (state == SIE.Core.WorkOrders.WorkOrderState.Release || state == SIE.Core.WorkOrders.WorkOrderState.Producing))
                    return state.ToLabel().L10N() + " " + "暂停".L10N();
                else
                    return state.ToLabel().L10N();
            }
            else
                return string.Empty;
        }
        #endregion

        #region 饼重 PieWight
        /// <summary>
        /// 饼重
        /// </summary>
        public static readonly Property<string> PieWightProperty = P<DispatchTask>.RegisterExtensionReadOnly("PieWight", typeof(DispatchTaskViewConfig),
            GetPieWight, DispatchTask.ItemWeightProperty, DispatchTask.WeightUnitProperty);

        /// <summary>
        /// 饼重
        /// </summary>
        /// <param name="me">任务单</param>
        /// <returns>饼重</returns>
        public static string GetPieWight(DispatchTask me)
        {
            if (me.ItemWeight == "")
                return "0" + me.WeightUnit;
            else if (me.ItemWeight == null)
                return "0" + me.WeightUnit;
            return me.ItemWeight + me.WeightUnit;
        }
        #endregion

        #region 父旧物料号 ParShortDescription
        ///// <summary>
        ///// 父旧物料号
        ///// </summary>
        //public static readonly Property<string> ParShortDescriptionProperty = P<DispatchTask>.RegisterExtensionReadOnly("ParShortDescription", typeof(DispatchTaskViewConfig),
        //    GetParShortDescription, DispatchTask.ProductIdProperty);

        ///// <summary>
        ///// 父旧物料号
        ///// </summary>
        ///// <param name="me">任务单</param>
        ///// <returns>优先级</returns>
        //public static string GetParShortDescription(DispatchTask me)
        //{
        //    if (parentItems == null)
        //        parentItems = RF.GetAll<ParentItem>();  //如果数据量大,需要考虑其他方式处理;
        //    return parentItems.FirstOrDefault(p => p.ItemId == me.ProductId)?.Bismt;
        //}
        #endregion

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            //parentItems = null;
            View.DeclareExtendViewGroup(workOrderTaskView);
            if (ViewGroup == workOrderTaskView)
                WorkOrderTaskView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.MES.TaskManagement.Dispatchs.Scripts.DispatchTaskBehavior");
            View.UseCommands(typeof(ResumeTaskCommand).FullName, typeof(PauseTaskCommand).FullName,
                typeof(SchedulingInfReturnCommand).FullName, typeof(OpenFeedingCloseCommand).FullName, typeof(DispatchTaskCommand).FullName, typeof(TaskFinishOrOpenCommand).FullName,
                //typeof(CloseTaskCommand).FullName,
                typeof(CancelDispatchTaskCommand).FullName, WebCommandNames.Save,
                //typeof(MergeTaskCommand).FullName, typeof(CancelMergeTaskCommand).FullName,
                typeof(SplitTaskCommand).FullName, typeof(SetNormalCommand).FullName, typeof(SetUrgentCommand).FullName,
                typeof(PrintDispatchTaskCommand).FullName,
                //typeof(TaskDeleteCommand).FullName,
                WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.UseGridSelectionModel();
            View.Property(p => p.No).FixColumn().ShowInList(width: 150).Readonly();
            View.Property(p => p.DispatchQty).Readonly();
            View.Property(p => p.ReportQty).Readonly();
            View.Property(p => p.OkQty).Readonly();
            View.Property(p => p.NgQty).Readonly();
            View.Property(p => p.ReworkQty).Readonly();
            View.Property(p => p.SuspectQty).Readonly();
            View.Property(p => p.ExcessReportRatio).ShowInList().Readonly().HasLabel("超额比例");
            View.Property(p => p.ExcessReportQty).ShowInList().Readonly().HasLabel("超额数量");

            View.Property(p => p.TaskStatus).Readonly();
            View.Property(DisplayPriorityProperty).HasLabel("优先级");
            View.Property(p => p.TaskPerformer).Readonly();
            View.Property(p => p.PlanBeginTime);
            View.Property(p => p.PlanEndTime);
            View.Property(p => p.AssociatedWorkOrder).Readonly();
            View.Property(DisplayStateProperty).Readonly().HasLabel("工单状态");
            View.Property(p => p.ProductCode).Readonly();
            View.Property(p => p.ProductName).Readonly();
            View.Property(p => p.ShortDescription).Readonly();
            View.Property(PieWightProperty).Readonly().HasLabel("单重");
            //View.Property(ParShortDescriptionProperty).Readonly().HasLabel("父旧物料号");
            View.Property(p => p.ParShortDescription).Readonly().Show();
            View.Property(p => p.Fevor).Readonly();
            View.Property(p => p.Seq).Readonly();
            View.Property(p => p.ProcessCode).Readonly();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.IsNeedEquipment).Readonly();
            View.Property(p => p.StartProcess).Show().Readonly();
            View.Property(p => p.EquipAccountId).UseDataSource((e, p, s) =>
            {
                var entity = e as DispatchTask;
                if (entity != null)
                    return RT.Service.Resolve<DispatchController>().GetEquipAccounts(entity.ProductId, p, s);
                return new EntityList<EquipAccount>();
            });
            View.Property(p => p.SpecificationCode).Readonly();
            View.Property(p => p.SpecificationName).Readonly();
            View.Property(p => p.IsVirtualPart).Readonly();
            View.Property(p => p.VirtualPartCode).Readonly();
            View.Property(p => p.VirtualPartName).Readonly();
            View.Property(p => p.ProcessStandardHour).ShowInList(width: 150).Readonly();
            View.Property(p => p.ProcessHourSum).ShowInList(width: 150).Readonly();
            View.Property(p => p.ExpectedProductionTime).ShowInList(width: 150).Readonly();
            View.Property(p => p.ReportMode).Readonly();
            View.Property(p => p.WorkShopCode).Readonly();
            View.Property(p => p.WorkShopName).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.ResourceCode).Readonly();
            View.Property(p => p.ResourceSourceType).Readonly();
            View.Property(p => p.IsSyntype).Readonly();
            View.Property(p => p.TechNo).Readonly();
            View.Property(p => p.Classes).Readonly();
            View.Property(p => p.SourceType).Readonly();
            View.Property(p => p.ImportTime).Readonly();
            View.Property(p => p.IsFeedingClose).Show().Readonly();
            View.Property(p => p.IsSchedulingInfReturn).Show().Readonly();
            View.Property(p => p.SchedulingInfReturnReason).Show().Readonly();
            View.Property(p => p.IsOutsourcing).Show().Readonly();
            View.ChildrenProperty(p => p.Details).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.TaskList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.Boms).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.Records).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.OptLogList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.TransferLabels).Show(ChildShowInWhere.Hide);
            View.AttachChildrenProperty(typeof(AssociatedTask), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var dispatchTask = args.Parent.CastTo<DispatchTask>();
                if (dispatchTask == null)
                    return new EntityList<AssociatedTask>();
                return RT.Service.Resolve<DispatchController>().GetAssociatedDispatchTaskList(dispatchTask.Id);
            }).Show(ChildShowInWhere.List).HasLabel("关联任务单").HasOrderNo(20);
            View.AttachChildrenProperty(typeof(TaskProcessBom), (e) =>
            {
                var entity = e.Parent as DispatchTask;
                if (entity == null)
                {
                    return new EntityList<TaskProcessBom>();
                }
                return RT.Service.Resolve<ReportController>().GetTaskProcessBoms(entity.Id);
            }, TaskProcessBomViewConfig.reportDispatchView).Show(ChildShowInWhere.Hide).HasLabel("工序BOM").OrderNo = 30;
            View.AttachChildrenProperty(typeof(ResourcesTasksViewModel), (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var entity = args.Parent.CastTo<DispatchTask>();
                if (entity == null)
                {
                    return new EntityList<ResourcesTasksViewModel>();
                }
                return RT.Service.Resolve<ReportController>().GetResourcesTasksViewModels(args.SortInfo, args.PagingInfo, entity.Id);
            }).Show(ChildShowInWhere.List).HasLabel("已排资源任务").OrderNo = 40;


        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }

        /// <summary>
        /// 工单列表任务单视图
        /// </summary>
        public void WorkOrderTaskView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.No).ShowInList(120).Readonly();
                View.Property(p => p.DispatchQty).ShowInList().Readonly();
                View.Property(CanSendQtyProperty).ShowInList().Readonly().HasLabel("可派工数量");
                View.Property(p => p.SendQty).ShowInList().Readonly();
                View.Property(p => p.ReportQty).ShowInList().Readonly();
                View.Property(p => p.ReportQty).Readonly();
                View.Property(p => p.OkQty).Readonly();
                View.Property(p => p.NgQty).Readonly();
                View.Property(p => p.ReworkQty).Readonly();
                View.Property(p => p.SuspectQty).Readonly();
                View.Property(p => p.ExcessReportRatio).ShowInList().Readonly().HasLabel("超额比例");
                View.Property(p => p.ExcessReportQty).ShowInList().Readonly().HasLabel("超额数量");

                View.Property(p => p.TaskStatus).UseEnumEditor().ShowInList().Readonly().HasLabel("任务状态");
                View.Property(p => p.Process).ShowInList().Readonly().HasLabel("关联工序");
                View.Property(p => p.SpecificationCode).ShowInList().Readonly();
                View.Property(p => p.SpecificationName).ShowInList().Readonly();
                View.Property(p => p.ProcessStandardHour).ShowInList(width: 150).Readonly();
                View.Property(p => p.ProcessHourSum).ShowInList(width: 150).Readonly();
                View.Property(p => p.ExpectedProductionTime).ShowInList(width: 150).Readonly();

                View.Property(p => p.IsVirtualPart).ShowInList().Readonly();
                View.Property(p => p.VirtualPartCode).ShowInList().Readonly();
                View.Property(p => p.VirtualPartName).ShowInList().Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.IsFeedingClose).Show().Readonly();
                View.ChildrenProperty(p => p.Details).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.Boms).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.Records).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.TaskList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.OptLogList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.TransferLabels).Show(ChildShowInWhere.Hide);
            }
        }
    }
}