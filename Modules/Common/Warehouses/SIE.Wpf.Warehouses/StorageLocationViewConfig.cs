using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Wpf.Command;
using SIE.Wpf.Common.Commands;
using SIE.Wpf.Warehouses.Command;
using SIE.Wpf.Warehouses.ViewBehaviors;

namespace SIE.Wpf.Warehouses
{
    /// <summary>
    /// 货位视图配置
    /// </summary>
    internal class StorageLocationViewConfig : WPFViewConfig<StorageLocation>
    {
        
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors();
            View.UseDefaultCommands();
            View.RemoveCommands(typeof(RedoCommand), typeof(UndoCommand));
            View.AddBehavior(typeof(SpecialItemBehavior));
            View.AddBehavior(typeof(StorageLocationChangeBehavior));
            View.UseCommands(typeof(StorageLocationFrozenCommand), typeof(PrintStorageLocationCommand));
            View.ReplaceCommands(WPFCommandNames.ListAdd, typeof(StorageLocationAddCommand));
            //View.ReplaceCommands(typeof(ListSaveCommand), typeof(StorageLocationSaveCommand));
            View.ReplaceCommands(WPFCommandNames.ListCopy, typeof(StorageLocationCopyCommand));
            View.ReplaceCommands(WPFCommandNames.ListDelete, typeof(StorageLocationDeleteCommand));
            View.ReplaceCommands(typeof(EnableCommand), typeof(StorageLocationEnableCommand));
            View.ReplaceCommands(typeof(DisableCommand), typeof(StorageLocationDisableCommand));
            View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Name);
            View.Property(p => p.LibraryType).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Warehouse).Readonly(DataEntityStatus.IsEditStatusProperty).UseWarehouseLookUpEditor();
            View.Property(p => p.Area).Readonly(DataEntityStatus.IsEditStatusProperty).UseStorageAreaEditor(p => p.ReloadDataOnPopping = true);
            View.Property(p => p.IsFrozen).Readonly();
            View.Property(p => p.State).Readonly();
            View.Property(p => p.ErpInvOrg);
            View.Property(p => p.ErpSubLibrary);
            View.Property(p => p.ErpLocation);
            //View.Property(DataEntityExtension.UpdateByNameProperty);
            View.Property(DataEntity.UpdateDateProperty);
            View.AssociateChildrenProperty(StorageLocationDetailProperty.BaseInfoProperty, (o) =>
            {
                var storageLocation = o.Parent as StorageLocation;
                var storageLocationInfo = RT.Service.Resolve<WarehouseController>().GetStorageLocationInfo(storageLocation.Id);
                return storageLocationInfo == null ? new StorageLocationInfo() { StorageLocation = storageLocation, StorageLocationId = storageLocation.Id } : storageLocationInfo;
            }).HasLabel("基础资料").OrderNo = 1;
            View.AssociateChildrenProperty(StorageLocationDetailProperty.LayinInfoProperty, (o) =>
            {
                var storageLocation = o.Parent as StorageLocation;
                var storageLocationInfo = RT.Service.Resolve<WarehouseController>().GetStorageLocationLayinInfo(storageLocation.Id);
                return storageLocationInfo == null ? new StorageLocationLayinInfo() { StorageLocation = storageLocation, StorageLocationId = storageLocation.Id } : storageLocationInfo;
            }).HasLabel("仓储资料").OrderNo = 2;
            View.AssociateChildrenProperty(StorageLocationDetailProperty.OperationInfoProperty, (o) =>
            {
                var storageLocation = o.Parent as StorageLocation;
                var storageLocationInfo = RT.Service.Resolve<WarehouseController>().GetStorageLocationOperation(storageLocation.Id);
                return storageLocationInfo == null ? new StorageLocationOperation() { StorageLocation = storageLocation, StorageLocationId = storageLocation.Id } : storageLocationInfo;
            }).HasLabel("操作管理").OrderNo = 3;
            WPFChildrenPropertyViewMeta childView = View.ChildrenProperty(p => p.StorageLocationItemListList);
            childView.OrderNo = 4;
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.LibraryType).Show();
            View.Property(p => p.Warehouse).Show();
            View.Property(p => p.Area).Show();
            View.Property(p => p.IsFrozen).Show();
        }
    }
}