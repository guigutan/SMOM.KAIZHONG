using SIE.Domain;
using SIE.MES.Storages;
using SIE.Wpf.Command;
using SIE.Wpf.MES.Storages.Commands;
using System.Collections.Generic;

namespace SIE.Wpf.MES.Storages
{
    /// <summary>
    /// 产线货区视图配置
    /// </summary>
    internal class StorageAreaViewConfig : WPFViewConfig<StorageArea>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.ReplaceCommands(typeof(ListCopyCommand), typeof(StorageAreaCopyCommand));
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type);
            View.Property(p => p.WarehouseId).HasLabel("仓库");
            View.AssociateChildrenProperty(StationStorageExtStorageArea.StationStorageAreaExtListProperty, (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var storageArea = e.Parent as StorageArea;
                var stationStorageArea = RT.Service.Resolve<StorageController>().GetStationAreas(storageArea.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return stationStorageArea;
            }, StationStorageAreaViewConfig.ListView).HasLabel("产线工位货区").OrderNo = 10; ////工位
            View.AssociateChildrenProperty(StorageLocationExtStorageArea.StorageLocationExtListProperty, (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var storageArea = args.Parent as StorageArea;
                var storageLocation = RT.Service.Resolve<StorageController>().GetStorageLocations(storageArea.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return storageLocation;
            }).HasLabel("产线货区货位").OrderNo = 20; ////货位
            View.AssociateChildrenProperty(StorageSaftyExtStorageArea.StorageSaftyExtListProperty, (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var storageArea = args.Parent as StorageArea;
                var storageSafty = RT.Service.Resolve<StorageController>().GetStorageSaftys(storageArea.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return storageSafty;
            }).HasLabel("物料库存").OrderNo = 30;
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type);
            View.Property(p => p.WarehouseName);
        }
    }
}