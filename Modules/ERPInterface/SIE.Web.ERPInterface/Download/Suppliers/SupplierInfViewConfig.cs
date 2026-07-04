using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.MetaModel.View;

namespace SIE.Web.ERPInterface.Download.Suppliers
{
    /// <summary>
    /// 供应商中间表视图配置
    /// </summary>
    internal class SupplierInfViewConfig : WebViewConfig<SupplierInf>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(SupplierInf.NameProperty);
            View.UseDefaultCommands();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.Type);
            View.Property(p => p.ShortName);
            View.Property(p => p.EnglishName);
            View.Property(p => p.Region);
            View.Property(p => p.DutyParagraph);
            View.Property(p => p.Contacts);
            View.Property(p => p.ContactNumber);
            View.Property(p => p.ContactAddress);
            View.Property(p => p.Email);
            View.Property(p => p.ZipCode);
            View.Property(p => p.Remark);
            View.Property(p => p.IsPortal);
        }

        ///<summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }
    }
}