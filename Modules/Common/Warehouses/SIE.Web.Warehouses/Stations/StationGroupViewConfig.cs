using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Warehouses.Stations;

namespace SIE.Web.Warehouses.Stations
{
    /// <summary>
    /// 站台组视图配置
    /// </summary>
    internal class StationGroupViewConfig : WebViewConfig<StationGroup>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.RemoveCommands(WebCommandNames.Copy);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.State);
            View.Property(p => p.StationGroupType);
            View.Property(p => p.TurnoverBoxModelId);
            View.Property(p => p.WarehouseId).UsePagingLookUpEditor(p => p.XType = "StationGroupWarehouseComboList");
            View.Property(p => p.StorageAreaId).UseDataSource((o, c, r) =>
            {
                var stationGroup = o as StationGroup;
                if (stationGroup == null || stationGroup.WarehouseId <= 0)
                {
                    return new EntityList<StorageArea>();
                }

                return RT.Service.Resolve<WarehouseController>().GetEnableStorageAreas(stationGroup.WarehouseId, r, c, true);
            });
            View.Property(p => p.Location);
            View.ChildrenProperty(p => p.StationGroupLineList);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Location);
            View.Property(p => p.WarehouseCode);
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.StationGroupType);
            View.Property(p => p.TurnoverBoxModelId);
            View.Property(p => p.WarehouseId);
            View.Property(p => p.Location);
        }
    }
}