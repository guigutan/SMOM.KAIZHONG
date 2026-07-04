using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.Customers
{
    /// <summary>
    /// 客户地址中间表视图配置
    /// </summary>
    internal class CustomerAddressInfViewConfig : WebViewConfig<CustomerAddressInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Address);
            View.Property(p => p.Contacts);
            View.Property(p => p.Phone);
            View.Property(p => p.ZipCode);
            View.Property(p => p.Email);
            View.Property(p => p.AddressType);
            View.Property(p => p.Name);
            View.Property(p => p.Country);
            View.Property(p => p.Province);
            View.Property(p => p.City);
            View.Property(p => p.Area);
            View.Property(p => p.Fax);
            View.Property(p => p.Remark);
            View.Property(p => p.CustomerCode);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Address);
            View.Property(p => p.Contacts);
        }
    }
}