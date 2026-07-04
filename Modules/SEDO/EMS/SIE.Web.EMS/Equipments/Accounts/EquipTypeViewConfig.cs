using SIE.Core.Equipments;
using SIE.EMS.Equipments.Accounts;
using SIE.Web.Common;

namespace SIE.Web.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备类型视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class EquipTypeViewConfig : WebViewConfig<EquipType>
    {

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.TypeCode).HasLabel("编码");
            View.Property(p => p.TypeName).HasLabel("名称");
            View.Property(p => p.TypeCategory).UseCatalogEditor(c => c.CatalogType = EquipType.EquipTypeCatalogType).HasLabel("类别");
            View.Property(p => p.Num).DefaultValue(0).Show(ShowInWhere.Hide); ////隐藏
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.TypeCode).HasLabel("编码");
            View.Property(p => p.TypeName).HasLabel("名称");
            View.Property(p => p.TypeCategory).UseCatalogEditor(c => c.CatalogType = EquipType.EquipTypeCatalogType).HasLabel("类别");
        }

        /// <summary>
		/// 配置查询视图
		/// </summary>
		protected override void ConfigQueryView()
        {
            View.Property(p => p.TypeCode).HasLabel("编码");
            View.Property(p => p.TypeName).HasLabel("名称");
            View.Property(p => p.TypeCategory).UseCatalogEditor(c => c.CatalogType = EquipType.EquipTypeCatalogType).HasLabel("类别");
        }
    }
}