using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Warehouses.Commands;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 巷道基础数据表视图配置
    /// </summary>
    internal class RoutewayViewConfig : WebViewConfig<Routeway>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            // 配置视图
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(RoutewayAddCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            View.UseCommands(typeof(RoutewayImportCommand).FullName);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified);
            View.Property(p => p.Name);
            View.Property(p => p.RoutewayNumber).UseSpinEditor(p => p.MinValue = 1);
            View.Property(p => p.WarehouseId).UseWarehouseEditor().Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified).Cascade(p => p.StorageAreaId, null);
            View.Property(p => p.StorageAreaId).Readonly(p => p.PersistenceStatus == PersistenceStatus.Modified || p.WarehouseId == 0).
                UseDataSource((source, pagingInfo, keyword) =>
                {
                    var storageLocation = source as Routeway;
                    if (storageLocation != null)
                    {
                        return RT.Service.Resolve<WarehouseController>().GetStorageArea(null, storageLocation.WarehouseId, keyword, pagingInfo);
                    }

                    return new EntityList<StorageArea>();
                }).UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true);
            View.Property(p => p.Description);
            View.AttachChildrenProperty(typeof(StorageLocation), (w) =>
            {
                var arg = w as ChildPagingDataArgs;
                var entity = arg.Parent as Routeway;
                if (entity == null)
                    return new EntityList<StorageLocation>();
                return RT.Service.Resolve<WarehouseController>().GetStorageLocationByRouteway(entity.Id, arg.PagingInfo);
            }, StorageLocationViewConfig.routewayListView, true).Show(ChildShowInWhere.All);
        }

        /// <summary>
        /// 导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.WarehouseCode).Show();
            View.Property(p => p.StorageAreaCode).Show();
            View.Property(p => p.RoutewayNumber).Show();
            View.Property(p => p.Description).Show();
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.Description).Show();
            View.Property(p => p.RoutewayNumber);
            View.Property(p => p.WarehouseId);
            View.Property(p => p.StorageAreaId);
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.WarehouseId).Show();
            View.Property(p => p.StorageAreaId).Show();
        }
    }
}
