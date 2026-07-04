using SIE.ProductIntfc.ProductStorages;
using SIE.Wpf.ProductIntfc.ProductStorages.Editors;

namespace SIE.Wpf.ProductIntfc.ProductStorages
{
    /// <summary>
    /// 成品入库参数视图配置
    /// </summary>
    public class ProductStorageParamViewConfig : WPFViewConfig<ProductStorageParam>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            //方法重写
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultBehaviors();
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).HasLabel("产品编码").UseEditor(StorageParamLookUpEditor.EditorName);
                View.Property(p => p.ItemName).HasLabel("产品名称");
                View.Property(p => p.ItemType).UseEnumEditor().Readonly().HasLabel("入库类型");
                View.Property(p => p.InspDimension).HasLabel("入库维度");
                View.Property(p => p.Qty).UseSpinEditor(p => p.Decimals = 0).HasLabel("入库参数");
            }
        }

        /// <summary>
        /// 弹出窗体配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            base.ConfigSelectionView();
            View.Property(p => p.ItemCode).HasLabel("产品编码");
            View.Property(p => p.ItemName).HasLabel("产品名称");
            View.Property(p => p.ItemType).UseEnumEditor().HasLabel("入库类型");
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            base.ConfigQueryView();
            View.Property(p => p.Item).HasLabel("产品编码");
            View.Property(p => p.Item.Type).UseEnumEditor().HasLabel("入库类型");
        }
    }
}