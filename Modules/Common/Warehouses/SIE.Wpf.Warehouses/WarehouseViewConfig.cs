using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Behaviors;
using SIE.Wpf.Common;
using SIE.Wpf.Common.Commands;
using SIE.Wpf.Warehouses.Command;

namespace SIE.Wpf.Warehouses
{
    /// <summary>
    /// 仓库视图配置
    /// </summary>
    internal class WarehouseViewConfig : WPFViewConfig<Warehouse>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Name);
            View.Property(p => p.LibraryType).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Category).UseCatalogEditor(e => e.CatalogType = Warehouse.CatalogCategory);
            View.Property(p => p.SimpleCode);
            View.Property(p => p.IsFrozen).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.UpdateDate).Readonly();
            View.AssociateChildrenProperty(WarehouseInfoDetailProperty.WarehouseInfoProperty, c =>
            {
                var warehouse = c.Parent as Warehouse;
                var warehouselist = RT.Service.Resolve<WarehouseController>().GetWarehouseInfoDetail(warehouse.Id);
                return warehouselist == null ? new WarehouseInfo() { Warehouse = warehouse, WarehouseId = warehouse.Id } : warehouselist;
            }).HasLabel("基本资料").OrderNo = 1;
            View.ChildrenProperty(p => p.WarehouseAddressList).OrderNo = 2;
            View.ChildrenProperty(p => p.EmployeeList).OrderNo = 3;
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.AddBehavior(typeof(GridRowDoubleClickViewBehavior));
            View.UseDefaultCommands();
            View.ReplaceCommands(WPFCommandNames.ListDelete, typeof(WarehouseDeleteCommand));
            View.ReplaceCommands(WPFCommandNames.ListAdd, typeof(WarehouseAddCommand));
            View.ReplaceCommands(typeof(EnableCommand), typeof(WarehouseEnableCommand));
            View.ReplaceCommands(typeof(DisableCommand), typeof(WarehouseDisableCommand));
            View.ReplaceCommands(WPFCommandNames.ListCopy, typeof(WarehouseListCopyCommand));
            View.RemoveCommands(WPFCommandNames.Undo, WPFCommandNames.Redo);
            View.UseCommands(typeof(FrozenCommand));
            View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Name);
            View.Property(p => p.LibraryType).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Category).UseCatalogEditor(e => e.CatalogType = Warehouse.CatalogCategory);
            View.Property(p => p.SimpleCode);
            View.Property(p => p.IsFrozen).Readonly();
            View.Property(p => p.State).Readonly();
            //View.Property(DataEntityExtension.UpdateByNameProperty).Readonly(); //更新人
            View.Property(p => p.UpdateDate).Readonly();
            View.AssociateChildrenProperty(WarehouseInfoDetailProperty.WarehouseInfoProperty, c =>
            {
                var warehouse = c.Parent as Warehouse;
                var warehouselist = RT.Service.Resolve<WarehouseController>().GetWarehouseInfoDetail(warehouse.Id);
                return warehouselist == null ? new WarehouseInfo() { Warehouse = warehouse, WarehouseId = warehouse.Id } : warehouselist;
            }).HasLabel("基本资料").OrderNo = 1;
            View.ChildrenProperty(p => p.WarehouseAddressList).OrderNo = 2;
            View.ChildrenProperty(p => p.EmployeeList).OrderNo = 3;
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.LibraryType).UseEnumEditor(p => p.AllowNullInput = true);
            View.Property(p => p.Category).UseCatalogEditor(e => e.CatalogType = Warehouse.CatalogCategory);
            View.Property(p => p.IsFrozen).UseCheckDropDownEditor();
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.LibraryType).Show();
            View.Property(p => p.Category).Show();
            View.Property(p => p.IsFrozen).Show();
        }
    }
}
