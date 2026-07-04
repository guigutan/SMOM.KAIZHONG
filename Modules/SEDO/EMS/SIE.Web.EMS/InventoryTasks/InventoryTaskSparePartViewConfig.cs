using SIE.EMS.InventoryBalances;
using SIE.EMS.InventoryTasks;
using System;

namespace SIE.Web.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务设备清单界面
    /// </summary>
    internal class InventoryTaskSparePartViewConfig : WebViewConfig<InventoryTaskSparePart>
    {
        //单个字符宽度
        private readonly int SingleCharWidth = 20;

        /// <summary>
        /// 盘点平账界面
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

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.InventoryTasks.InventoryTaskSparePartBehavior");
            View.UseCommand("SIE.Web.EMS.InventoryTasks.Commands.SearchInventoryTaskSparePartCommand");
            View.Property(p => p.SparePartId).ShowInList(SingleCharWidth * 8).Readonly().HasLabel("备件编码");
            View.Property(p => p.SparePartName).ShowInList(SingleCharWidth * 8).Readonly();
            View.Property(p => p.ControlMethod).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.GoodQty).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.NgQty).ShowInList(SingleCharWidth * 4).Readonly();
            View.Property(p => p.Total).ShowInList(SingleCharWidth * 4).Readonly();

            using (View.DeclareBand("初盘".L10N()))
            {
                View.Property(p => p.FirstGood).Readonly().HasLabel("良品数").ShowInList();
                View.Property(p => p.FirstNg).Readonly().HasLabel("不良品数").ShowInList();
                View.Property(p => p.FirstTotal).Readonly().HasLabel("总数").ShowInList();
                View.Property(p => p.FirstDiff).Readonly().HasLabel("差异数").ShowInList();
            }

            using (View.DeclareBand("复盘".L10N()))
            {
                View.Property(p => p.SecondGoodQty).Readonly().HasLabel("良品数").ShowInList();
                View.Property(p => p.SecondNgQty).Readonly().HasLabel("不良品数").ShowInList();
                View.Property(p => p.SecondTotal).Readonly().HasLabel("总数").ShowInList();
                View.Property(p => p.SecondDiff).Readonly().HasLabel("差异数").ShowInList();
            }

            View.Property(p => p.Specification).Readonly().ShowInList();
            View.Property(p => p.ItemCategoryName).Readonly().ShowInList();
            View.Property(p => p.SpartType).Readonly().ShowInList();
            View.Property(p => p.UnitName).Readonly().ShowInList();

            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 盘点平账界面
        /// </summary> 
        protected void ConfigBalanceView()
        {
            View.AssignAuthorize(typeof(InventoryBalance));
            View.AddBehavior("SIE.Web.EMS.InventoryTasks.InventoryTaskSparePartBehavior");
            View.UseCommand("SIE.Web.EMS.InventoryTasks.Commands.SearchInventoryTaskSparePartCommand");

            using (View.OrderProperties())
            {
                View.UseCommand("SIE.Web.EMS.InventoryTasks.Commands.SearchInventoryTaskSparePartCommand");
                View.Property(p => p.SparePartId).ShowInList(SingleCharWidth * 8).Readonly();
                View.Property(p => p.SparePartName).ShowInList(SingleCharWidth * 8).Readonly();
                View.Property(p => p.ControlMethod).ShowInList(SingleCharWidth * 4).Readonly();
                View.Property(p => p.GoodQty).ShowInList(SingleCharWidth * 4).Readonly();
                View.Property(p => p.NgQty).ShowInList(SingleCharWidth * 4).Readonly();
                View.Property(p => p.Total).ShowInList(SingleCharWidth * 4).Readonly();

                using (View.DeclareBand("初盘".L10N()))
                {
                    View.Property(p => p.FirstGood).Readonly().HasLabel("良品数").ShowInList();
                    View.Property(p => p.FirstNg).Readonly().HasLabel("不良品数").ShowInList();
                    View.Property(p => p.FirstTotal).Readonly().HasLabel("总数").ShowInList();
                    View.Property(p => p.FirstDiff).Readonly().HasLabel("差异数").ShowInList();
                }

                using (View.DeclareBand("复盘".L10N()))
                {
                    View.Property(p => p.SecondGoodQty).Readonly().HasLabel("良品数").ShowInList();
                    View.Property(p => p.SecondNgQty).Readonly().HasLabel("不良品数").ShowInList();
                    View.Property(p => p.SecondTotal).Readonly().HasLabel("总数").ShowInList();
                    View.Property(p => p.SecondDiff).Readonly().HasLabel("差异数").ShowInList();
                }

                View.Property(p => p.Specification).Readonly().ShowInList();
                View.Property(p => p.ItemCategoryName).Readonly().ShowInList();
                View.Property(p => p.SpartType).Readonly().ShowInList();
                View.Property(p => p.UnitName).Readonly().ShowInList();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
