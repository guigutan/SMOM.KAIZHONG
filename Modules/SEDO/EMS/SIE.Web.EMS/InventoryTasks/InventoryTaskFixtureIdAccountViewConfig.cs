using SIE.EMS.Enums;
using SIE.EMS.InventoryTasks;
using SIE.Equipments.Enums;
using SIE.MetaModel.View;
using SIE.Web.EMS.InventoryTasks.Commands;
using System;

namespace SIE.Web.EMS.InventoryTasks
{
    /// <summary>
    /// 工治具序列
    /// </summary>
    public class InventoryTaskFixtureIdAccountViewConfig : WebViewConfig<InventoryTaskFixtureIdAccount>
    {
        /// <summary>
        /// 平账界面
        /// </summary>
        public const string BalanceView = "BalanceView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BalanceView);
            if (ViewGroup == BalanceView)
            {
                ConfigBalanceView();
            }
        }

        /// <summary>
        /// 平账视图
        /// </summary>

        protected void ConfigBalanceView()
        {
            View.AddBehavior("SIE.Web.EMS.InventoryTasks.FixtureBlanceViewBehavior");
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection);
            View.Property(p => p.BalancingWay).ShowInList(80).Readonly(p => p.ApprovalStatus == ApprovalStatus.Audited);
            View.UseCommand("SIE.Web.EMS.InventoryTasks.Commands.FixtureInvSearchCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.FixtureEncode).ShowInList(120).Readonly();
                View.Property(p => p.Sn).ShowInList(120).HasLabel("序列号").Readonly();
                View.Property(p => p.FixtureStatus).ShowInList(120).Readonly();
                View.Property(p => p.FirstResult).ShowInList(80).Readonly();
                View.Property(p => p.FirstStatus).ShowInList(80).Readonly();

                View.Property(p => p.SecondResult).ShowInList(80).Readonly();
                View.Property(p => p.SecondStatus).ShowInList(80).Readonly();
                View.Property(p => p.WarehouseId).ShowInList(80).Readonly();
                View.Property(p => p.StorageLocationId).ShowInList(80).Readonly();
                View.Property(p => p.WorkshopId).ShowInList(80).Readonly();
                View.Property(p => p.ResourceId).ShowInList(80).Readonly();

                View.Property(p => p.InventoryAssetSource).ShowInList(80).Readonly();
                View.Property(p => p.FirstCounterId).ShowInList(80).Readonly();
                View.Property(p => p.FirstDateTime).ShowInList(80).Readonly();
                View.Property(p => p.SecondCounterId).ShowInList(80).Readonly();
                View.Property(p => p.SecondDateTime).ShowInList(120).Readonly();
            }
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.InventoryTasks.FixtureEncodeIDListBehavior");
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection,
                "SIE.Web.EMS.InventoryTasks.Commands.AddFixtureProfitCommand"
                , "SIE.Web.EMS.InventoryTasks.Commands.FixtureInvDeleteCommand"
                , "SIE.Web.EMS.InventoryTasks.Commands.FixtureInvSearchCommand"
                , typeof(OneKeyPassCommand).FullName
                );
            using (View.OrderProperties())
            {
                View.Property(p => p.InventoryStatus).ShowInList(120).Readonly();
                View.Property(p => p.FixtureEncode).ShowInList(120).Readonly();
                View.Property(p => p.Sn).ShowInList(120).HasLabel("序列号").Readonly();
                View.Property(p => p.FixtureStatus).ShowInList(120).Readonly();

                View.Property(p => p.FirstResult).ShowInList(80).Readonly(p => p.InventoryTaskStatus != InventoryTaskStatus.Doing || p.FirstPower == false || p.FirstResult == InventoryResult.Profit).HasLabel("初盘结果".L10N()+"*");
                View.Property(p => p.FirstStatus).ShowInList(80).Readonly(p => p.InventoryTaskStatus != InventoryTaskStatus.Doing || p.FirstPower == false || p.FirstResult != InventoryResult.InfoChange).HasLabel("初盘状态".L10N() + "*");

                View.Property(p => p.SecondResult).ShowInList(80)
                    .Readonly(p => p.InventoryTaskStatus == InventoryTaskStatus.NotBegin || p.InventoryTaskStatus == InventoryTaskStatus.Doing || p.InventoryTaskStatus == InventoryTaskStatus.Completed || p.SecondPower == false || p.SecondResult == InventoryResult.Profit);
                View.Property(p => p.SecondStatus).ShowInList(80)
                    .Readonly(p => p.InventoryTaskStatus == InventoryTaskStatus.NotBegin || p.InventoryTaskStatus == InventoryTaskStatus.Doing || p.InventoryTaskStatus == InventoryTaskStatus.Completed || p.SecondPower == false || p.SecondResult != InventoryResult.InfoChange);
                View.Property(p => p.WarehouseId).ShowInList(80).Readonly();
                View.Property(p => p.StorageLocationId).ShowInList(80).Readonly();
                View.Property(p => p.WorkshopId).ShowInList(80).Readonly();
                View.Property(p => p.ResourceId).ShowInList(80).Readonly();

                View.Property(p => p.InventoryAssetSource).ShowInList(80).Readonly();
                View.Property(p => p.FirstCounterId).ShowInList(80).Readonly();
                View.Property(p => p.FirstDateTime).ShowInList(80).Readonly();

                View.Property(p => p.SecondCounterId).ShowInList(80).Readonly();
                View.Property(p => p.SecondDateTime).ShowInList(120).Readonly();
            }
        }

        /// <summary>
        /// 配置导入界面信息
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.InventoryTask.TaskNo).HasLabel("盘点任务单号");
            View.PropertyRef(p => p.FixtureEncode.Code).HasLabel("工治具编码");
            View.Property(p => p.Sn).HasLabel("序列号");
            View.Property(p => p.PassQty).HasLabel("实盘合格数");
            View.Property(p => p.NgQty).HasLabel("实盘不合格数");
            View.Property(p => p.OnlineQty).HasLabel("实盘在线数");
        }
    }
}
