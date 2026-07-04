using SIE.CSM.Customers;
using SIE.Kit.APS.EngineerPlans.Settings;
using SIE.Web.Resources;

namespace SIE.Web.Kit.APS.EngineerPlans.Settings
{
    /// <summary>
    /// 客户等级设置查询视图
    /// </summary>
    public class CustLevelSettingCriteriaViewConfig : WebViewConfig<CustLevelSettingCriteria>
    {
        /// <summary>
        /// 配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryId).UseFactoryEditor().Show(ShowInWhere.All);
                View.Property(p => p.Customer)
                        .UsePagingLookUpEditor(p =>
                        {
                            p.SearchFieldList.Add(Customer.NameProperty.Name);
                            p.BindDisplayField = Customer.NameProperty.Name;
                        })
                        .Show(ShowInWhere.All);

                View.Property(p => p.CustLevel).HasLabel("优先级").Show(ShowInWhere.All);
                View.Property(p => p.Remerk).Show(ShowInWhere.All);
            }
        }
    }
}
