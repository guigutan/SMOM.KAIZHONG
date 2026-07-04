using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.Customers
{
    /// <summary>
    /// 客户中间表视图配置
    /// </summary>
    internal class CustomerInfViewConfig : WebViewConfig<CustomerInf>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ShortName);
            View.Property(p => p.Region);
            View.Property(p => p.EnglishName);
            View.Property(p => p.Description);
            View.Property(p => p.DutyParagraph);
            View.Property(p => p.Contacts);
            View.Property(p => p.ContactsNumber);
            View.Property(p => p.ContactsAddress);
            View.Property(p => p.EMail);
            View.Property(p => p.ZipCode);
            View.Property(p => p.Remark);
            View.Property(p => p.OwnCode);
            View.Property(p => p.OwnName);
            View.Property(p => p.CustomerType);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}