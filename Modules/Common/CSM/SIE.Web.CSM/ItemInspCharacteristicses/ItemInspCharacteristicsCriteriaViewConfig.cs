using SIE.CSM.Suppliers;
using SIE.CSM.ItemInspCharacteristicses;

namespace SIE.Web.CSM.ItemInspCharacteristicses
{
    /// <summary>
    /// 物料检验特性查询实体视图配置
    /// </summary>
    internal class ItemInspCharacteristicsCriteriaViewConfig : WebViewConfig<ItemInspCharacteristicsCriteria>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Supplier).Show(ShowInWhere.All);
                View.Property(p => p.SupplierName).Show(ShowInWhere.All);
                View.Property(p => p.Item).UsePagingLookUpEditor().UseDataSource((e, p, s) =>
                {
                    var supplier = (e as ItemInspCharacteristicsCriteria).Supplier;
                    return AppRuntime.Service.Resolve<SupplierController>().GetItems(p, s, supplier?.Id, state: null);
                }).Show(ShowInWhere.All);
                View.Property(p => p.ItemName).Show(ShowInWhere.All);
                View.Property(p => p.SupplierState).Show(ShowInWhere.All);
            }
        }
    }
}
