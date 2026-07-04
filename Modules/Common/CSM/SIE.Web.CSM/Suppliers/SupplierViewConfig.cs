using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Common;
using SIE.Web.CSM.Suppliers.Commands;
using System;

namespace SIE.Web.CSM.Suppliers
{
    /// <summary>
	///供应商视图配置
	/// </summary>
    internal class SupplierViewConfig : WebViewConfig<Supplier>
    {
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.UseCommands(typeof(EnablePortalCommand).FullName);
            View.UseCommands(typeof(DisablePortalCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Add, typeof(AddSupplierCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Delete, typeof(SupplierDeleteCommand).FullName);
            View.RemoveCommands(WebCommandNames.Copy);
            View.Property(p => p.Code).FixColumn();
            View.Property(p => p.Name).FixColumn();
            View.Property(p => p.ShortName);
            View.Property(p => p.EnglishName);
            View.Property(p => p.Description); //描述
            View.Property(p => p.Type).UseListSetting(e =>
            {
                e.HelpInfo = "供应商快码类型(SUPPLIER_TYPE)";
            }).ShowInList(width: 70)
            .UseCatalogEditor(e =>
            {
                e.CatalogType = Supplier.SupperType; e.CatalogReloadData = true;
            });
            View.Property(p => p.Region).UseListSetting(e =>
            {
                e.HelpInfo = "所在区域快码类型(AREA_TYPE)";
            }).ShowInList(width: 70).UseCatalogEditor(e =>
            {
                e.CatalogType = Supplier.CatalogAreaType; e.CatalogReloadData = true;
            });
            View.Property(p => p.DutyParagraph);
            View.Property(p => p.Contacts);
            View.Property(p => p.ContactNumber);
            View.Property(p => p.ContactAddress);
            View.Property(p => p.EMail).ShowInList(width: 150);
            View.Property(p => p.ZipCode).ShowInList(width: 70);
            View.Property(p => p.SourceType).Readonly().ShowInList(width: 70);
            View.Property(p => p.State).Readonly().ShowInList(width: 60);
            View.Property(p => p.IsPortal).Readonly().ShowInList(width: 60).HasLabel("门户");
            View.Property(p => p.OutsourcingInLoc).UseDataSource((o, e, r) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetAllEnableStorageLocations(r, e, null, true);
            }).UsePagingLookUpEditor().ShowInList();
            View.Property(p => p.OutsourcingOutLoc).UseDataSource((o, e, r) =>
            {
                return RT.Service.Resolve<WarehouseController>().GetAllEnableStorageLocations(r, e, null, true);
            }).UsePagingLookUpEditor().ShowInList();
            View.Property(p => p.OutsourcingReceive).ShowInList();
            View.Property(p => p.OutsourcingUseTime).ShowInList();
            View.Property(p => p.IsHasStorer).ShowInList();
            View.Property(p => p.Remark);

            View.ChildrenProperty(p => p.AddressList); //子列表-地址
            View.ChildrenProperty(p => p.ItemList); //子列表-物料           
            View.AttachChildrenProperty(typeof(SupplierUser), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<Supplier>();
                if (parent == null)
                {
                    return new EntityList<SupplierUser>();
                }

                var roleUser = RT.Service.Resolve<SupplierController>().GetUsersBySupplierId(parent.Id, args.SortInfo, args.PagingInfo);
                return roleUser;
            }, nameof(SupplierUser)).OrderNo = 1;
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.Type).UseListSetting(e => { e.HelpInfo = "供应商快码类型(SUPPLIER_TYPE)"; })
                .UseCatalogEditor(e => { e.CatalogType = Supplier.SupperType;e.CatalogReloadData = true; });
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.IsPortal).UseCheckDropDownEditor(p => p.AllowBlank = true).HasLabel("门户");
            View.Property(p => p.State).UseEnumEditor(p => p.AllowBlank = true);
        }
    }
}
