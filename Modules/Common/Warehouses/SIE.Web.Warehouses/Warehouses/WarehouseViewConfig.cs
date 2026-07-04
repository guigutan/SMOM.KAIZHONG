using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Common;
using SIE.Web.Common.Commands;
using SIE.Web.Warehouses.Commands;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 仓库视图配置
    /// </summary>
    internal class WarehouseViewConfig : WebViewConfig<Warehouse>
    {
        /// <summary>
        /// 只读视图
        /// </summary>
        public const string ReadonlyView = "ReadonlyView";

        /// <summary>
        ///  库存组织视图
        /// </summary>
        public const string WarehouseInvorgView = "WarehouseInvorgView";

        /// <summary>
        /// 查询视图
        /// </summary>
        public const string SearchWarehouseView = "SearchWarehouseView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { ReadonlyView, WarehouseInvorgView, SearchWarehouseView });
            View.AssignAuthorize(typeof(Warehouse));
            if (ViewGroup == ReadonlyView)
                ConfigReadonlyView();
            if (ViewGroup == WarehouseInvorgView)
                ConfigWarehouseInvorgView();
            if (ViewGroup == SearchWarehouseView)
            {
                ConfigSearchWarehouseView();
            }
        }

        private void ConfigReadonlyView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList();
                View.Property(p => p.Name).ShowInList();
                View.Property(p => p.LibraryType).ShowInList();
                View.Property(p => p.Category).UseCatalogEditor(e => { e.CatalogType = Warehouse.CatalogCategory; e.CatalogReloadData = true; }).ShowInList();
                View.Property(p => p.SimpleCode).ShowInList();
                View.Property(p => p.IsFrozen).Readonly().ShowInList();
                View.Property(p => p.State).ShowInList();
                View.Property(p => p.IsLineWarehouse).Readonly().ShowInList();
                View.Property(p => p.IngoreOnhand).Readonly().ShowInList();
                View.Property(p => p.UpdateDate).ShowInList();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(typeof(WarehouseAddCommand).FullName, WebCommandNames.Edit, typeof(WarehouseDeleteCommand).FullName, WebCommandNames.Save);
            View.ReplaceCommands(EnableCommand.CommandName, typeof(WarehouseEnableCommand).FullName);
            View.ReplaceCommands(DisableCommand.CommandName, typeof(WarehouseDisableCommand).FullName);
            View.UseCommands(typeof(FrozenCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "根据仓库编码规则(配置项--仓库编码规则)生成库区编码，新增状态可编辑"; });
            View.Property(p => p.Name);
            View.Property(p => p.LibraryType).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Category).UseCatalogEditor(e => { e.CatalogType = Warehouse.CatalogCategory; e.CatalogReloadData = true; })
                .UseListSetting(e => { e.HelpInfo = "仓库分类快码类型(WAREHOUSE_TYPE)"; });
            View.Property(p => p.SimpleCode);
            View.Property(p => p.IsFrozen).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.IsLineWarehouse).UseListSetting(e => { e.HelpInfo = "用于MES线边仓的基础资料"; });
            
            View.Property(p => p.IngoreOnhand).ShowInList();
            View.Property(p => p.UpdateDate).Readonly();
            View.AssociateChildrenProperty(WarehouseInfoDetailProperty.WarehouseInfoProperty, c =>
            {
                var warehouse = c.Parent as Warehouse;
                var warehouselist = RT.Service.Resolve<WarehouseController>().GetWarehouseInfoDetail(warehouse.Id);
                if (warehouselist == null)
                {
                    return new WarehouseInfo() { Warehouse = warehouse, WarehouseId = warehouse.Id, WarehouseForm = WarehouseForm.Mix };
                }
                return warehouselist;
            }, ViewConfig.DetailsView).HasLabel("基本资料").OrderNo = 1;
            View.ChildrenProperty(p => p.WarehouseAddressList).HasLabel("仓库地址").OrderNo = 2;
            View.ChildrenProperty(p => p.EmployeeList).OrderNo = 3;
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Show().Readonly();
            View.Property(p => p.Name).Show().Readonly();
            View.Property(p => p.LibraryType).Show().Readonly();
            View.Property(p => p.Category).UseCatalogEditor(e => { e.CatalogType = Warehouse.CatalogCategory; e.CatalogReloadData = true; }).Show().Readonly();
            View.Property(p => p.IsFrozen).Show().Readonly();
            View.Property(p => p.InvOrgName).Show().Visibility(p => p.InvOrgName != "");
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        public void ConfigWarehouseInvorgView()
        {
            View.WithoutPaging();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show().Readonly();
                View.Property(p => p.Name).Show().Readonly();
                View.Property(p => p.InvOrgName).Show().Readonly();
                View.Property(p => p.LibraryType).Show().Readonly();
                View.Property(p => p.Category).UseCatalogEditor(e => { e.CatalogType = Warehouse.CatalogCategory; e.CatalogReloadData = true; }).Show().Readonly();
                View.Property(p => p.IsFrozen).Show().Readonly();
            }
        }

        public void ConfigSearchWarehouseView()
        {
            View.WithoutPaging();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.InvOrgName).Show();
                View.Property(p => p.LibraryType).Show();
                View.Property(p => p.Category).UseCatalogEditor(e => { e.CatalogType = Warehouse.CatalogCategory; e.CatalogReloadData = true; }).Show();
                View.Property(p => p.IsFrozen).Show();
            }
        }
    }
}
