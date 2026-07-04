using SIE.Equipments.Configs;

namespace SIE.Web.EMS.Common
{
    /// <summary>
    /// 配置值视图
    /// </summary>
    public class IsCreateHandoverBillConfigValueViewConig : WebViewConfig<IsCreateHandoverBillConfigValue>
    {
        /// <summary>
        /// 配置表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsCreateHandoverBill);
        }
    }
}
