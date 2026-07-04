using SIE.EMS.DevicePurs;
using SIE.MetaModel.View;

namespace SIE.Web.EMS.DevicePurs
{
    /// <summary>
    /// 采购对象权限视图配置
    /// </summary>
    internal class DevicePurchaseObjectTypeViewConfig : WebViewConfig<DevicePurchaseObjectType>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete);
            View.Property(p => p.PurchaseObjectType);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}