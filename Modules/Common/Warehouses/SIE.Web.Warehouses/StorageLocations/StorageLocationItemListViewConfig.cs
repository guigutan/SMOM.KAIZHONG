using SIE.Domain;
using SIE.Warehouses;
using SIE.Web.Warehouses.Commands;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 库区专储物料清单 视图配置
    /// </summary>
    internal class StorageLocationItemListViewConfig : WebViewConfig<StorageLocationItemList>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(StorageLocationItemLookUpCommand).FullName, typeof(StorageLocationItemDeleteCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).HasLabel("物料编码").Readonly();
                View.Property(p => p.ItemName).HasLabel("物料名称").Readonly();
                View.Property(p => p.ItemSpecificationModel).HasLabel("规格型号").Readonly();
                View.Property(p => p.ItemUnit).HasLabel("单位").Readonly();
                View.Property(DataEntity.UpdateDateProperty).Readonly();
            }
        }
    }
}
