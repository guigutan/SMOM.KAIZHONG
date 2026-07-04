using SIE.Domain;
using SIE.Warehouses;
using SIE.Wpf.Common.Commands;
using SIE.Wpf.Warehouses.Command;
using SIE.Wpf.Warehouses.ViewBehaviors;

namespace SIE.Wpf.Warehouses
{
    /// <summary>
    /// 库区视图配置
    /// </summary>
    internal class StorageAreaViewConfig : WPFViewConfig<StorageArea>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(StorageArea.NameProperty);
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultBehaviors();
            View.AddBehavior(typeof(StorageAreaChangeBehavior));
            View.UseDefaultCommands();
            View.RemoveCommands(WPFCommandNames.Redo, WPFCommandNames.Undo);
            View.ReplaceCommands(WPFCommandNames.ListDelete, typeof(StorageAreaDeleteCommand));
            View.ReplaceCommands(WPFCommandNames.ListAdd, typeof(StorageAreaAddCommand));
            View.ReplaceCommands(typeof(EnableCommand), typeof(StorageAreaEnableCommand));
            View.ReplaceCommands(typeof(DisableCommand), typeof(StorageAreaDisableCommand));
            View.UseCommands(typeof(StorageAreaFrozenCommand));
            View.Property(p => p.Code).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Name);
            View.Property(p => p.LibraryType).Readonly(DataEntityStatus.IsEditStatusProperty);
            View.Property(p => p.Warehouse).Readonly(DataEntityStatus.IsEditStatusProperty).UseWarehouseLookUpEditor(p => p.ReloadDataOnPopping = true).HasLabel("仓库");
            View.Property(p => p.IsFrozen).Readonly();
            View.Property(p => p.State).Readonly();
            //View.Property(DataEntityExtension.UpdateByNameProperty);
            View.Property(p => p.UpdateDate);
            View.AssociateChildrenProperty(StorageAreaInfoDetailProperty.StorageAreaInfoProperty, c =>
            {
                var storageArea = c.Parent as StorageArea;
                var storagearealist = RT.Service.Resolve<WarehouseController>().GetStorageAreaInfoDetail(storageArea.Id);
                return storagearealist == null ? new StorageAreaInfo() { StorageArea = storageArea, StorageAreaId = storageArea.Id } : storagearealist;
            }).HasLabel("基本资料").OrderNo = 1;
            View.AssociateChildrenProperty(StorageAreaOperationDetailProperty.StorageAreaOperationProperty, c =>
            {
                var storageArea = c.Parent as StorageArea;
                var storagearealist = RT.Service.Resolve<WarehouseController>().GetStorageAreaOperationDetail(storageArea.Id);
                return storagearealist == null ? new StorageAreaOperation() { StorageArea = storageArea, StorageAreaId = storageArea.Id } : storagearealist;
            }).HasLabel("操作管理").OrderNo = 2;
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
            View.Property(p => p.IsFrozen).Show();
        }
    }
}
