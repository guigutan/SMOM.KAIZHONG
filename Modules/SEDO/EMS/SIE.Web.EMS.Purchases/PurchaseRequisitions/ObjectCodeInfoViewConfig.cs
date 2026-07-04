using SIE.EMS.Purchases.PurchaseRequisitions;

namespace SIE.Web.EMS.Purchases.PurchaseRequisitions
{
    /// <summary>
    /// 采购对象编码视图
    /// </summary>
    public class ObjectCodeInfoViewConfig : WebViewConfig<ObjectCodeInfo>
    {
        /// <summary>
        /// 下拉视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Value).ShowInList(150);
            View.Property(p => p.Name).ShowInList(200);
            View.Property(p => p.ModelCode);
            View.Property(p => p.ItemUnitNmae).HasLabel("单位").ShowInList(80);
            View.Property(p => p.Specification).ShowInList(200);
        }
    }
}
