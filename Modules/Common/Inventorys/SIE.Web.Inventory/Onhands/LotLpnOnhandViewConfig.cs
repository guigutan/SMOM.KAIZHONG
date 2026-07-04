using SIE.Inventory.Onhands;
using SIE.MetaModel.View;

namespace SIE.Web.Inventory.Onhands
{
    /// <summary>
    /// 批次和LPN库存视图配置
    /// </summary>
    internal class LotLpnOnhandViewConfig : WebViewConfig<LotLpnOnhand>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.DisableEditing();
            View.AddBehavior("SIE.Web.Inventory.LotLpnOnhandBehavior");
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {                             
                View.Property(p => p.WarehouseName).FixColumn();
                View.Property(p => p.WarehouseCode);
                View.Property(p => p.StorageLocationCode);
                View.Property(p => p.AreaCode);
                View.Property(p => p.AreaName);
                View.Property(p => p.LotCode);
                View.Property(p => p.ItemCode).ShowInList(150);
                View.Property(p => p.ItemName).ShowInList(150);
                View.Property(p => p.Qty);
                View.Property(p => p.AvailableQty);
                View.Property(p => p.AllottedQty);
                View.Property(p => p.FreezingQty);
                View.Property(p => p.UnitName);
                View.Property(p => p.Lpn);              
                View.Property(p => p.StorageLocationName);
                View.Property(p => p.State);
                View.Property(p => p.ItemSpecificationModel);
                View.Property(p => p.ItemExtPropName).ShowInList(180);
                View.Property(p => p.StorerCode);
                View.Property(p => p.ProjectNo);
                View.Property(p => p.TaskNo).ShowInList(150);
                View.Property(p => p.SecondQty).DisableSort();
                View.Property(p => p.SecondUnitName).DisableSort();
                View.Property(p => p.ConvertFigre).DisableSort();
                View.Property(p => p.LotAtt01).HasLabel("生产日期").ShowInList(150);
                View.Property(p => p.LotAtt02).HasLabel("失效日期").ShowInList(150);
                View.Property(p => p.LotAtt03).HasLabel("收货日期").ShowInList(150);
                View.Property(p => p.LotAtt04).HasLabel("生产批次");
                View.Property(p => p.LotAtt05).HasLabel("复检次数");
                View.Property(p => p.LotAtt06);
                View.Property(p => p.LotAtt07).HasLabel("是否特采").Readonly();
                View.Property(p => p.LotAtt08);
                View.Property(p => p.LotAtt09);
                View.Property(p => p.LotAtt11);
                View.Property(p => p.LotAtt12);
                //View.Property(p => p.RouteNo).ShowInList();
                //View.Property(p => p.RowNo);              
                //View.Property(p => p.LayerNo);
                //View.Property(p => p.ColumnNo);
                //View.Property(p => p.Depth);
                //View.Property(p => p.IsMaxDepth);
                View.Property(p => p.Id).HasLabel("ID");
            }
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.ItemCode).ShowInList(width: 150);
            View.Property(p => p.ItemExtPropName).ShowInList(width: 180);
            View.Property(p => p.WarehouseCode);
            View.Property(p => p.WarehouseName);
            View.Property(p => p.StorageLocationCode);
            View.Property(p => p.StorageLocationName);
            View.Property(p => p.LotCode);
            View.Property(p => p.Lpn);
            View.Property(p => p.State);
            View.Property(p => p.Qty);
            View.Property(p => p.AvailableQty);
            View.Property(p => p.FreezingQty);
            View.Property(p => p.AllottedQty);
            View.Property(p => p.StorerCode);
            View.Property(p => p.ProjectNo);
            View.Property(p => p.TaskNo);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.ItemId).HasLabel("物料");
            View.Property(p => p.WarehouseId).HasLabel("仓库");
            View.Property(p => p.StorageLocationId).HasLabel("库位");
            View.Property(p => p.LotCode);
            View.Property(p => p.Lpn);
            View.Property(p => p.State);
            View.Property(p => p.StorerCode);
            View.Property(p => p.ProjectNo);
            View.Property(p => p.TaskNo);
        }
    }
}
