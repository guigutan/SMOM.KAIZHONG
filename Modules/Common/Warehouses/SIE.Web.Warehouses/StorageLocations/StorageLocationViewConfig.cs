using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Common.Commands;
using SIE.Web.Warehouses.Commands;
using System.Collections.Generic;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 库位视图配置
    /// </summary>
    public class StorageLocationViewConfig : WebViewConfig<StorageLocation>
    {
        public const string routewayListView = "RoutewayListView";

        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(Routeway));
            View.DeclareExtendViewGroup(routewayListView);
            if (ViewGroup == routewayListView)
            {
                RoutewayListView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(StorageLocationAddCommand).FullName, WebCommandNames.Edit, typeof(StorageLocationDeleteCommand).FullName, WebCommandNames.Save);
            View.UseCommands(typeof(StorageLocationFrozenCommand).FullName, typeof(PrintStorageLocationCommand).FullName, typeof(StorageLocationImportCommand).FullName, typeof(StorageLocationRouteImportCommand).FullName);
            View.ReplaceCommands(EnableCommand.CommandName, typeof(StorageLocationEnableCommand).FullName);
            View.ReplaceCommands(DisableCommand.CommandName, typeof(StorageLocationDisableCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified)
                    .UseListSetting(e => { e.HelpInfo = "立库编码规则：Cell:排编号_层编号_列编码_深度_库区编码_仓库编码"; });
                View.Property(p => p.Name).ShowInList();
                View.Property(p => p.LibraryType).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified).Cascade(p => p.Warehouse, null).Cascade(p => p.Area, null)
                    .UseListSetting(e => { e.HelpInfo = "更改类型清空仓库/库区数据，修改模式不可编辑"; });
                View.Property(p => p.WarehouseId).UseAllWarehouseEditor().Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified)
                    .UseWarehouseLookUpEditor((p,r) =>
                    {
                        p.ReloadDataOnPopping = true;
                        Dictionary<string, string> keyValues = new Dictionary<string, string>();
                        keyValues.Add(nameof(r.WarehouseCode), nameof(r.Warehouse.Code));
                        p.DicLinkField = keyValues;
                        //p.DisplayField = Warehouse.CodeProperty.Name;
                        //p.BindDisplayField = "WarehouseCode";
                        //p.BindDisplayField = Warehouse.CodeProperty.Name;
                    }).Cascade(p => p.Area, null).Cascade(p => p.RoutewayId, null)
                    .UseListSetting(e => { e.HelpInfo = "更改仓库清空库区数据，修改模式不可编辑"; }).HasLabel("仓库名称");
                
                View.Property(p => p.AreaId).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified || p.WarehouseId == 0).
                    UseDataSource((source, pagingInfo, keyword) =>
                    {
                        var storageLocation = source as StorageLocation;
                        if (storageLocation != null)
                        {
                            return RT.Service.Resolve<WarehouseController>().GetStorageArea(null, storageLocation.WarehouseId, keyword, pagingInfo);
                        }
                        return new EntityList<StorageArea>();
                    }).UsePagingLookUpEditor((m, r) =>
                    {
                        Dictionary<string, string> keyValues = new Dictionary<string, string>();
                        keyValues.Add(nameof(r.IsAutomatedStorage), nameof(r.Area.IsAutomatedArea));
                        keyValues.Add(nameof(r.AreaCode), nameof(r.Area.Code));
                        m.DicLinkField = keyValues;
                        //m.DisplayField = StorageArea.CodeProperty.Name;
                        //m.BindDisplayField = "AreaCode";
                    }).Cascade(p => p.RoutewayId, null).UseListSetting(e => { e.HelpInfo = "当前仓库下库区，修改模式不可编辑"; }).HasLabel("库区名称");
                View.Property(p => p.WarehouseCode).Readonly().Show();
                View.Property(p => p.AreaCode).Readonly().Show();
                View.Property(p => p.IsAutomatedStorage).Readonly();
                View.Property(p => p.IsFrozen).Readonly();
                View.Property(p => p.State).Readonly();
                View.Property(p => p.ErpInvOrg);
                View.Property(p => p.ErpSubLibrary);
                View.Property(p => p.ErpLocation);
                View.Property(p => p.RoutewayId).Readonly(p => p.WarehouseId == 0 || p.AreaId == 0).UseDataSource((source, pagingInfo, keyword) =>
                      {
                          var storageLocation = source as StorageLocation;
                          if (storageLocation != null)
                          {
                              return RT.Service.Resolve<WarehouseController>().GetRouteways(storageLocation.WarehouseId, storageLocation.AreaId, keyword, pagingInfo);
                          }
                          return new EntityList<Routeway>();
                      }).UsePagingLookUpEditor((m, r) =>
                      {
                          m.ReloadDataOnPopping = true;
                      }).UseListSetting(e => { e.HelpInfo = "仓库库区有值才可编辑"; });
                View.Property(p => p.RowNo).UseSpinEditor(p => p.MinValue = 1).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified && false);
               
                View.Property(p => p.ColumnNo).UseSpinEditor(p => p.MinValue = 1).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified && false);
                View.Property(p => p.LayerNo).UseSpinEditor(p => p.MinValue = 1).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified && false);
                View.Property(p => p.Depth).UseSpinEditor(p => p.MinValue = 0).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified).UseListSetting(e => { e.HelpInfo = "深度从0开始"; });
                View.Property(p => p.IsMaxDepth);
                View.Property(p => p.IsInLock);
                View.Property(p => p.IsOutLock);
                View.Property(p => p.IsCountLock);
                View.Property(p => p.IsBackup);
                View.Property(DataEntity.UpdateDateProperty);
            }
            View.AssociateChildrenProperty(StorageLocationDetailProperty.BaseInfoProperty, (o) =>
            {
                var storageLocation = o.Parent as StorageLocation;
                var storageLocationInfo = RT.Service.Resolve<WarehouseController>().GetStorageLocationInfo(storageLocation.Id);
                return storageLocationInfo == null ? new StorageLocationInfo() { StorageLocation = storageLocation, StorageLocationId = storageLocation.Id } : storageLocationInfo;
            }, ViewConfig.DetailsView).HasLabel("基础资料").OrderNo = 1;
            View.AssociateChildrenProperty(StorageLocationDetailProperty.LayinInfoProperty, (o) =>
            {
                var storageLocation = o.Parent as StorageLocation;
                var storageLocationInfo = RT.Service.Resolve<WarehouseController>().GetStorageLocationLayinInfo(storageLocation.Id);
                return storageLocationInfo == null ? new StorageLocationLayinInfo() { StorageLocation = storageLocation, StorageLocationId = storageLocation.Id } : storageLocationInfo;
            }, ViewConfig.DetailsView).HasLabel("仓储资料").OrderNo = 2;
            View.AssociateChildrenProperty(StorageLocationDetailProperty.OperationInfoProperty, (o) =>
            {
                var storageLocation = o.Parent as StorageLocation;
                var storageLocationInfo = RT.Service.Resolve<WarehouseController>().GetStorageLocationOperation(storageLocation.Id);
                return storageLocationInfo == null ? new StorageLocationOperation() { StorageLocation = storageLocation, StorageLocationId = storageLocation.Id } : storageLocationInfo;
            }, ViewConfig.DetailsView).HasLabel("操作管理").OrderNo = 3;
            WebChildrenPropertyViewMeta childView = View.ChildrenProperty(p => p.StorageLocationItemListList);
            childView.OrderNo = 4;
            View.AssociateChildrenProperty(StorageLocationDetailProperty.StorageLocationFrozenReasonListProperty, (o) =>
            {
                var storageLocation = o.Parent as StorageLocation;
                var locFrozenReasons = RT.Service.Resolve<WarehouseController>().GetStorageLocationFrozenReasons(storageLocation.Id);
                return locFrozenReasons;
            }).HasLabel("冻结原因").OrderNo = 5;
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.LibraryType).Show();
            View.Property(p => p.WarehouseCode).DisableSort();
            View.Property(p => p.WarehouseName).Show().DisableSort();
            View.Property(p => p.AreaCode).Show().DisableSort();
            View.Property(p => p.AreaName).Show().DisableSort();
            View.Property(p => p.IsFrozen).Show();
        }

        /// <summary>
        /// 导入模板
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.WarehouseCode).Show();
            View.Property(p => p.AreaCode).Show();
            View.Property(p => p.RowNo).Show();
            View.Property(p => p.LayerNo).Show();
            View.Property(p => p.ColumnNo).Show();
            View.Property(p => p.Depth).Show();
            View.Property(p => p.RoutewayId).Show().HasLabel("巷道编码");
            View.Property(p => p.Code).Show().HasLabel("库位编码");
            View.Property(p => p.Name).Show().HasLabel("库位名称");
            View.Property(p => p.IsMaxDepth).Show();
            View.Property(p => p.Weight).Show();
            View.Property(p => p.Long).Show();
            View.Property(p => p.Width).Show();
            View.Property(p => p.Height).Show();
        }

        /// <summary>
        /// 巷道使用的附加视图
        /// </summary>
        protected void RoutewayListView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.LibraryType).Show();
                View.Property(p => p.AreaId).Show();
                View.Property(p => p.IsAutomatedStorage).Readonly().Show();
                View.Property(p => p.IsFrozen).Readonly().Show();
                View.Property(p => p.RowNo).Show();
                View.Property(p => p.LayerNo).Show();
                View.Property(p => p.ColumnNo).Show();
                View.Property(p => p.Depth).Show();
                View.Property(p => p.IsMaxDepth).Show();
                View.Property(p => p.IsInLock).Show();
                View.Property(p => p.IsOutLock).Show();
                View.Property(p => p.IsCountLock).Show();
                View.Property(p => p.IsBackup).Show();
            }
            View.ChildrenProperty(p => p.StorageLocationItemListList).Show(ChildShowInWhere.Hide);
        }
    }
}
