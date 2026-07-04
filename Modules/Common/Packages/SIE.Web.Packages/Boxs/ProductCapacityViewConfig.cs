using SIE.MetaModel.View;
using SIE.Packages.Boxs;

namespace SIE.Web.Packages.Boxs
{
    /// <summary>
    /// 产品容量视图配置
    /// </summary>
    public class ProductCapacityViewConfig : WebViewConfig<ProductCapacity>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            base.ConfigView();
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands().UseCommands("SIE.Web.Packages.Boxs.Commands.AddCpaacityCommand", WebCommandNames.Edit, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).ShowInList(width: 150).HasLabel("物料编码").Readonly();
                View.Property(p => p.ItemName).ShowInList(width: 150).HasLabel("物料名称").Readonly();
                View.Property(p => p.Capacity).HasLabel("容量").UseSpinEditor(p => { p.MinValue = 1; p.MaxValue = 1000000; });
                View.Property(p => p.IsOperate).HasLabel("满箱转运").UseCheckEditor();
                View.Property(p => p.IsDefault).HasLabel("是否默认").UseCheckEditor();
            }
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            base.ConfigDetailsView();
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).HasLabel("物料编码").Readonly();
                View.Property(p => p.ItemName).HasLabel("物料名称").Readonly();
                View.Property(p => p.Capacity).HasLabel("容量").UseSpinEditor(p => { p.MinValue = 1; p.MaxValue = 1000000; });
                View.Property(p => p.IsOperate).HasLabel("满箱转运").UseCheckEditor();
                View.Property(p => p.IsDefault).HasLabel("是否默认").UseCheckEditor();
            }
        }
    }
}
