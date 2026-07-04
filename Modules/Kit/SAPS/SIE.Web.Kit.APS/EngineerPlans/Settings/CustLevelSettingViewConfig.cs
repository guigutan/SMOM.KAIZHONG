using SIE.Kit.APS.EngineerPlan.Settings;
using SIE.Resources.Enterprises;
using SIE.Web.Resources;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Kit.APS.EngineerPlans.Configs
{
    /// <summary>
    /// 客户等级设置 视图配置
    /// </summary>
    internal class CustLevelSettingViewConfig : WebViewConfig<CustLevelSetting>
    {
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryId).UseFactoryEditor();
                View.Property(p => p.CustomerId).GetEnableCustomersEditor()
                    .UsePagingLookUpEditor((m, e) =>
                    {
                        m.SearchFieldList.Add(SIE.CSM.Customers.Customer.NameProperty.Name);

                        //m.BindDisplayField = SIE.CSM.Customers.Customer.CodeProperty.Name;
                        //m.DisplayField = SIE.CSM.Customers.Customer.CodeProperty.Name;
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add(nameof(e.ViewCustomerName), nameof(e.Customer.Name));
                        m.DicLinkField = dic;
                    })
                    .Show(ShowInWhere.All);

                View.Property(p => p.ViewCustomerName).HasLabel("客户名称").Readonly(true);
                View.Property(p => p.CustLevel)
                    .UsePagingLookUpEditor((m, e) =>
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add(nameof(e.ViewCustLevelHour), nameof(e.CustLevel.Hour));
                        m.DicLinkField = dic;
                    }).HasLabel("优先级").Show(ShowInWhere.All);

                View.Property(p => p.ViewCustLevelHour).Show(ShowInWhere.All).Readonly(true);

                View.Property(p => p.Remerk).Show(ShowInWhere.All);
            }
        }

    }
}
