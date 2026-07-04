using SIE.Domain;
using SIE.Items;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.DashBoard.Reports.ProductFPY;

namespace SIE.Wpf.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 产品直通率设置视图配置
    /// </summary>
    internal class ProductFpySettingViewConfig : WPFViewConfig<ProductFpySetting>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(ProductFpySetting.ProductIdProperty);
            View.UseDefaultBehaviors();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(ProductReportViewModel));
            View.UseCommands(WPFCommandNames.ListAdd, WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            View.Property(p => p.Product).HasLabel("产品编码").UseDataSource((e, c, r) =>
            {
                var setting = e as ProductFpySetting;
                if (setting?.ProductModelFpySetting?.Model != null)
                    return RT.Service.Resolve<ItemController>().GetItems(ItemType.Material, c, r, setting.ProductModelFpySetting.ModelId);
                return new EntityList<Item>();
            }).UsePagingLookUpEditor(p => { p.ReloadDataOnPopping = true; p.DisplayMember = Item.CodeProperty.Name; });
            View.Property(p => p.Product.Name).HasLabel("产品名称");
            View.Property(p => p.Desired).UseSpinEditor(e => { e.Decimals = 2; e.MinValue = 0; e.MaxValue = 100; });
            View.Property(p => p.Alarm).UseSpinEditor(e => { e.Decimals = 2; e.MinValue = 0; e.MaxValue = 100; });
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
        }
    }
}