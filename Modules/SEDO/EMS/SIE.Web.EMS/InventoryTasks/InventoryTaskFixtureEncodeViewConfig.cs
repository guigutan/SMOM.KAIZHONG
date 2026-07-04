using SIE.EMS.Enums;
using SIE.EMS.InventoryTasks;
using SIE.Equipments.Enums;
using SIE.Fixtures;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SIE.Web.EMS.InventoryTasks
{
    /// <summary>
    /// 编码列表
    /// </summary>
    public class InventoryTaskFixtureEncodeViewConfig : WebViewConfig<InventoryTaskFixtureEncode>
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
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll,WebCommandNames.ExportXlsSelection);
            View.UseCommand("SIE.Web.EMS.InventoryTasks.Commands.FixtureInvSearchCommand");
            View.AddBehavior("SIE.Web.EMS.InventoryTasks.FixtureBlanceViewBehavior");
            using (View.OrderProperties())
            {
                View.Property(p => p.BalancingWay).HasLabel("平账方式".L10N()+"*").ShowInList(80).Readonly(p => p.ApprovalStatus == ApprovalStatus.Audited);
                View.Property(p => p.FirstResult).ShowInList(80).Readonly();
                View.Property(p => p.SecondResult).ShowInList(80).Readonly();
                View.Property(p => p.FixtureEncodeId).ShowInList(100).Readonly();
                View.Property(p => p.StockQty).ShowInList(100).Readonly();
                View.Property(p => p.StockPassQty).ShowInList(100).Readonly();
                View.Property(p => p.StockNgQty).ShowInList(100).Readonly();

                View.Property(p => p.Online).ShowInList(80).Readonly();
                View.Property(p => p.Total).ShowInList(80).Readonly();
                View.Property(p => p.FirstStock).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.FirstStockPassQty).ShowInList(100).Readonly();
                View.Property(p => p.FirstStockNgQty).ShowInList(100).Readonly();
                View.Property(p => p.FirstOnline).ShowInList(100).Readonly();

                View.Property(p => p.FirstTotal).ShowInList(80).Readonly();
                View.Property(p => p.FirstDiff).ShowInList(100).Readonly();
                View.Property(p => p.SecondStock).Show(ShowInWhere.Hide).Readonly();
                View.Property(p => p.SecStockPassQty).ShowInList(100).Readonly();
                View.Property(p => p.SecStockNgQty).ShowInList(100).Readonly();

                View.Property(p => p.SecondOnline).ShowInList(120).Readonly();
                View.Property(p => p.SecondTotal).ShowInList(80).Readonly();
                View.Property(p => p.SecondDiff).ShowInList(100).Readonly();

                View.Property(p => p.ModelCode).ShowInList(100).Readonly();
                View.Property(p => p.ModelName).ShowInList(100).Readonly();
                View.Property(p => p.FixtureType).ShowInList(100).Readonly();
                View.Property(p => p.ManageMode).ShowInList(100).Readonly();
                View.Property(p => p.Unit).ShowInList(60).Readonly();

                View.Property(p => p.InventoryAssetSource).ShowInList(120).Readonly();
                View.Property(p => p.FirstCounterId).ShowInList(80).Readonly();
                View.Property(p => p.InventoryDateTime).ShowInList(80).Readonly();
                View.Property(p => p.SecondCounterId).ShowInList(80).Readonly();
                View.Property(p => p.SecondDateTime).ShowInList(80).Readonly();
            }
        }

        /// <summary>
        /// 配置列表
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.InventoryTasks.FixtureEncodeListBehavior");
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, WebCommandNames.ExportXlsSelection,"SIE.Web.EMS.InventoryTasks.Commands.AddFixtureProfitCommand","SIE.Web.EMS.InventoryTasks.Commands.FixtureInvDeleteCommand","SIE.Web.EMS.InventoryTasks.Commands.FixtureInvSearchCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.InventoryStatus).ShowInList(80).Readonly();
                View.Property(p => p.FirstResult).ShowInList(80).Readonly().HasLabel("初盘结果".L10N()+"*");
                View.Property(p => p.SecondResult).ShowInList(80).Readonly();
                View.Property(p => p.FixtureEncodeId).ShowInList(100).Readonly();
                View.Property(p => p.StockQty).ShowInList(100).Readonly();
                View.Property(p => p.StockPassQty).ShowInList(100).Readonly();
                View.Property(p => p.StockNgQty).ShowInList(100).Readonly();

                View.Property(p => p.Online).ShowInList(80).Readonly();
                View.Property(p => p.Total).ShowInList(80).Readonly();
                View.Property(p => p.FirstStock).Show(ShowInWhere.Hide).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; })
                    .Readonly(p => p.InventoryTaskStatus != InventoryTaskStatus.Doing || p.FirstPower == false || p.ManageMode == ManageMode.Number);
                View.Property(p => p.FirstStockPassQty).ShowInList(100).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; })
                  .Readonly(p => p.InventoryTaskStatus != InventoryTaskStatus.Doing || p.FirstPower == false || p.ManageMode == ManageMode.Number).HasLabel("初盘在库合格数".L10N() + "*");

                View.Property(p => p.FirstStockNgQty).ShowInList(100).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; })
                  .Readonly(p => p.InventoryTaskStatus != InventoryTaskStatus.Doing || p.FirstPower == false || p.ManageMode == ManageMode.Number).HasLabel("初盘在库不合格数".L10N() + "*");


                View.Property(p => p.FirstOnline).ShowInList(100).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; })
                    .Readonly(p => p.InventoryTaskStatus != InventoryTaskStatus.Doing || p.FirstPower == false || p.ManageMode == ManageMode.Number).HasLabel("初盘在线数".L10N() + "*");

                View.Property(p => p.FirstTotal).ShowInList(80).Readonly().UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; });
                View.Property(p => p.FirstDiff).ShowInList(100).Readonly().UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; });

                View.Property(p => p.SecondStock).Show(ShowInWhere.Hide).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; })
                    .Readonly(p => (p.InventoryTaskStatus == InventoryTaskStatus.NotBegin || p.InventoryTaskStatus == InventoryTaskStatus.Completed
                    || p.InventoryTaskStatus == InventoryTaskStatus.Doing)
                    || p.SecondPower == false || p.ManageMode == ManageMode.Number);

                View.Property(p => p.SecStockPassQty).ShowInList(100).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; })
              .Readonly(p => (p.InventoryTaskStatus == InventoryTaskStatus.NotBegin || p.InventoryTaskStatus == InventoryTaskStatus.Completed
              || p.InventoryTaskStatus == InventoryTaskStatus.Doing)
              || p.SecondPower == false || p.ManageMode == ManageMode.Number);


                View.Property(p => p.SecStockNgQty).ShowInList(100).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; })
              .Readonly(p => (p.InventoryTaskStatus == InventoryTaskStatus.NotBegin || p.InventoryTaskStatus == InventoryTaskStatus.Completed
              || p.InventoryTaskStatus == InventoryTaskStatus.Doing)
              || p.SecondPower == false || p.ManageMode == ManageMode.Number);

                View.Property(p => p.SecondOnline).ShowInList(120).UseSpinEditor(p => { p.MinValue = 0; p.AllowDecimals = false; })
                    .Readonly(p => (p.InventoryTaskStatus == InventoryTaskStatus.NotBegin || p.InventoryTaskStatus == InventoryTaskStatus.Completed
                    || p.InventoryTaskStatus == InventoryTaskStatus.Doing)
                    || p.SecondPower == false || p.ManageMode == ManageMode.Number);

                View.Property(p => p.SecondTotal).ShowInList(80).Readonly();
                View.Property(p => p.SecondDiff).ShowInList(100).Readonly();

                View.Property(p => p.ModelCode).ShowInList(100).Readonly();
                View.Property(p => p.ModelName).ShowInList(100).Readonly();
                View.Property(p => p.FixtureType).ShowInList(100).Readonly();
                View.Property(p => p.ManageMode).ShowInList(100).Readonly();
                View.Property(p => p.Unit).ShowInList(60).Readonly();

                View.Property(p => p.InventoryAssetSource).ShowInList(120).Readonly();
                View.Property(p => p.FirstCounterId).ShowInList(80).Readonly();
                View.Property(p => p.InventoryDateTime).ShowInList(80).Readonly();
                View.Property(p => p.SecondCounterId).ShowInList(80).Readonly();
                View.Property(p => p.SecondDateTime).ShowInList(80).Readonly();
            }
        }


        //Balancing
        /// <summary>
        /// 配置导入
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
