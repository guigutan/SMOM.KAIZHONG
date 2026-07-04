using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Web.Equipments.EquipAccounts.Commands;

namespace SIE.Web.Equipments.EquipAccounts
{
    /// <summary>
	/// 设备台账产品视图配置
	/// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class EquipAccountProductViewConfig : WebViewConfig<EquipAccountProduct>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelAccountProductCommand).FullName, WebCommandNames.Delete, WebCommandNames.Save);
            View.Property(p => p.ProductCode);
            View.Property(p => p.ProductName);
            View.Property(p => p.SpecificationModel);
            View.Property(p => p.UnitName);
            View.Property(p => p.ProductState);
        }
    }
}
