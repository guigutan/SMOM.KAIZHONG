using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Wpf.Common;
using SIE.Wpf.CSM.Suppliers.Commonds;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.CSM.Suppliers
{
    /// <summary>
    /// 供应商视图配置
    /// </summary>
    internal class SupplierViewConfig : WPFViewConfig<Supplier>
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.UseDefaultCommands();
            View.UseDetail(dialogWidth: 800, dialogHeight: 500);
        }

        /// <summary>
        /// 视图列表配置
        /// </summary>
        protected override void ConfigListView()
        {
            //View.UseCommands(typeof(SupplierEnableCommand), typeof(SupplierDisableCommand));
            View.UseCommands(typeof(PortalEnabledCommand), typeof(PortalDisEnabledCommand));
            View.ReplaceCommands(WPFCommandNames.ListCopy, typeof(CopyCommand));
            View.Property(p => p.Code); //视图
            View.Property(p => p.Name); //名称
            View.Property(p => p.ShortName);
            View.Property(p => p.EnglishName);
            View.Property(p => p.Description); //描述
            View.Property(p => p.Type).UseListSetting(e => { e.HelpInfo = "供应商快码类型(SUPPLIER_TYPE)"; })
                .UseCatalogEditor(e => e.CatalogType = Supplier.SupperType).UseListSetting(w => w.ListGridWidth = 70);
            View.Property(p => p.Region).UseListSetting(e => { e.HelpInfo = "所在区域快码类型(AREA_TYPE)"; })
                .UseCatalogEditor(e => e.CatalogType = Supplier.CatalogAreaType).UseListSetting(w => w.ListGridWidth = 70);
            View.Property(p => p.DutyParagraph);
            View.Property(p => p.Contacts);
            View.Property(p => p.ContactNumber);
            View.Property(p => p.ContactAddress);
            View.Property(p => p.EMail).UseListSetting(w => w.ListGridWidth = 150);
            View.Property(p => p.ZipCode).UseListSetting(w => w.ListGridWidth = 70);
            View.Property(p => p.SourceType).Readonly().UseListSetting(w => w.ListGridWidth = 70);
            View.Property(p => p.State).Readonly().UseListSetting(w => w.ListGridWidth = 60);
            View.Property(p => p.IsPortal).Readonly().UseListSetting(w => w.ListGridWidth = 60).HasLabel("门户");
            View.Property(p => p.Remark);

            View.ChildrenProperty(p => p.AddressList); //子列表-地址
            View.ChildrenProperty(p => p.ItemList); //子列表-物料
            //View.ChildrenProperty(p => p.UserList); //子列表-用户
            View.AttachChildrenProperty(typeof(SupplierUser), (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<Supplier>();
                if (parent == null)
                {
                    return new EntityList<SupplierUser>();
                }

                var roleUser = RT.Service.Resolve<SupplierController>().GetUsersBySupplierId(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return roleUser;
            }, nameof(SupplierUser)).OrderNo = 1;
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code); //编码
            View.Property(p => p.Name); //名称
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.Type).UseListSetting(e => { e.HelpInfo = "供应商快码类型(SUPPLIER_TYPE)"; })
                .UseCatalogEditor(e => e.CatalogType = Supplier.SupperType);
        }
    }
}
