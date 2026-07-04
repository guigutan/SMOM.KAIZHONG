using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Warehouses.Command;

namespace SIE.Wpf.Warehouses
{
    /// <summary>
    /// 库区专储物料清单 视图配置
    /// </summary>
    internal class StorageLocationItemListViewConfig : WPFViewConfig<StorageLocationItemList>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(StorageLocationItemList.IdProperty);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors();
            View.UseCommands(typeof(StorageLocationItemLookUpCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Item).HasLabel("物料编码").Readonly();
                View.Property(p => p.Item.Name).HasLabel("物料名称");
                View.Property(p => p.Item.Description).HasLabel("规格型号");
                View.Property(p => p.Remark);
                //View.Property(DataEntityExtension.UpdateByNameProperty).Readonly();
                View.Property(DataEntity.UpdateDateProperty).Readonly();
            }
        }
    }
}
