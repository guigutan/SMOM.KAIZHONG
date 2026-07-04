using SIE.Inventory.Task;
using SIE.ManagedProperty;
using SIE.MetaModel.View;
using SIE.Web.Inventory.Task.Commands;

namespace SIE.Web.Inventory.Task
{
    /// <summary>
    /// 任务管理视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class TaskManagementViewConfig : WebViewConfig<TaskManagement>
    {
        /// <summary>
        ///  扩展附加视图
        /// </summary>
        public const string TaskManagementAttachView = "TaskManagementAttachViewGroup";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { TaskManagementAttachView });

            if (ViewGroup == TaskManagementAttachView)
            {
                AttachTaskManagementView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(typeof(EditTaskManagementCommand).FullName, typeof(SaveTaskManagementCommand).FullName);
            View.UseCommands(typeof(ReleaseTaskManagementCommand).FullName, typeof(FrozenTaskManagementCommand).FullName, typeof(UrgentLevelCommand).FullName, typeof(HighLevelCommand).FullName, typeof(MiddleLevelCommand).FullName, typeof(LowLevelCommand).FullName);
            View.UseCommands(typeof(ViewTaskManagementCommand).FullName, typeof(PrintTaskManagementCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.ReleaseDate).Readonly(p => p.State != TaskState.Release && p.State != TaskState.Appoint && p.State != TaskState.Create)
                    .UseListSetting(e => { e.HelpInfo = "状态为释放/指定/创建可编辑"; }).ShowInList(150);
                View.Property(p => p.TaskGroupNo).Readonly();
                View.Property(p => p.No).Readonly()
                    .UseListSetting(e => { e.HelpInfo = string.Format("根据{0}(配置项--{0})生成{1}任务号", "单号生成规则", "任务管理"); }).ShowInList(150);
                View.Property(p => p.BillNo).Readonly().ShowInList(150);
                View.Property(p => p.State).Readonly();
                View.Property(p => p.StateDesc);
                View.Property(p => p.Level).Readonly();
                View.Property(p => p.OperationRequires);
                View.Property(p => p.OperationType).Readonly();
                View.Property(p => p.GetById).Readonly();
                View.Property(p => p.ItemCode).Readonly().ShowInList(150);
                View.Property(p => p.ItemName).Readonly().ShowInList(150);
                View.Property(p => p.ItemSpecificationModel).Readonly();
                View.Property(p => p.ItemExtPropName).Readonly().ShowInList(width: 180);
                View.Property(p => p.Qty).Readonly();
                View.Property(p => p.ActualQty).Readonly();
                View.Property(p => p.ItemUnitName).Readonly().HasLabel("单位");
                View.Property(p => p.SuggestLotId).Readonly().ShowInList(150);
                View.Property(p => p.ActualLotId).Readonly().ShowInList(150);
                View.Property(p => p.FromWarehouseId).Readonly();
                View.Property(p => p.FromAreaId).Readonly();
                View.Property(p => p.SuggestFromLocCode).Readonly().HasLabel("建议来源库位编码").ShowInList(150);
                View.Property(p => p.SuggestFromLocName).Readonly().HasLabel("建议来源库位名称").ShowInList(150);
                View.Property(p => p.SuggestFromLpn).Readonly();
                View.Property(p => p.ActualFromLocCode).Readonly().HasLabel("实际来源库位编码").ShowInList(150);
                View.Property(p => p.ActualFromLocName).Readonly().HasLabel("实际来源库位名称").ShowInList(150);
                View.Property(p => p.ActualFromLpn).Readonly();
                View.Property(p => p.ToWarehouseId).Readonly();
                View.Property(p => p.SuggestToLocCode).Readonly().HasLabel("建议目标库位编码").ShowInList(150);
                View.Property(p => p.SuggestToLocName).Readonly().HasLabel("建议目标库位名称").ShowInList(150);
                View.Property(p => p.SuggestToLpn).Readonly();
                View.Property(p => p.ActualToLocCode).Readonly().HasLabel("实际目标库位编码").ShowInList(150);
                View.Property(p => p.ActualToLocName).Readonly().HasLabel("实际目标库位名称").ShowInList(150);
                View.Property(p => p.ActualToLpn).Readonly();
                View.Property(p => p.SuggestFromStation).Readonly();
                View.Property(p => p.ActualFromStation).Readonly();
                View.Property(p => p.SuggestToStation).Readonly();
                View.Property(p => p.ActualToStaion).Readonly();
                View.Property(p => p.StorerCode);
                View.Property(p => p.ProjectNo).Readonly();
                View.Property(p => p.TaskNo).Readonly();
                View.Property(p => p.OnhandState).Readonly();
                View.Property(p => p.BillDtlNo).Readonly();
                View.Property(p => p.SecondBillDtlNo).HasLabel("二级明细行号").Readonly();
                View.Property(p => p.BeginDate).Readonly().ShowInList(150);
                View.Property(p => p.EndDate).Readonly().ShowInList(150);
                View.Property(p => p.LengthTime).Readonly();
                View.Property(p => p.CloseById).Readonly().ShowInList(150);
                View.Property(p => p.CloseDate).Readonly().ShowInList(150);
                View.Property(p => p.FrozenById).Readonly().ShowInList(150);
                View.Property(p => p.FrozenDate).Readonly().ShowInList(150);
                View.Property(p => p.OrderType).Readonly();
                View.Property(p => p.TransactionName).HasLabel("单据小类");
                View.Property(p => p.TransactionType).Readonly();
                View.Property(p => p.SowType).Readonly();
                View.Property(p => p.RelationOrderNo).Readonly();
            }

            View.ChildrenProperty(p => p.OperatorList).HasLabel("指定操作人");
            View.ChildrenProperty(p => p.ActualOperatorList).HasLabel("实际操作人");
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.IsHorizontalGroup(false);
            using (View.OrderProperties())
            {
                using (View.DeclareGroup("任务基本信息", 4, true))
                {
                    View.Property(p => p.No).Readonly();
                    View.Property(p => p.Level).Readonly();
                    View.Property(p => p.State).Readonly();
                    View.Property(p => p.StateDesc).Readonly();
                    View.Property(p => p.ReleaseDate).Readonly();
                    View.Property(p => p.OperationRequires).Readonly();
                    View.Property(p => p.GetBy).Readonly();
                    View.Property(p => p.BeginDate).Readonly();
                    View.Property(p => p.EndDate).Readonly();
                    View.Property(p => p.LengthTime).Readonly();
                }

                using (View.DeclareGroup("物料基本信息", 4, true))
                {
                    View.Property(p => p.ItemCode);
                    View.Property(p => p.ItemName);
                    View.Property(p => p.ItemSpecificationModel);
                    View.Property(p => p.ItemUnitName);
                    View.Property(p => p.StorerCode).Readonly();
                    View.Property(p => p.ProjectNo).Readonly();
                    View.Property(p => p.OperationType).Readonly();
                    View.Property(p => p.BillNo).Readonly();
                }

                using (View.DeclareGroup("物料操作信息", 5, true))
                {
                    View.Property(p => p.SuggestLot).ShowInDetail(columnSpan: 2).Readonly();
                    View.Property(p => p.Qty).Readonly();
                    View.Property(p => p.ActualLot).Readonly();
                    View.Property(p => p.ActualQty).Readonly();

                    View.Property(p => p.FromWarehouse).Readonly();
                    View.Property(p => p.SuggestFromLoc).Readonly();
                    View.Property(p => p.SuggestFromLpn).Readonly();
                    View.Property(p => p.ActualFromLoc).Readonly();
                    View.Property(p => p.ActualFromLpn).Readonly();

                    View.Property(p => p.ToWarehouse).Readonly();
                    View.Property(p => p.SuggestToLoc).Readonly();
                    View.Property(p => p.SuggestToLpn).Readonly();
                    View.Property(p => p.ActualToLoc).Readonly();
                    View.Property(p => p.ActualToLpn).Readonly();
                }
            }
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.No);
            View.Property(p => p.State);
            View.Property(p => p.Level);
            View.Property(p => p.OperationType);
            View.Property(p => p.BillNo);
            View.Property(p => p.FromWarehouse);
            View.Property(p => p.ToWarehouse);
            View.Property(p => p.Item);
            View.Property(p => p.ReleaseDate).UseDateEditor();
        }

        /// <summary>
        /// 配置扩展附加视图
        /// </summary>
        protected void AttachTaskManagementView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.ReleaseDate).Show();
                View.Property(p => p.No).Show();
                View.Property(p => p.BillNo).Show();
                View.Property(p => p.State).Show();
                View.Property(p => p.StateDesc).Show();
                View.Property(p => p.Level).Show();
                View.Property(p => p.OperationRequires).Show();
                View.Property(p => p.OperationType).Show();
                View.Property(p => p.GetById).Show();
                View.Property(p => p.ItemCode).HasLabel("物料编码").Show();
                View.Property(p => p.ItemName).HasLabel("物料名称").Show();
                View.Property(p => p.ItemExtPropName).ShowInList(180);
                View.Property(p => p.ItemSpecificationModel).HasLabel("规格型号").Show();
                View.Property(p => p.Qty).Show();
                View.Property(p => p.ActualQty).Show();
                View.Property(p => p.ItemUnitName).HasLabel("单位").Show();
                View.Property(p => p.SuggestLotId).Show();
                View.Property(p => p.ActualLotId).Show();
                View.Property(p => p.FromWarehouseId).Show();
                View.Property(p => p.SuggestFromLocCode).HasLabel("建议来源库位编码").Show();
                View.Property(p => p.SuggestFromLocName).HasLabel("建议来源库位名称").Show();
                View.Property(p => p.SuggestFromLpn).Show();
                View.Property(p => p.ActualFromLocCode).HasLabel("实际来源库位编码").Show();
                View.Property(p => p.ActualFromLocName).HasLabel("实际来源库位名称").Show();
                View.Property(p => p.ActualFromLpn).Show();
                View.Property(p => p.ToWarehouseId).Show();
                View.Property(p => p.SuggestToLocCode).HasLabel("建议目标库位编码").Show();
                View.Property(p => p.SuggestToLocName).HasLabel("建议目标库位名称").Show();
                View.Property(p => p.SuggestToLpn).Show();
                View.Property(p => p.ActualToLocCode).HasLabel("实际目标库位编码").Show();
                View.Property(p => p.ActualToLocName).HasLabel("实际目标库位名称").Show();
                View.Property(p => p.ActualToLpn).Show();
                View.Property(p => p.StorerCode).Show();
                View.Property(p => p.ProjectNo).Show();
                View.Property(p => p.TaskNo).Readonly().Show();
                View.Property(p => p.OnhandState).Readonly().Show();
                View.Property(p => p.BillDtlNo).Show();
                View.Property(p => p.SecondBillDtlNo).HasLabel("二级明细行号").Readonly();
                View.Property(p => p.BeginDate).Show();
                View.Property(p => p.EndDate).Show();
                View.Property(p => p.LengthTime).Show();
                View.Property(p => p.CloseById).Show();
                View.Property(p => p.CloseDate).Show();
                View.Property(p => p.FrozenById).Show();
                View.Property(p => p.FrozenDate).Show();
                View.Property(p => p.OrderType).Show();
                View.Property(p => p.TransactionName).HasLabel("单据小类").Show();
                View.Property(p => p.TransactionType).Show();
                View.ChildrenProperty(p => p.OperatorList).Visible(false);
                View.ChildrenProperty(p => p.ActualOperatorList).Visible(false);
            }
        }
    }
}
