using SIE.Domain;
using SIE.Kit.MES.Storages;
using SIE.MetaModel.View;
using SIE.Web.Resources;
using System.Collections.Generic;

namespace SIE.Web.Kit.MES.Storages
{
    /// <summary>
    /// 产线货区视图配置
    /// </summary>
    internal class StorageAreaViewConfig : WebViewConfig<StorageArea>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            //方法重写
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().UseImportCommands();
            View.ReplaceCommands(WebCommandNames.Copy, "SIE.Web.Kit.MES.Storages.Commands.StorageAreaCopyCommand");
            View.Property(p => p.Code).ShowInList(150).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name).ShowInList(150);
            View.Property(p => p.Type);
            View.Property(p => p.Warehouse).HasLabel("仓库");
            View.Property(p => p.Factory).UseFactoryEditor();
            View.Property(p => p.IsMixItem);
            View.Property(p => p.State).Readonly();
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
            View.AssociateChildrenProperty(StationStorageExtStorageArea.StationStorageAreaExtListProperty, (e) =>
            {
                var args = e as ChildPagingDataArgs;
                var storageArea = e.Parent as StorageArea;
                var stationStorageArea = RT.Service.Resolve<StorageController>().GetStationAreas(storageArea.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return stationStorageArea;
            }).HasLabel("产线工位").OrderNo = 10; ////工位
            View.AssociateChildrenProperty(StorageLocationExtStorageArea.StorageLocationExtListProperty, (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var storageArea = w.Parent as StorageArea;
                var storageLocation = RT.Service.Resolve<StorageController>().GetStorageLocations(storageArea.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return storageLocation;
            }).HasLabel("货位").OrderNo = 20; ////货位
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
            View.Property(p => p.Factory);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type);
            View.Property(p => p.Warehouse);
            View.Property(p => p.Factory);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Type);
            View.Property(p => p.Factory.Code).HasLabel("工厂编码");
            View.PropertyRef(p => p.Warehouse.Code).HasLabel("仓库编码");
        }
    }
}