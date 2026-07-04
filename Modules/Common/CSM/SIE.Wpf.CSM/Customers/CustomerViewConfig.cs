using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Wpf.Common;
using SIE.Wpf.CSM.Customers.ViewBehaviors;

namespace SIE.Wpf.CSM.Customers
{
    /// <summary>
    /// 客户视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class CustomerViewConfig : WPFViewConfig<Customer>
    {
        /// <summary>
        /// 供应商是否可编辑
        /// </summary>
        internal static readonly Property<bool> IsSupplierModifyProperty = P<Customer>.RegisterExtensionReadOnly("IsSupplierModify", typeof(CustomerViewConfig), (c) => { return c.CustomerType != CustomerType.SHIPPER; }, Customer.SupplierIdProperty);

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code); //编码
            View.Property(p => p.Name); //名称
            View.Property(p => p.ShortName); //简称
            View.Property(p => p.Region); //销售区域
            View.Property(p => p.State); //状态
        }

        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.UseDetail(dialogHeight: 400);
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior(typeof(CustomerChangeBehavior));
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ShortName);
            View.Property(p => p.EnglishName);
            View.Property(p => p.Description);
            ////View.Property(p => p.CustomerType).UseCatalogEditor(e => e.CatalogType = Customer.CatalogCustomerType);
            View.Property(p => p.CustomerType);
            View.Property(p => p.Supplier).Readonly(IsSupplierModifyProperty);
            View.Property(p => p.Region).UseCatalogEditor(e => e.CatalogType = Supplier.CatalogAreaType);
            View.Property(p => p.OwnCode);
            View.Property(p => p.OwnName);
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
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code); //编码
            View.Property(p => p.Name); //名称
        }
    }
}
