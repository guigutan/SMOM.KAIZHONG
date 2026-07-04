using SIE.CSM.Suppliers;

namespace SIE.WPF.CSM.Suppliers
{
    /// <summary>
    /// 供应商查询实体视图配置
    /// </summary>
    internal class SupplierCriteriaViewConfig : WPFViewConfig<SupplierCriteria>
    {
        protected override void ConfigView()
        {
            View.DomainName("供应商查询");
            View.UseDefaultCommands();

            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("编码").Show(ShowInWhere.All);
                View.Property(p => p.Name).HasLabel("名称").Show(ShowInWhere.All);
            }
        }
    }
}
