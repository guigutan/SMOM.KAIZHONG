using SIE.MetaModel.View;
using SIE.Packages.Packages;
using SIE.Web.Packages.Packages.Commands;

namespace SIE.Web.Packages.Packages
{
    /// <summary>
    /// 包装单位视图配置
    /// </summary>
    public class PackingUnitViewConfig : WebViewConfig<PackingUnit>
    {
        /// <summary>
        /// 通用列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            //添加框架按钮以及自定义添加主单位按钮
            View.InlineEdit().UseCommands(typeof(InitPackingUnitCommand).FullName, WebCommandNames.Add, WebCommandNames.Edit, typeof(PackingUnitDeleteCommand).FullName, WebCommandNames.Save, WebCommandNames.ExportXls);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.IsMasterUnit).Readonly();
            View.Property(p => p.PackageUnitType).Readonly(p => p.IsMasterUnit);
        }
        /// <summary>
        /// 通用下拉配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }
        /// <summary>
        /// 通用查询配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Code);
                View.Property(p => p.Name);
            }
        }
    }
}
