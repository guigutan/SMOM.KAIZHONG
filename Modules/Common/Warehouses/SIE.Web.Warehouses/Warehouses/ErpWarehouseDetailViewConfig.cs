using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.Warehouses.Warehouses.Commands;
using System.Collections.Generic;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// Erp子库 视图配置
    /// </summary>
    internal class ErpWarehouseDetailViewConfig : WebViewConfig<ErpWarehouseDetail>
    {        
        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {           
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);         
            View.Property(p => p.WarehouseId).UseAllWarehouseEditor()
                  .UsePagingLookUpEditor(p => p.ReloadDataOnPopping = true).Cascade(p => p.AreaId, null)
                  .UseListSetting(e => { e.HelpInfo = "更改仓库清空库区数据"; });
            View.Property(p => p.AreaId).Readonly(p => p.WarehouseId == 0).
                UseDataSource((source, pagingInfo, keyword) =>
                {
                    var storageLocation = source as ErpWarehouseDetail;
                    if (storageLocation != null)
                    {
                        return RT.Service.Resolve<WarehouseController>().GetStorageArea(null, storageLocation.WarehouseId, keyword, pagingInfo);
                    }
                    return new EntityList<StorageArea>();
                }).Cascade(p => p.StorageLocationId, null).UseListSetting(e => { e.HelpInfo = "当前仓库下库区"; });

            View.Property(p => p.StorageLocationId).Readonly(p => p.AreaId == 0 || p.AreaId == null).
                UseDataSource((source, pagingInfo, keyword) =>
                {
                    var storageLocation = source as ErpWarehouseDetail;
                    if (storageLocation != null)
                    {
                        return RT.Service.Resolve<WarehouseController>().GetEnableStorageLocations(storageLocation.AreaId, storageLocation.WarehouseId, keyword, pagingInfo);
                    }
                    return new EntityList<StorageLocation>();
                });            
        }        
    }
}
