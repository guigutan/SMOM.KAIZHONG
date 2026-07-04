using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.CSM.Customers.Commands;

namespace SIE.Web.CSM.Customers
{
    /// <summary>
    /// 客户视图配置
    /// </summary>
    internal class CustomerViewConfig : WebViewConfig<Customer>
    {
        /// <summary>
        /// 只读视图
        /// </summary>
        public const string readOnlyView = "ReadOnlyView";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            if (ViewGroup == readOnlyView)
            {//只读视图
                ReadOnlyConfigView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            //替换保存方法和删除的方法
            View.ReplaceCommands(WebCommandNames.Save, "SIE.Web.CSM.Customers.Commands.CustomerSaveCommand");
            View.ReplaceCommands(WebCommandNames.Delete, typeof(CustomerDeleteCommand).FullName);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ShortName);
            View.Property(p => p.EnglishName);
            View.Property(p => p.Description);
            View.Property(p => p.CustomerType).UseListSetting(e => { e.HelpInfo = "更改类型清空供应商"; }).Cascade(p => p.Supplier, null);
            View.Property(p => p.Supplier).Readonly(p => p.CustomerType != CustomerType.SHIPPER && p.CustomerType != CustomerType.CUSTOMER)
                .UseListSetting(e => { e.HelpInfo = "类型为货主时可编辑"; });
            View.Property(p => p.Region).UseListSetting(e => { e.HelpInfo = "所在区域快码类型(AREA_TYPE)"; }).UseCatalogEditor(e =>
            {//区域使用快码
                e.CatalogType = Supplier.CatalogAreaType;
                e.CatalogReloadData = true;
            });
            View.Property(p => p.DutyParagraph);
            View.Property(p => p.Contacts);
            View.Property(p => p.ContactsNumber);
            View.Property(p => p.ContactsAddress);
            View.Property(p => p.EMail);
            View.Property(p => p.ZipCode);
            View.Property(p => p.SourceType).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.Remark);
            View.ChildrenProperty(p => p.CustomerAddressList);
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected void ReadOnlyConfigView()
        {
            View.Property(p => p.Code).ShowInList().Readonly();
            View.Property(p => p.Name).ShowInList().Readonly();
            View.Property(p => p.ShortName).ShowInList().Readonly();
            View.Property(p => p.Region).UseListSetting(e => { e.HelpInfo = "所在区域快码类型(AREA_TYPE)"; }).UseCatalogEditor(e =>
            {//区域使用快码
                e.CatalogType = Supplier.CatalogAreaType;
                e.CatalogReloadData = true;
            }).ShowInList().Readonly();
            View.Property(p => p.State).ShowInList().Readonly();
            View.Property(p => p.CustomerType).ShowInList().Readonly();
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ShortName);
            View.Property(p => p.Region).UseListSetting(e => { e.HelpInfo = "所在区域快码类型(AREA_TYPE)"; }).UseCatalogEditor(e =>
            {//区域使用快码
                e.CatalogType = Supplier.CatalogAreaType;
                e.CatalogReloadData = true;
            });
            View.Property(p => p.State);
            View.Property(p => p.CustomerType);
        }
    }
}
