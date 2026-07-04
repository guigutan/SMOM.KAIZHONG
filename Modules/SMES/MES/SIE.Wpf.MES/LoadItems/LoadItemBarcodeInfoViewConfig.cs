using SIE.MES.LoadItems;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 
    /// </summary>
    public class LoadItemBarcodeInfoViewConfig : WPFViewConfig<LoadItemBarcodeInfo>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(LoadItemBarcodeInfo));

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Label).Readonly();
            View.Property(p => p.ItemCode).Readonly();
            View.Property(p => p.ItemName).Readonly();
            View.Property(p => p.ItemExtPropName).Readonly().HasLabel("物料扩展属性名称");
            View.Property(p => p.ProjectNo).Readonly().HasLabel("项目号");
            View.Property(p => p.Qty).Readonly().HasLabel("数量");
            View.Property(p => p.LotNo).Readonly().HasLabel("批次");
            View.Property(p => p.ProjectNo).Readonly();
            View.Property(p => p.StorageLocationCode).Readonly().HasLabel("库位");
            View.Property(p => p.WarehouseName).Readonly().HasLabel("仓库");
        }
    }
}
